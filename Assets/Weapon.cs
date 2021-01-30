using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public string name = "";
	public float damage = 0;
	public int ammo = 0;
	public int ammoCapacity = 0;
	public int mag = 0;
	public int magCapacity = 0;
	public float shootInterval = 0;
	public float reloadTime = 0;
	public float bulletSpread = 0;

	float timeSinceShot = 0;
	float reloadingSinceTime = 0;
	bool reloading = false;

	AudioSource audioSource;
	LineRenderer shotRay;

	void Start()
	{
		shotRay = GetComponent<LineRenderer>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		timeSinceShot += Time.deltaTime;
		if (reloading) reloadingSinceTime += Time.deltaTime;
		if (reloading && reloadingSinceTime > reloadTime) FinishReload();

		if (Input.GetKeyDown(KeyCode.R)) Reload();
	}

	/// <summary>
	/// ray cast and detect if hit enemy
	/// </summary>
	public void Shoot(Vector3 point)
	{
		if (mag > 0 && timeSinceShot > shootInterval && !reloading)
		{
			mag--;
			timeSinceShot = 0;
			audioSource.Play();

			//Vector3 direction = (point - transform.position);
			//direction = new Vector3(direction.x, 0, direction.z);

			Vector3 direction = transform.right;
			//apply bullet spread
			//direction += transform.up * (Random.value*2 - 1) * bulletSpread + transform.forward * (Random.value * 2 - 1) * bulletSpread;
			//direction = direction.normalized;


			Ray ray = new Ray(transform.position + direction, direction);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.gameObject.CompareTag("Enemy")) 
				{
					Debug.DrawRay(transform.position, direction * 10f, Color.red);
					BaseEntity enemy = hit.collider.gameObject.GetComponent<BaseEntity>();
					if (enemy.health - damage > 0)
					{
						enemy.TakeDamage(damage);
					}
					else
					{
						enemy.TakeDamage(damage, direction);
					}
				}
				else if (hit.collider.gameObject.CompareTag("Player"))
				{

				}
				else Debug.DrawRay(transform.position, direction * 10f);
				
			}
			//Vector3[] points = { transform.position, hit.point };
			Vector3[] points = { transform.position, transform.position + direction * (hit.distance + 1) };
			shotRay.SetPositions(points);

		}
		if (mag == 0) Reload();
	}

	public void Reload() {
		if (ammo > 0 && mag < magCapacity) {
			reloading = true;
			reloadingSinceTime = 0;
		}
	}
	void FinishReload() {
		int add = Mathf.Min(ammo, magCapacity - mag);
		mag += add;
		ammo -= add;
		reloading = false;
	}
}
