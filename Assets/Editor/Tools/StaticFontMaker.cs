using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class StaticFontMaker:EditorWindow
{
    private TextAsset fontFile=null;
    private Texture fontTexure = null;

    private int lineHeight;
    private int texW;
    private int texH;


    public StaticFontMaker()
    {
        this.titleContent = new GUIContent("StaticFontMaker");
    }

    [MenuItem("Tools/Static Font Maker")]
    static void ShowWindow()
    {
        GetWindowWithRect(typeof(StaticFontMaker), new Rect(new Vector2(1000, 1000), new Vector2(400, 350)));
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUILayout.Label("StaticFontMaker");


        //绘制.fnt 文件 选择框
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.LabelField(".fnt file", GUILayout.Width(120));
        fontFile = EditorGUILayout.ObjectField(fontFile, typeof(TextAsset), true, GUILayout.Width(200)) as TextAsset;
        GUILayout.EndHorizontal();

        //绘制 texture 选择框
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.LabelField("font texture", GUILayout.Width(120));
        fontTexure = EditorGUILayout.ObjectField(fontTexure, typeof(Texture), true, GUILayout.Width(200)) as Texture;
        GUILayout.EndHorizontal();

        //绘制“Create Font”按钮
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(120);
        if (GUILayout.Button("Create Font", GUILayout.Width(180), GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.5f)))
        {
            if (fontFile==null)
            {
                EditorUtility.DisplayDialog("Error", ".fnt file not a text", "Cancle");
            }
            else if(fontTexure==null)
            {
                EditorUtility.DisplayDialog("Error", "fntTexure not a texure", "Cancle");
            }
            else
            {
                EditorUtility.DisplayDialog("Tips", "Create Font Successful\n 生成的Font和mat在.fnt文件目录下", "Ok");
                CreateFont();
            }
        }
        GUILayout.EndHorizontal();


        GUILayout.EndVertical();
    }


    private void CreateFont() {

        //获得.fnt文件所在目录
        string storePath= AssetDatabase.GetAssetPath(fontFile);
        string fontName = fontFile.name;

        string config = null;
        using (StreamReader stream = new StreamReader(storePath))
        {
             config = stream.ReadToEnd();
        }
           

        //创建material
        Material mat = new Material(Shader.Find("GUI/Text Shader"));

        //设置material的纹理
        mat.SetTexture("_MainTex", fontTexure);

        //创建Font
        Font font = new Font();

        //设置font文件的属性
        try
        {
            SetFontProperties(ref font, fontName, mat, config);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
            return;     
        }
        //存储mat和font
        AssetDatabase.CreateAsset(mat, storePath.Replace(".fnt", ".mat"));
        AssetDatabase.SaveAssets();
        Material matAsset = AssetDatabase.LoadAssetAtPath<Material>(storePath.Replace(".fnt", ".mat"));
        font.material = matAsset;
        AssetDatabase.CreateAsset(font, storePath.Replace(".fnt", ".fontsettings"));
        AssetDatabase.SaveAssets();
        
    }

    private struct Character
    {
       public int id;
        public int uvX;
        public int uvY;
        public int width;
        public int height;
        public int xoffset;
        public int yoffset;
        public int xadvance;
    }

    private void SetFontProperties(ref Font f,string name,Material mat,string cfg)
    {
        f.name = name;

        //从配置文件中读取配置
        List<Character> charas = new List<Character>();
        ParseConfig(charas, cfg);

        SetFontCharacters(ref f, charas);

    }

    private void ParseConfig(List<Character> charas,string cfg)
    {
        string[] lines = cfg.Split('\n');
        foreach(string line in lines)
        {
            if(line.StartsWith("char id="))
            {
                Character c = new Character();

                c.id = GetConfigValue(line, "char id=");
                c.uvX = GetConfigValue(line, "x=");
                c.uvY = GetConfigValue(line, "y=");
                c.width = GetConfigValue(line, "width=");
                c.height = GetConfigValue(line, "height=");
                c.xoffset = GetConfigValue(line, "xoffset=");
                c.yoffset = GetConfigValue(line, "yoffset=");
                c.xadvance = GetConfigValue(line, "xadvance=");

                charas.Add(c);
            }
            else if (line.StartsWith("common"))
            {
                lineHeight = GetConfigValue(line, "lineHeight=");
                texW = GetConfigValue(line, "scaleW=");
                texH = GetConfigValue(line, "scaleH=");
            }
        }
    }

    private int GetConfigValue(string line,string key)
    {
        int startPos = line.IndexOf(key);
        startPos += key.Length;
        string subStr = line.Substring(startPos);
        int i = 0;
        while (!subStr[i].Equals(' '))
            i++;
        subStr = subStr.Substring(0, i);
        try
        {
            int res = int.Parse(subStr);
            return res;
        }catch
        {
            throw new System.Exception("Error string: " + subStr+" | "+"Error key: "+key);
        }
    }

    private void SetFontCharacters(ref Font f,List<Character>charas)
    {
        int size = charas.Count;
        if (size == 0) { throw new System.Exception("not fount character"); }

        List<CharacterInfo> cis = new List<CharacterInfo>();
        foreach(Character c in charas)
        {
            CharacterInfo ci = new CharacterInfo();
            float uvx = (float)c.uvX / texW;
            float uvy = 1-(float)(c.uvY+c.height) / texH;
            float uvw = (float)c.width / texW;
            float uvh = (float)c.height / texH;

            ci.uvBottomLeft = new Vector2(uvx, uvy);
            ci.uvBottomRight = new Vector2(uvx + uvw, uvy);
            ci.uvTopLeft = new Vector2(uvx, uvy + uvh);
            ci.uvTopRight = new Vector2(uvx + uvw, uvy + uvh);

            //这里的设置很迷，至今未搞懂。以下配置是较合理的一种状态。
            ci.minX = c.xoffset;
            //ci.maxX = c.xoffset;
            //ci.minY = c.yoffset;
            ci.maxY = c.yoffset;
            ci.glyphWidth = c.width;
            ci.glyphHeight = c.height;
            ci.advance = c.xadvance;

            ci.index = c.id;

            cis.Add(ci);
        }

        f.characterInfo = cis.ToArray();
        //将font序列化，设置行高
        SerializedObject serializedFont = new SerializedObject(f);
        serializedFont.FindProperty("m_LineSpacing").floatValue = lineHeight;
        serializedFont.ApplyModifiedProperties();
    }


}

