using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon currentWeapon;
    [SerializeField] LayerMask raycastIgnore;

    public bool ads = false;

    public float lookAtLerpSpeed;

    Vector3 targetPos;

    Inputs input;
    bool controlsReady = false;

    [HideInInspector] public Camera mainCam;
    [HideInInspector] public float baseFov;

    void Start()
    {
        mainCam = Camera.main;
        baseFov = mainCam.fieldOfView;

        input = new Inputs();

        input.Weapon.ADS.performed += ctx => ads = true;
        input.Weapon.ADS.canceled += ctx => ads = false;

        input.Weapon.Fire.performed += ctx => currentWeapon.Fire();

        controlsReady = true;
        SetControlsActive(true);
    }

    void Update()
    {
        if (ads) {
            currentWeapon.transform.localRotation = Quaternion.Lerp(currentWeapon.transform.localRotation, Quaternion.identity, currentWeapon.weaponInfo.adsLerpSpeed * Time.deltaTime);

            return;
        }

        FindTargatPos();

        Vector3 direction = targetPos - currentWeapon.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        currentWeapon.transform.rotation = Quaternion.Lerp(currentWeapon.transform.rotation, toRotation, lookAtLerpSpeed * Time.deltaTime);
    }

    void FindTargatPos()
    {
        targetPos = transform.forward * 1000;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity, ~raycastIgnore))
        {
            targetPos = hit.point;
        }
    }

    void SetControlsActive(bool active)
    {
        if (!controlsReady) { return; }
        if (active) { input.Enable(); } else { input.Disable(); }
    }

    private void OnEnable()
    {
        SetControlsActive(true);
    }

    private void OnDisable()
    {
        SetControlsActive(false);
    }
}
