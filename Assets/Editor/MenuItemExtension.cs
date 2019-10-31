using UnityEngine;
using UnityEditor;

/***
****   使用MenuItem特性实现菜单选项的扩展  
***/

public class MenuItemExtension
{
    #region 删除菜单选项
    //参数2为true，代表这是相同标签中第一个执行的验证有效性的函数
    [MenuItem("Tools/Delete Objects", true)]
    static bool DeleteValidate()
    {
        return Selection.gameObjects.Length > 0 ? true : false;
    }

    [MenuItem("Tools/Delete Objects", false,1)]
    private static void MyToolDelete()
    {
        //Selection.objects 返回场景或者Project中选择的多个对象
        foreach (GameObject item in Selection.gameObjects)
        {
            //记录删除操作，允许撤销；使用Selection.gameObjects删除Assets中的prefab有bug
            Undo.DestroyObjectImmediate(item);   
        }
    }
    #endregion

    #region 指定脚本增加右键内容
    [MenuItem("CONTEXT/MenuContextTarget/PlayerPropertiesInit",false)]
    static void PlayerPropertiesInit(MenuCommand cmd)
    {
        MenuContextTarget playerProperties = cmd.context as MenuContextTarget;
        playerProperties.playerName = "";
        playerProperties.playerAge = 0;
        playerProperties.playerTel = "";
    }
    #endregion

}