using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DungeonGen2))]
public class LevelScriptEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DungeonGen2 myTarget = (DungeonGen2)target;
        if(GUILayout.Button("Build Object"))
        {
            myTarget.CheckDivisionWithLowestNumber();
        }

        if(GUILayout.Button("Randomly Divide"))
        {
            myTarget.RandomlyDivide();
        }
    }
}