using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Damagable), true)]
public class DamagableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Damagable targetObject = (Damagable)target;
        
        if (GUILayout.Button("Kill Object"))
        {
            targetObject.Kill();
        }

        DrawDefaultInspector();
    }
}