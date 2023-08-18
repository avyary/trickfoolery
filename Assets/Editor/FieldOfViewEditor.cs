using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//*******************************************************************************************
// FieldOfViewEditor
//*******************************************************************************************
/// <summary> TODO:
/// Not an editor guru, but I'm guessing it acts like OnDrawGizmos() to draw the 
/// FieldOfView boundaries in the scene viewport...
/// </summary>
[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI() {
        FieldOfView fow = (FieldOfView) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.detectRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
    }
}
