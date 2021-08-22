using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon currentWeapon;
    [SerializeField] LayerMask raycastIgnore;

    public bool ADS = false;
    public float lerpSpeed;

    Vector3 targetPos;

    Inputs input;
    bool controlsReady = false;

    void Start()
    {
        input = new Inputs();

        input.Weapon.ADS.performed += ctx => ADS = true;
        input.Weapon.ADS.canceled += ctx => ADS = false;

        controlsReady = true;
        SetControlsActive(true);
    }

    void Update()
    {
        if (ADS) {
            currentWeapon.transform.localRotation = Quaternion.Lerp(currentWeapon.transform.localRotation, Quaternion.identity, lerpSpeed * Time.deltaTime);
            return;
        }

        FindTargatPos();

        Vector3 direction = targetPos - currentWeapon.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        currentWeapon.transform.rotation = Quaternion.Lerp(currentWeapon.transform.rotation, toRotation, lerpSpeed * Time.deltaTime);
    }

    void FindTargatPos()
    {
        targetPos = transform.forward * 1000;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ~raycastIgnore))
        {
            targetPos = hit.point;
            Debug.Log(hit.collider.name);
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
