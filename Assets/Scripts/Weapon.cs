using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector] public bool ADSing = false;

    [HideInInspector] public Vector3 normalPos;
    [HideInInspector] public Vector3 ADSingPos;

    void Update()
    {
        ADSing = GetComponentInParent<WeaponController>().ADS;
        float lerpSpeed = GetComponentInParent<WeaponController>().lerpSpeed;

        Vector3 targetPos = normalPos;
        if (ADSing) targetPos = ADSingPos;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, lerpSpeed * Time.deltaTime);
    }
}
