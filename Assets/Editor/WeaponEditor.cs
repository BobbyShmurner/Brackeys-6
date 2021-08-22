using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

[CustomEditor(typeof(Weapon), true)]
public class WeaponEditor : Editor
{
    bool invertToggle = false;
    bool adsToggle = false;

    public override void OnInspectorGUI()
    {
        Weapon targetWeapon = (Weapon)target;
        invertToggle = Math.Sign(targetWeapon.transform.localScale.z) == -1;

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.Toggle("Invert Weapon Direction", invertToggle);

        if (EditorGUI.EndChangeCheck())
        {
            targetWeapon.transform.localScale = new Vector3(
                targetWeapon.transform.localScale.x,
                targetWeapon.transform.localScale.y,
               -targetWeapon.transform.localScale.z
            );
        }

        //ADS

        if (!Application.isPlaying)
        {
            EditorGUI.BeginChangeCheck();

            targetWeapon.ADSing = EditorGUILayout.Toggle("ADS", targetWeapon.ADSing);

            if (EditorGUI.EndChangeCheck())
            {
                if (targetWeapon.ADSing)
                {
                    targetWeapon.transform.localPosition = targetWeapon.ADSingPos;
                }
                else
                {
                    targetWeapon.transform.localPosition = targetWeapon.normalPos;
                }
            }

            if (targetWeapon.ADSing)
            {
                targetWeapon.normalPos = EditorGUILayout.Vector3Field("Normal Pos", targetWeapon.normalPos);

                targetWeapon.ADSingPos = EditorGUILayout.Vector3Field("ADSing Pos", targetWeapon.transform.localPosition);
                targetWeapon.transform.localPosition = targetWeapon.ADSingPos;
            }
            else
            {
                targetWeapon.normalPos = EditorGUILayout.Vector3Field("Normal Pos", targetWeapon.transform.localPosition);
                targetWeapon.transform.localPosition = targetWeapon.normalPos;

                targetWeapon.ADSingPos = EditorGUILayout.Vector3Field("ADSing Pos", targetWeapon.ADSingPos);
            }
        } else {
            EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.Toggle("ADS", targetWeapon.ADSing);

            EditorGUILayout.Vector3Field("Normal Pos", targetWeapon.normalPos);
            EditorGUILayout.Vector3Field("ADSing Pos", targetWeapon.ADSingPos);

            EditorGUI.EndDisabledGroup();
        }

    }
}