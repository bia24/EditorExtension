using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.IO;

/***
****   编辑器脚本，设计一个持久化窗口，功能为bug信息存储 
***/

public class WindowCustom : EditorWindow {

    #region properties

    private string bugReportName = "";

    private GameObject buggyGameObject = null;

     private string bugDescription = "";

    #endregion


    [MenuItem("Tools/Bug Reporter",false,20)] 
    static void ShowWindow()//唤出本窗口的命令
    {
        GetWindow(typeof(WindowCustom));
    }

    public WindowCustom() //构造函数中设置标题等
    {
        titleContent = new GUIContent("Bug Reporter");
    }

    private void OnGUI() //使用OnGUI消息函数来绘制
    {
        //使用GUILayout中的函数来绘制；EditorGUILayout也能绘制，但这更偏向于编辑器输入的那种
        GUILayout.BeginVertical();

        //绘制标题
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUILayout.Label("Bug Reporter");


        //绘制bugName输入框  
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Bug Name", GUILayout.Width(120));
        bugReportName = EditorGUILayout.TextField(bugReportName, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        //显示当前正在编辑的场景名称
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Scene Name", GUILayout.Width(120));
        EditorGUILayout.LabelField(EditorSceneManager.GetActiveScene().name, GUILayout.Width(200));
        // GUILayout.Label("Scene Name");//也可以
        GUILayout.EndHorizontal();

        //显示当前的时间
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Time Now", GUILayout.Width(120));
        EditorGUILayout.LabelField(System.DateTime.Now.ToString(), GUILayout.Width(200));
        GUILayout.EndHorizontal();

        //绘制对象选择框
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.LabelField("BuggyGameObject", GUILayout.Width(120));
        buggyGameObject = (GameObject)EditorGUILayout.ObjectField(buggyGameObject, typeof(GameObject), true, GUILayout.Width(200));
        GUILayout.EndHorizontal();


        //绘制bug描述文本框
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Bug Description", GUILayout.Width(120));
        //bugDescription = EditorGUILayout.TextArea(bugDescription, GUILayout.MaxHeight(75),GUILayout.Width(200));
        bugDescription = GUILayout.TextArea(bugDescription, GUILayout.Height(75), GUILayout.Width(200));
        GUILayout.EndHorizontal();


        //添加“Save Bug”按钮
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(120);
        if (GUILayout.Button("Save Bug", GUILayout.Width(180), GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.5f)))
        {
            if (bugReportName.Equals(""))
            {
                EditorUtility.DisplayDialog("Warnning", "Bug Name is empty！", "Cancle");
            }
            else
            {
                SaveBug();
                EditorUtility.DisplayDialog("Tips", "Save Bug Successfully!", "Ok");
            }
        }
        GUILayout.EndHorizontal();


        //添加“Save Bug with Screenshot”按钮
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(120);
        if (GUILayout.Button("Save Bug with Screenshot", GUILayout.Width(180), GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.5f)))
        {
            if (bugReportName.Equals(""))
            {
                EditorUtility.DisplayDialog("Warnning", "Bug Name is empty！", "Cancle");
            }
            else
            {
                SaveBugWithScreenshot();
                EditorUtility.DisplayDialog("Tips", "Save Bug with Screenshot Successfully!", "Ok");
            }
        }
        GUILayout.EndHorizontal();




        GUILayout.EndVertical();

    }

    //保存Bug的方法
    private void SaveBug()
    {
        DateTime nowTime = DateTime.Now;
        string dirPath = Application.dataPath + "/Editor/BugReporter" + "/" + bugReportName + "_" + nowTime.ToString("yyyyMMddHHmmssffff");
        string filePath = dirPath + "/" + bugReportName + ".txt";
        Directory.CreateDirectory(dirPath);
        StreamWriter sw = new StreamWriter(filePath);
        sw.WriteLine("Bug Name : " + bugReportName);
        sw.WriteLine("Time : " + nowTime);
        if (buggyGameObject != null)
            sw.WriteLine("GameObject Name : " + buggyGameObject.name);
        else
            sw.WriteLine("GameObject Name : " + "null");
        sw.WriteLine(EditorSceneManager.GetActiveScene().name);
        sw.Write(bugDescription);
        sw.Close();
    }
    //保存Bug截图的方法
    private void SaveBugWithScreenshot()
    {
        DateTime nowTime = DateTime.Now;
        string dirPath = Application.dataPath + "/Editor/BugReporter" + "/" + bugReportName + "_" + nowTime.ToString("yyyyMMddHHmmssffff");
        string filePath = dirPath + "/" + bugReportName + ".txt";
        Directory.CreateDirectory(dirPath);
        StreamWriter sw = new StreamWriter(filePath);
        sw.WriteLine("Bug Name : " + bugReportName);
        sw.WriteLine("Time : " + nowTime);
        if (buggyGameObject != null)
            sw.WriteLine("GameObject Name : " + buggyGameObject.name);
        else
            sw.WriteLine("GameObject Name : " + "null");
        sw.WriteLine(EditorSceneManager.GetActiveScene().name);
        sw.Write(bugDescription);
        sw.Close();
        ScreenCapture.CaptureScreenshot(dirPath + "/" + bugReportName + ".png");
    }

}
