using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

/***
****   编辑器脚本，重新绘制Player的Inspector面板  
***/
#pragma warning disable 0219
[CustomEditor(typeof(Player))] //自定义检视面板的特性说明
[CanEditMultipleObjects]
public class PlayerInspector :Editor{

    #region properties
    SerializedProperty playerName = null;
    SerializedProperty playerAge = null;
    SerializedProperty playerTel = null;
    SerializedProperty playerWeight = null;
    SerializedProperty headIcon = null;
    SerializedProperty skinColor = null;
    ReorderableList playerIDs = null;
    ReorderableList playerSkills = null;

    SerializedProperty enable = null;
    SerializedProperty selection = null;
    
    string[] selections = null;
    #endregion


    private void OnEnable()
    {
        
        #region FindProperty
        //获取目标对象的对应字段属性
        playerName = serializedObject.FindProperty("playerName");
        playerAge = serializedObject.FindProperty("playerAge");
        playerTel = serializedObject.FindProperty("playerTel");
        playerWeight = serializedObject.FindProperty("playerWeight");
        headIcon = serializedObject.FindProperty("headIcon");
        skinColor = serializedObject.FindProperty("skinColor");
        playerIDs = new ReorderableList(serializedObject,serializedObject.FindProperty("playerIDs"),true,true,true,true);
        playerSkills = new ReorderableList(serializedObject, serializedObject.FindProperty("playerSkills"), true, true, true, true);

        //用于控制的私有字段获取
        enable = serializedObject.FindProperty("enable");
        selection = serializedObject.FindProperty("selection");
        selections = new string[3] { "selection_01", "selection_02", "selection_03" };
        #endregion
        
        #region ReorderableList Callback set

        //可排序列表的每行元素高度
        playerSkills.elementHeight = 100;



        //列表标题设置
        playerIDs.drawHeaderCallback = (Rect rect) =>
        {
            GUI.skin.label.fontSize = 10;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.fontStyle = FontStyle.Normal;
            GUI.Label(rect, "玩家拥有账号ID列表");
        };

        playerSkills.drawHeaderCallback = (Rect rect) =>
        {
            GUI.skin.label.fontSize = 10;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.fontStyle = FontStyle.Normal;
            GUI.Label(rect, "技能列表");
        };



        //列表中的每行元素绘制回调设置
        playerIDs.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
        {
            //根据index获取对应元素
            SerializedProperty item = playerIDs.serializedProperty.GetArrayElementAtIndex(index);
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.y += 2;
            EditorGUI.PropertyField(rect, item, new GUIContent("ID " + index));
        };

        playerSkills.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
        {
            //根据index获取对应元素
            SerializedProperty item = playerSkills.serializedProperty.GetArrayElementAtIndex(index);
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.y += 2;
            EditorGUI.PropertyField(rect, item, new GUIContent("Charactor" + index));
        };



        //添加新元素时的回调函数
        playerIDs.onAddCallback = (ReorderableList list) =>
        {
            if (list.serializedProperty != null)
            {
                list.serializedProperty.arraySize++;
                list.index = list.serializedProperty.arraySize - 1;
                SerializedProperty item = list.serializedProperty.GetArrayElementAtIndex(list.index);
                item.stringValue = "";//新建元素的初始值
            }
            else
            {
                ReorderableList.defaultBehaviours.DoAddButton(list);
            }
        };
        playerSkills.onAddCallback = (ReorderableList list) =>
        {

            if (list.serializedProperty != null)
            {
                list.serializedProperty.arraySize++;
                list.index = list.serializedProperty.arraySize - 1;
                SerializedProperty item = list.serializedProperty.GetArrayElementAtIndex(list.index);
                //item.FindPropertyRelative("xxx").stringValue = "";可以设置复杂元素的初始值
            }
            else
            {
                ReorderableList.defaultBehaviours.DoAddButton(list);
            }
        };




        //当删除元素时候的回调函数，实现删除元素时，有提示框跳出
        playerIDs.onRemoveCallback = (ReorderableList list) =>
        {
            if (EditorUtility.DisplayDialog("Warnning", "Do you want to remove this element?", "Remove", "Cancel"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
            }
        };
        playerSkills.onRemoveCallback = (ReorderableList list) =>
        {
            if (EditorUtility.DisplayDialog("Warnning", "Do you want to remove this element?", "Remove", "Cancel"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
            }
        };
        #endregion
    }

    //重写Inspector进行绘制
    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI(); 使用原本的GUI进行绘制
        serializedObject.Update(); //最初更新绑定的序列化对象
        EditorGUILayout.BeginVertical();

        GUILayout.Space(10);
        //标题
        EditorGUILayout.LabelField("玩家基本信息",new GUIStyle() { fontStyle=FontStyle.Bold});

        /******EditorGUIUtility.labelWidth = 50;*****/
        //对于lable和field一起显示的控件，上述命令可以控制lable标签的统一宽度，该自动标签可以通过GUIContext.None参数取消；
        //若将lable和field分成两部分并排显示，上述的命令无法控制；
        //下面的例子对于两种方式都有运用；

        //Head Icon
        //EditorGUILayout.ObjectField(headIcon); // warning:头像框目前无能为力，在多选操作和显示图片上的问题；
        EditorGUILayout.PropertyField(headIcon);
        GUILayout.Space(2);
        EditorGUILayout.BeginHorizontal();
        //playerName
        EditorGUILayout.LabelField("Name", GUILayout.MaxWidth(40));
        EditorGUILayout.DelayedTextField(playerName,GUIContent.none);
        

        //playerAge
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Age", GUILayout.MaxWidth(30));
        EditorGUILayout.DelayedIntField(playerAge, GUIContent.none);
        EditorGUILayout.EndHorizontal();

        //playerTel
        GUILayout.Space(2);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Tele", GUILayout.MaxWidth(40));
        EditorGUILayout.DelayedTextField(playerTel,GUIContent.none);
        EditorGUILayout.EndHorizontal();

        //playerWeight
        GUILayout.Space(2);
        EditorGUILayout.Slider(playerWeight, 20f, 180f, new GUIContent("Weight(Kg)"));
        if (playerWeight.floatValue < 30f)
        {
            EditorGUILayout.HelpBox("player is too slight.", MessageType.Info);
        }
        else if (playerWeight.floatValue > 150f)
        {
            EditorGUILayout.HelpBox("player is too fat...", MessageType.Warning);
        }


        //playerSkinColor
        GUILayout.Space(2);
        Color color = GUI.color;
        //GUI.color = skinColor.colorValue; 
        //skinColor.colorValue = EditorGUILayout.ColorField("Skin Color", skinColor.colorValue);
        /*更新：使用 EditorGUILayout.PropertyField()，这个是万能框，解决多选bug问题。*/
        EditorGUILayout.PropertyField(skinColor);
        GUILayout.Space(2);
        if (!skinColor.hasMultipleDifferentValues)
        {
            Rect lastColorRect = GUILayoutUtility.GetRect(10, 20);
            EditorGUI.ProgressBar(lastColorRect, 1f, "进度条的使用");
        }
        GUI.color = color;


        GUILayout.Space(10);
        EditorGUILayout.LabelField("游戏角色信息", new GUIStyle() { fontStyle = FontStyle.Bold });

        //可控区域勾选框
        if (!enable.hasMultipleDifferentValues)
        {
            GUILayout.Space(2);
            EditorGUILayout.BeginHorizontal();
            //error : BeginToggleGroup不存在使用SerializedProperty的API，因此在多选上存在Bug
            enable.boolValue = EditorGUILayout.BeginToggleGroup("自定义游戏职业", enable.boolValue);

            //selection，控制职业pop框选择
            if (!selection.hasMultipleDifferentValues)
            {
                selection.intValue = EditorGUILayout.Popup(selection.intValue, selections);//error : Popup不存在使用SerializedProperty的API，因此在多选上存在Bug
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndToggleGroup();
            if (!selection.hasMultipleDifferentValues)
            {
                GUILayout.Space(2);
                if (enable.boolValue)
                {
                    EditorGUILayout.HelpBox("你选择了第 " + (selection.intValue + 1) + " 种自定义职业", MessageType.Info);
                }
            }
        }

        //账号playerIDs列表
        GUILayout.Space(5);
        playerIDs.DoLayoutList();

        //角色技能skill列表
        GUILayout.Space(5);
        playerSkills.DoLayoutList();

        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
}
