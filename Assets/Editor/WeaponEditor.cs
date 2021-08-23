using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

[CustomEditor(typeof(Weapon), true)]
public class WeaponEditor : Editor
{
    bool invertToggle = false;
    float baseFov;

    public override void OnInspectorGUI()
    {
        Weapon targetWeapon = (Weapon)target;
        WeaponController targetWeaponController = targetWeapon.GetComponentInParent<WeaponController>();

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

        if (!Application.isPlaying)
        {
            if (targetWeaponController) targetWeapon.gameObject.SetActive(targetWeaponController.currentWeapon == targetWeapon); // Enable if its the selected Weapon, Disable if its not

            EditorGUI.BeginChangeCheck();

            targetWeapon.ads = EditorGUILayout.Toggle("Ads", targetWeapon.ads);

            if (EditorGUI.EndChangeCheck())
            {
                if (targetWeapon.ads)
                {
                    targetWeapon.transform.localPosition = targetWeapon.weaponInfo.adsingPos;

                    baseFov = Camera.main.fieldOfView;
                }
                else
                {
                    targetWeapon.transform.localPosition = targetWeapon.weaponInfo.normalPos;

                    Camera.main.fieldOfView = baseFov;
                }
            }

            DrawAdsFovSlider(targetWeapon);

            if (targetWeapon.ads)
            {
                targetWeapon.weaponInfo.normalPos = EditorGUILayout.Vector3Field("Normal Pos", targetWeapon.weaponInfo.normalPos);

                targetWeapon.weaponInfo.adsingPos = EditorGUILayout.Vector3Field("Adsing Pos", targetWeapon.transform.localPosition);
                targetWeapon.transform.localPosition = targetWeapon.weaponInfo.adsingPos;

                Camera.main.fieldOfView = targetWeapon.weaponInfo.adsFov;
            }
            else
            {
                targetWeapon.weaponInfo.normalPos = EditorGUILayout.Vector3Field("Normal Pos", targetWeapon.transform.localPosition);
                targetWeapon.transform.localPosition = targetWeapon.weaponInfo.normalPos;

                targetWeapon.weaponInfo.adsingPos = EditorGUILayout.Vector3Field("Adsing Pos", targetWeapon.weaponInfo.adsingPos);
            }
        } else {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Toggle("Ads", targetWeapon.ads);
            EditorGUI.EndDisabledGroup();

            DrawAdsFovSlider(targetWeapon);

            targetWeapon.weaponInfo.normalPos = EditorGUILayout.Vector3Field("Normal Pos", targetWeapon.weaponInfo.normalPos);
            targetWeapon.weaponInfo.adsingPos = EditorGUILayout.Vector3Field("Adsing Pos", targetWeapon.weaponInfo.adsingPos);
        }

        //DrawDefaultInspector();
    }

    void DrawAdsFovSlider(Weapon targetWeapon)
    {
        EditorGUI.BeginDisabledGroup(!targetWeapon.ads);
        targetWeapon.weaponInfo.adsFov = EditorGUILayout.Slider("Ads Fov", targetWeapon.weaponInfo.adsFov, 0.001f, 179f);
        EditorGUI.EndDisabledGroup();
    }
}