using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject cameraObject;
    CameraFollow cameraFollow;
    Camera camera;

    public Weapon currentWeapon;

    NavMeshAgent agent;
    float speed = 0.5f; //speed of movement
    public float health = 100;
    public bool hiding = false;
    //Quaternion hidingRotation;
    Vector3 hidingDirection;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cameraFollow = cameraObject.GetComponent<CameraFollow>();
        camera = GetComponent<Camera>();
    }
    void Update()
    {
        float x = Input.GetAxisRaw("Vertical");
        float z = Input.GetAxisRaw("Horizontal");

        //Move
        Vector3 moveDestination = transform.position + new Vector3(- x + z, 0, x + z) * speed;
        agent.destination = moveDestination;

        //Rotate
        RaycastHit hit = cameraFollow.floorRaycast;
        Vector3 direction = (transform.position - hit.point).normalized;
        RotateY(direction);

        //crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (hiding) hiding = false;
            else Hide();
        }
        if (hiding) RotateY(hidingDirection);

        //Shoot
        if (Input.GetMouseButton(0)) currentWeapon.Shoot(cameraFollow.floorRaycast.point);

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
        transform.rotation = Quaternion.Euler(0f, 180 - rot_z, 0f);
    }
}
