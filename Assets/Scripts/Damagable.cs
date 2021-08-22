using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] float hp;

    float maxHp;

    public virtual void Start()
    {
        maxHp = hp;
        CheckIfShouldKill();
    }

    void CheckIfShouldKill()
    {
        Damage(0);
    }

    public void Damage(float amount)
    {
        hp -= amount;

        if (hp <= 0) {
            hp = 0;
            Kill();
        }
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }
}
