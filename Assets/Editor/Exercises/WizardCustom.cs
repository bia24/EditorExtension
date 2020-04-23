using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/***
****   编辑器脚本，设计一个向导型小窗口，功能为可批量修改某脚本的公有变量值 
***/


public class WizardCustom :ScriptableWizard{

    [MenuItem("Tools/Change Player Age and Weight",false,40)]
    static void ShowWizardWindow() //唤出本窗口的命令
    {
        DisplayWizard<WizardCustom>("Player Age and Weight Multi-change", "change and close", "change");
    }


    #region properties
    public int changeAgeValue = 1;
    public float changeWeightValue = 1f;
    private bool valid = true;
    #endregion


    //在本窗口打开时，验证游戏对象选择状况，并显示对应信息
    //OnEnable会在Wizard打开，和编辑模式更换时调用
    private void OnEnable() 
    {
        valid = CheckGameObjectCount();
        if (valid)
        {
            helpString = "You have selected " + Selection.gameObjects.Length + " GameObjects.";
            errorString = null;
        }
        else
        {
            errorString = "No GameObject selected!";
            helpString = null;
        }
    }

    //当选择对象变换时候，也进行检测和信息输出
    private void OnSelectionChange()
    {
        valid = CheckGameObjectCount();
        if (valid)
        {
            helpString = "You have selected " + Selection.gameObjects.Length + " GameObjects.";
            errorString = null;
        }
        else
        {
            errorString = "No GameObject selected!";
            helpString = null;
        }
    }

    //Wizard向导只有两个按钮："create"和"other"
    //create 执行完会退出
    private void OnWizardCreate()
    {
        GameObject[] gobjs = Selection.gameObjects;
        foreach (GameObject obj in gobjs)
        {
            Player player = obj.GetComponent<Player>();
            if (player != null)
            {
                Undo.RecordObject(player, "undo value change");//跟着即将更改的属性使用
                player.playerAge += changeAgeValue;
                player.playerWeight += changeWeightValue;
            }
        }
    }


    //other 执行完不会退出
    private void OnWizardOtherButton()
    {
        GameObject[] gobjs = Selection.gameObjects;
        int count = 0;
        foreach (GameObject obj in gobjs)
        {
            Player player = obj.GetComponent<Player>();
            if (player != null)
            {
                Undo.RecordObject(player, "undo value change");
                player.playerAge += changeAgeValue;
                player.playerWeight += changeWeightValue;
                count++;
            }
        }
        //展示一个透明显示框
        ShowNotification(new GUIContent("You have changed " + count + " GameObjects value"));
    }



    //检测当前是否有选择游戏对象
    private bool CheckGameObjectCount()
    {
        return Selection.gameObjects.Length == 0 ? false : true;
    }
}
