using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct WeaponInfo
{
    public GameObject bulletImpact;

    public bool automatic;
    public float fireRate;

    public float magCount;
    public float reloadTime;

    public float damage;
    public float force;

    public float adsLerpSpeed;
    public float adsFov;

    public Vector3 normalPos;
    public Vector3 adsingPos;
}
