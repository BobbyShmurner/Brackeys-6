using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

[CustomEditor(typeof(Weapon), true)]
public class WeaponEditor : Editor
{
    bool invertToggle = false;
    bool showGunPositions = false;

    bool lockGunPositions = true;

    float baseFov;

    public override void OnInspectorGUI()
    {
        Weapon targetWeapon = (Weapon)target;
        WeaponController targetWeaponController = targetWeapon.GetComponentInParent<WeaponController>();

        targetWeapon.weaponInfo.bulletImpact = EditorGUILayout.ObjectField("Bullet Impact", targetWeapon.weaponInfo.bulletImpact, typeof(GameObject), false) as GameObject;
        targetWeapon.weaponInfo.damage = EditorGUILayout.FloatField("Damage", targetWeapon.weaponInfo.damage);
        targetWeapon.weaponInfo.force = EditorGUILayout.FloatField("Impact Force", targetWeapon.weaponInfo.force);
        targetWeapon.weaponInfo.recoil = EditorGUILayout.FloatField("Recoil", targetWeapon.weaponInfo.recoil);
        targetWeapon.weaponInfo.automatic = EditorGUILayout.Toggle("Is Automatic", targetWeapon.weaponInfo.automatic);

        EditorGUI.BeginDisabledGroup(!targetWeapon.weaponInfo.automatic);
        if (targetWeapon.weaponInfo.automatic) targetWeapon.weaponInfo.fireRate = EditorGUILayout.FloatField("Fire Rate", targetWeapon.weaponInfo.fireRate);
        EditorGUI.EndDisabledGroup();

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
                    Camera.main.fieldOfView = targetWeapon.weaponInfo.adsFov;
                }
                else
                {
                    targetWeapon.transform.localPosition = targetWeapon.weaponInfo.normalPos;

                    Camera.main.fieldOfView = baseFov;
                }
            }

            if (targetWeapon.ads) Camera.main.fieldOfView = targetWeapon.weaponInfo.adsFov;

            EditorGUI.BeginDisabledGroup(!targetWeapon.ads);
            targetWeapon.weaponInfo.adsFov = EditorGUILayout.Slider("Ads Fov", targetWeapon.weaponInfo.adsFov, 0.001f, 179f);
            EditorGUI.EndDisabledGroup();

            BeginGunPositionFoldout();

            if (targetWeapon.ads)
            {
                if (showGunPositions)
                {
                    EditorGUI.BeginDisabledGroup(lockGunPositions);

                    Vector3 targetPos = targetWeapon.transform.localPosition;
                    if (lockGunPositions) targetPos = targetWeapon.weaponInfo.adsingPos;

                    targetWeapon.weaponInfo.normalPos = EditorGUILayout.Vector3Field("Normal Pos", targetWeapon.weaponInfo.normalPos);
                    targetWeapon.weaponInfo.adsingPos = EditorGUILayout.Vector3Field("Adsing Pos", targetPos);

                    EditorGUI.EndDisabledGroup();
                } else
                {
                    if (lockGunPositions) return;

                    targetWeapon.weaponInfo.adsingPos = targetWeapon.transform.localPosition;
                }

                if (!lockGunPositions) targetWeapon.transform.localPosition = targetWeapon.weaponInfo.adsingPos;
            }
            else
            {
                if (showGunPositions)
                {
                    EditorGUI.BeginDisabledGroup(lockGunPositions);

                    Vector3 targetPos = targetWeapon.transform.localPosition;
                    if (lockGunPositions) targetPos = targetWeapon.weaponInfo.normalPos;

                    targetWeapon.weaponInfo.normalPos = EditorGUILayout.Vector3Field("Normal Pos", targetPos);
                    targetWeapon.weaponInfo.adsingPos = EditorGUILayout.Vector3Field("Adsing Pos", targetWeapon.weaponInfo.adsingPos);

                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    if (lockGunPositions) return;

                    targetWeapon.weaponInfo.normalPos = targetWeapon.transform.localPosition;
                }

                if (!lockGunPositions) targetWeapon.transform.localPosition = targetWeapon.weaponInfo.normalPos;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        } else {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Toggle("Ads", targetWeapon.ads);
            EditorGUI.EndDisabledGroup();

            targetWeapon.weaponInfo.adsFov = EditorGUILayout.Slider("Ads Fov", targetWeapon.weaponInfo.adsFov, 0.001f, 179f);

            BeginGunPositionFoldout();

            if (showGunPositions)
            {
                EditorGUI.BeginDisabledGroup(lockGunPositions);

                targetWeapon.weaponInfo.normalPos = EditorGUILayout.Vector3Field("Normal Pos", targetWeapon.weaponInfo.normalPos);
                targetWeapon.weaponInfo.adsingPos = EditorGUILayout.Vector3Field("Adsing Pos", targetWeapon.weaponInfo.adsingPos);

                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        //DrawDefaultInspector();
    }

    void BeginGunPositionFoldout()
    {
        showGunPositions = EditorGUILayout.BeginFoldoutHeaderGroup(showGunPositions, "Gun Positioning");

        if (showGunPositions)
        {
            lockGunPositions = EditorGUILayout.Toggle("Lock Gun Positions", lockGunPositions);
        }
    }
}