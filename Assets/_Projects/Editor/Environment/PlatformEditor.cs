using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlatformMovementScript))]
public class PlatformEditor : Editor
{
    //float position = 0;
    public override void OnInspectorGUI()
    {
     
        base.OnInspectorGUI();

        PlatformMovementScript platform = (PlatformMovementScript)target;

        /*
        EditorGUILayout.LabelField("Preview");
        position = EditorGUILayout.Slider(position,0,20);
        platform.NodeBasedMovement(position);
        */
        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Generate Node"))
        {
            platform.GenerateNode();
        }

        if(GUILayout.Button("Remove Last Node"))
        {
            platform.DeleteNode();
        }

        GUILayout.EndHorizontal();

        if(GUILayout.Button("Toggle Gizmo"))
        {
            platform.gizmoView = !platform.gizmoView;
        }
    
       EditorGUILayout.LabelField("WARNING!! : UNPACK BEFORE USING");
       
    }
}
