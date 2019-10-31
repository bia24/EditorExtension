using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
****   在Mono脚本中使用OnDrawGizmos展示Camera视锥体效果  
***/

public class GizmosMonoShowCamera : MonoBehaviour {

    private void OnDrawGizmos()
    {
        var color = Gizmos.color;
        Gizmos.color = Color.red;
        Camera camera = GetComponent<Camera>();
        Gizmos.matrix = transform.localToWorldMatrix;
        //Matrix4x4.TRS(transform .position, camera.transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(transform.position, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
        Gizmos.color = color;
    }

}
