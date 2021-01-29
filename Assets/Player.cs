using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject cameraObject;
    CameraFollow cameraFollow;
    NavMeshAgent agent;
    float speed = 0.5f; //speed of movement
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cameraFollow = cameraObject.GetComponent<CameraFollow>();
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
        float rot_z = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 90 - rot_z, 0f);
    }
}
