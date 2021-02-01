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

	public Transform spawnPoint;
	public GameObject bulletPrefab;

	float timeSinceShot = 0;
	float reloadingSinceTime = 0;
	bool reloading = false;

	public bool equiped = false; //is this weapon on the ground to be picked up or already equiped

	AudioSource audioSource;
	LineRenderer shotRay;

	void Start()
	{
		//shotRay = GetComponent<LineRenderer>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		timeSinceShot += Time.deltaTime;
		if (reloading) reloadingSinceTime += Time.deltaTime;
		if (reloading && reloadingSinceTime > reloadTime) FinishReload();
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
			Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<Bullet>().Setup(damage);
		}

		if (mag == 0)
		{
			Reload();
		}
			
		//Vector3 direction = (point - transform.position);
		//direction = new Vector3(direction.x, 0, direction.z);

		/*
		Vector3 direction = -transform.forward;
		//Debug.Log("Weapon direction: " + direction.ToString());
		//apply bullet spread
		direction += transform.up * (Random.value*2 - 1) * bulletSpread + transform.right * (Random.value * 2 - 1) * bulletSpread;
		direction = direction.normalized;

		Ray ray = new Ray(transform.position, direction);
		LayerMask mask = ~LayerMask.GetMask("Detector");
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, mask))
		{
			if (hit.collider.gameObject.CompareTag("Enemy"))
			{
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
				Player player = hit.collider.gameObject.GetComponent<Player>();
				player.TakeDamage(damage);
			}

		}
		else hit.distance = 100;
		Debug.DrawRay(transform.position, direction * 10f);
		//Vector3[] points = { transform.position, hit.point };
		Vector3[] points = { transform.position, transform.position + direction * (hit.distance + 1) };
		//shotRay.SetPositions(points);

	}
	*/

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
	private void Equip(Player player) {
		GetComponent<BoxCollider>().enabled = false;
		if (player.currentWeapon != null)
		{
			player.currentWeapon.Drop();
		}
		player.currentWeapon = this;
		equiped = true;
		transform.position = player.weaponSlot.transform.position;
		transform.parent = player.weaponSlot.transform;
		//transform.position = Vector3.zero;
		transform.position = player.weaponSlot.transform.position;
		//transform.rotation = Quaternion.Euler(0, -90, 0);
	}
	public void Drop() {
		Ray ray = new Ray(transform.position, -transform.up);
		LayerMask mask = ~LayerMask.GetMask("Detector");
		RaycastHit hit;
		Physics.Raycast(ray, out hit, 100, mask);
		equiped = false;
		transform.parent = null;
		transform.position = hit.point - transform.forward*2 + transform.up/2;
		GetComponent<BoxCollider>().enabled = true;
	}
	private void OnCollisionStay(Collision collision)
	{
		//Debug.Log("Weapon pickup");
		if (!equiped && collision.collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.Q)) {
			Equip(collision.collider.GetComponent<Player>());
		}
	}
}
