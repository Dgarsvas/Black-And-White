using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public GameObject cameraObject;
    CameraFollow cameraFollow;

    public Weapon currentWeapon;
    public Transform weaponSlot;

    Animator animator;

    NavMeshAgent agent;
    public float speed = 1f; //speed of movement
    public float health = 100;
    public bool hiding = false;
    public bool moving = false;
    public bool sneaking = false;
    public int recentlyShot = 0;
    //Quaternion hidingRotation;
    Vector3 hidingDirection;

    private bool canEscape = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraFollow.Setup(transform);

        GameManager.instance.OnGirlFound += StartEscape;
    }

    private void StartEscape(Transform player)
    {
        canEscape = true;
    }

    void Update()
    {
        
        float x = Input.GetAxisRaw("Vertical");
        float z = Input.GetAxisRaw("Horizontal");
        moving = (x != 0 || z != 0);

        //Move
        float currentSpeed = speed;
        if (sneaking) currentSpeed *= 0.1f;
        Vector3 moveDestination = transform.position + new Vector3(-x + z, 0, x + z) * currentSpeed;
        agent.destination = moveDestination;

        //Rotate
        RaycastHit hit = cameraFollow.floorRaycast;
        Vector3 direction = (transform.position - hit.point).normalized;
        RotateY(direction);

        //hide
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (hiding) hiding = false;
            else Hide();
        }
        if (hiding) RotateY(hidingDirection);

        if (Input.GetKey(KeyCode.LeftShift) && !sneaking) sneaking = true;
        else if (!Input.GetKey(KeyCode.LeftShift) && sneaking) sneaking = false;

        //Shoot
        if (Input.GetMouseButton(0)) {
            currentWeapon.Shoot(cameraFollow.floorRaycast.point);
            recentlyShot = 50;
        }
        recentlyShot--;
        //reload
        if (Input.GetKeyDown(KeyCode.R)) currentWeapon.Reload();


        animator.SetBool("Walking", moving);
        animator.SetBool("Shooting", recentlyShot>-50);
        Vector3 walkingDirection = (transform.position - agent.destination).normalized;
        float angle = Vector3.Angle(walkingDirection, direction);
        animator.SetBool("Sideways", angle > 70 && angle < 150);
        if (canEscape)
        {
            if (AIUtils.ApproximatePositionReached(transform.position, GameManager.instance.stairsPos))
            {
                GameManager.instance.PlayerEscaped();
            }
        }
    }
    /// <summary>
    /// checks if there is a wall and sets player rotation to wall normal
    /// currently hiding is still active after leaving the wall
    /// better to be replaced with collision with a field around a wall
    /// </summary>
    void Hide()
    {
        Ray ray = new Ray(transform.position + transform.right/2, transform.right);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2))
        {
            /*
            Vector3 rot = transform.rotation.eulerAngles;
            rot = new Vector3(hit.normal);
            hidingRotation = transform.rotation;
            hidingRotation = Quaternion.Euler(hidingRotation.eulerAngles.x, hit.normal., hidingRotation.eulerAngles.z);
            */
            hidingDirection = -hit.normal;
            hiding = true;
        }
    }
    void RotateY(Vector3 direction) {
        float rot_z = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 270 - rot_z, 0f);
    }
    public void TakeDamage(float damage)
    {
        health = Mathf.Max(0, health - damage);
        if (health == 0)
        {
            GameManager.instance.PlayerDied();
        }
    }
}
