using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public float health;
    public Weapon equipedWeapon;

    public virtual void TakeDamage(float damage, Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage)
    {
        TakeDamage(damage, Vector3.zero);
    }

    public virtual void Despawn()
    {
        Destroy(gameObject);
    }

    public void AttackEnemy(Transform enemy)
    {
        Vector3 direction = (transform.position - enemy.position).normalized;
        transform.rotation = AIUtils.RotateY(direction);
        equipedWeapon.Shoot(new Vector3());
    }
}
