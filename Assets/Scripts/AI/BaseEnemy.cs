using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public float health;

    public virtual void TakeDamage(float damage)
    {
        throw new NotImplementedException();
    }
}
