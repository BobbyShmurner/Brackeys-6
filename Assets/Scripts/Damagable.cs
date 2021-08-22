using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public float hp;

    [HideInInspector] float maxHp;

    void Start()
    {
        maxHp = hp;
    }

    public void Damage(float amount)
    {
        hp -= amount;

        if (hp <= 0) {
            hp = 0;
            Kill();
        }
    }

    protected virtual void Kill()
    {
        Destroy(gameObject);
    }
}
