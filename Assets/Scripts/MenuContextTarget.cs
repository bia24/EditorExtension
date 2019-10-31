using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
****   在Mono脚本中，使用特性添加本脚本的右键菜单项  
***/

public class MenuContextTarget : MonoBehaviour {
    public string playerName = "";
    public int playerAge=0;
    [ContextMenuItem("GetNiceTel", "GetNiceTel")] //给字段添加右键菜单
    public string playerTel = "13158587777";

    [ContextMenu("SetExampleProperties",false)]//将该方法添加到脚本的右键菜单中
    private void SetExampleProperties()
    {
        playerName = "Flash";
        playerAge = 28;
        playerTel = "1588888888";
    }

    private void GetNiceTel()
    {
        playerTel = "1588888888";
    }
}
