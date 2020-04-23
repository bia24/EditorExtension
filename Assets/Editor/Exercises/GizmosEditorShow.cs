using UnityEditor;
using UnityEngine;

/***
****   在Editor文件夹中的脚本使用特性展示Gizmos自定义效果  
* **    目标物体上需要绑定脚本
***/

public class GizmosEditorShow{
    [DrawGizmo(GizmoType.Active|GizmoType.Selected,typeof(GizmosTarget))]
    static void GizmosShow(GizmosTarget target,GizmoType gizmoType)
    {
        var color = Gizmos.color;
        Gizmos.color = Color.yellow;
        Gizmos.matrix = target.transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one * 1.5f);
        Gizmos.color = color;
    }
}
