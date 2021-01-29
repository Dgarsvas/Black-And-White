using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; //player to follow
    Vector3 originalPosition; //offset for player following
    Camera camera; //reference to camera component
    public RaycastHit floorRaycast;
    public float cameraSpeed = 8f; //speed for following

    void Start()
    {
        originalPosition = transform.position;
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        raycastFloor();
        followPlayer();
    }

    /// <summary>
    /// raycast with floor and sets floorRaycast to hit
    /// </summary>
    void raycastFloor() {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = LayerMask.GetMask("Floor");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            floorRaycast = hit;
        }
    }

    /// <summary>
    /// follows player and updates positions
    /// </summary>
    void followPlayer() {
        Vector3 nextPosition = target.position * 0.7f + floorRaycast.point * 0.3f + originalPosition;
        Vector3 interpolatedPosition = Vector3.Lerp(transform.position, nextPosition, cameraSpeed * Time.deltaTime);
        transform.position = interpolatedPosition;
    }
}
