using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public string name = "";
	public int ammo = 0;
	public int ammoCapacity = 0;
	public int mag = 0;
	public int magCapacity = 0;
	public float shootInterval = 0;
	public float reloadTime = 0;

	float timeSinceShot = 0;
	float reloadingSinceTime = 0;
	bool reloading = false;

	AudioSource audioSource;

	void Start()
    {
		audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
		timeSinceShot += Time.deltaTime;
		if(reloading) reloadingSinceTime += Time.deltaTime;
		if (reloading && reloadingSinceTime > reloadTime) FinishReload();

		if (Input.GetKeyDown(KeyCode.R)) Reload();
    }

	public void Shoot(Vector3 point)
	{
		if (mag > 0 && timeSinceShot > shootInterval && !reloading)
		{
			mag--;
			timeSinceShot = 0;
			Debug.Log("shot");
			audioSource.Play();

			Vector3 direction = (point - transform.position);
			direction = new Vector3(direction.x, 0, direction.z).normalized;
			Ray ray = new Ray(transform.position + direction, direction);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag.Equals("Enemy"))
			{
				Debug.DrawRay(transform.position, direction * 10f, Color.red);
			}
			else Debug.DrawRay(transform.position, direction * 10f);
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
