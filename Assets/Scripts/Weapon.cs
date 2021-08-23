using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector] public bool ads = false;

    public WeaponInfo weaponInfo;
    WeaponController weaponController;

    void Start()
    {
        weaponController = GetComponentInParent<WeaponController>();
    }

    void Update()
    {
        ads = weaponController.ads;
        Ads(ads);
    }

    void Ads(bool isAdsing)
    {
        Vector3 targetPos = weaponInfo.normalPos;
        float targetFov = weaponController.baseFov;

        if (isAdsing)
        {
            targetPos = weaponInfo.adsingPos;
            targetFov = weaponInfo.adsFov;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, weaponInfo.adsLerpSpeed * Time.deltaTime);
        weaponController.mainCam.fieldOfView = Mathf.Lerp(weaponController.mainCam.fieldOfView, targetFov, weaponInfo.adsLerpSpeed * Time.deltaTime);
    }

    public virtual void Fire()
    {

    }
}
