using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : Damagable
{
    [SerializeField] List<GameObject> destroyedPrefabs = new List<GameObject>();

    public override void Start()
    {
        base.Start();
    }

    public override void Kill()
    {
        Collider col = GetComponent<Collider>();
        if (col) { col.enabled = false; }

        GameObject destroyedObject = Instantiate(destroyedPrefabs[Random.Range(0, destroyedPrefabs.Count)]);

        destroyedObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
        destroyedObject.transform.localScale = transform.localScale;

        destroyedObject.GetComponent<AccurateMass>().CalculateMass();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb) {
            foreach(Rigidbody rigidbody in destroyedObject.GetComponentsInChildren<Rigidbody>())
            {
                rigidbody.velocity = rb.velocity;
            }
        }

        foreach (Transform childTrans in destroyedObject.GetComponentsInChildren<Transform>())
        {
            childTrans.parent = null;
        }

        base.Kill();
    }
}
