using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector] public bool ads = false;
    [HideInInspector] public bool shouldFire = false;

    float timeTillNextFire = 0;

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

        if (shouldFire) Fire();
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
        if (timeTillNextFire > 0) return;

        if (!weaponInfo.automatic) shouldFire = false;

        if (weaponInfo.automatic) timeTillNextFire = weaponInfo.fireRate;

        if (Physics.Raycast(weaponController.mainCam.transform.position, weaponController.mainCam.transform.forward, out RaycastHit hit, Mathf.Infinity, ~weaponController.raycastIgnore))
        {
            if (hit.collider.GetComponent<Damagable>()) hit.collider.GetComponent<Damagable>().Damage(weaponInfo.damage);
            if (hit.collider.GetComponent<Rigidbody>()) hit.collider.GetComponent<Rigidbody>().AddForce((hit.point - weaponController.mainCam.transform.position).normalized * weaponInfo.force);

            Instantiate(weaponInfo.bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    void OnEnable()
    {
        Vector3 targetPos = weaponInfo.normalPos;
        if (ads) targetPos = weaponInfo.adsingPos;

        transform.localPosition = targetPos - new Vector3(0, 1, 0);
    }

    private void LateUpdate()
    {
        timeTillNextFire -= Time.deltaTime;
    }
}
