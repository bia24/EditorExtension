using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/***
****   在Mono脚本中使用OnDrawGizmosSelected展示Gizmos自定义效果  
***/

public class GizmosMonoShow : MonoBehaviour {

    private void OnDrawGizmosSelected()
    {
        Color color = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;//模型空间转换到世界空间，设置Gizmos的变换矩阵
        Gizmos.DrawCube(Vector3.zero, Vector3.one*1.2f);//相当于模型空间坐标数据
        Gizmos.color = color;
    }

}
