using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//*******************************************************************************************
// PlayerMovementEditor
//*******************************************************************************************
/// <summary> TODO:
/// 
/// </summary>
[CustomEditor (typeof (PlayerMovement))]
public class PlayerMovementEditor: Editor
{
    private void OnSceneGUI() {
        PlayerMovement player = (PlayerMovement) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(player.transform.position, Vector3.up, Vector3.forward, 360, player.dodgeRadius);
    }
}
