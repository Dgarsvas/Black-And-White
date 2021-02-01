using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    private static float speed = 500f;
	private static float selfDestructTime = 5f;

	private bool isSetup;
	private float timer;

    internal void Setup(float damage)
    {
        this.damage = damage;
        GetComponent<Rigidbody>().AddForce(-transform.forward * speed);
		isSetup = true;
    }

	private void Update()
	{
		if (isSetup)
		{
			timer += Time.deltaTime;
			if (timer >= selfDestructTime)
			{
				Destroy(gameObject);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.CompareTag("Enemy"))
		{
			BaseEntity enemy = collision.gameObject.GetComponent<BaseEntity>();
			if (enemy.health - damage > 0)
			{
				enemy.TakeDamage(damage);
			}
			else
			{
				enemy.TakeDamage(damage);
			}
		}
		else if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();
			player.TakeDamage(damage);
		}

		Destroy(gameObject);
	}
}