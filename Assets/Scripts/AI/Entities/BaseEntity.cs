using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public float health;

    public virtual void TakeDamage(float damage, Vector3 direction)
    {
        throw new NotImplementedException();
    }

    public void TakeDamage(float damage)
    {
        TakeDamage(damage, Vector3.zero);
    }

    public virtual void Despawn()
    {

    }
}
