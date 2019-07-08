using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoShuttle : MonoBehaviour {
    /*
    Create Gizmo
     */
    public enum Type { SPWAN, PATROL }
    // Choose Type
    public Type type = Type.SPWAN;
    public Color _color = Color.cyan;
    // Basic Type : SPWAN
    // Basic Color : Cyan

    public float _radius = 0.1f;
    
    private void OnDrawGizmos()
    {
        if (type == Type.SPWAN)
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
        }
        else
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
        }
        /*
        SPWANとPATROLの動作を設定します。
         */
    }
}
