using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
****   在Mono脚本中，使用特性展示本脚本的共有属性，并简单编辑Inspector面板  
***/

[AddComponentMenu("Player/Normal Inspector Show"),DisallowMultipleComponent]//Component菜单中可以添加本脚本，不允许一个物体挂多个本脚本
public class PlayerNormal : MonoBehaviour {
    [Header("基本信息"),Delayed]  //绘制标题信息
    public string playerName = "";
    [Delayed] //本字段回车或确认后才能生效
    public int playerAge = 18;
    [Range(10,240)] //下面一个字段的值可以通过slider控制
    public float playerWeight = 60f;
    public Texture headIcon = null;
    public Color skinColor = Color.white;
}
