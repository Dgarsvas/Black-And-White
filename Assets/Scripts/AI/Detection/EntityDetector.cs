using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDetector : MonoBehaviour
{
    public Detector soundDetector, visionDetector, peripheralDetector;

    public bool canReactToNewDetections;
    public bool detected;
    public bool hasSight;
    public float visionDistance;
    public float footstepHearingDistance;

    public Vector3 entityPos;
    public Transform entity;
    public float timelastSeen = 0;

    private void Awake()
    {
        detected = false;
        if (soundDetector != null)
        {
            soundDetector.OnDetected += SoundDetected;
        }

        if (visionDetector != null)
        {
            visionDetector.OnDetected += VisionDetected;
        }

        if (peripheralDetector != null)
        {
            peripheralDetector.OnDetected += PeripheralDetected;
        }
    }

    public bool DirectSight(Vector3 position) {
        Vector3 direction = (position - transform.position).normalized;
        Ray ray = new Ray(transform.position + direction*0.5f, direction);
        RaycastHit hit;
        
        bool ret = Physics.Raycast(ray, out hit, visionDistance, ~LayerMask.GetMask("Detector"));
        if (!ret) return false;
        Debug.DrawRay(transform.position, hit.point - transform.position);
        return hit.collider.CompareTag("Player");
    }

    private void PeripheralDetected(Collider coll)
    {
        Debug.Log("PeripheralDetected");
        if (!canReactToNewDetections)
            return;

        if (coll.CompareTag("Player"))
        {
            detected = true;
            entityPos = coll.transform.position;
            timelastSeen = Time.time;
        }
    }

    private void VisionDetected(Collider coll)
    {
        Debug.Log("VisionDetected");
        if (!canReactToNewDetections)
            return;

        if (coll.CompareTag("Player") && DirectSight(coll.transform.position))
        {
            //detected = true;
            timelastSeen = Time.time;
            hasSight = true;
            entity = coll.transform;
            entityPos = coll.transform.position;
        }
    }

    private void SoundDetected(Collider coll)
    {
        Debug.Log("SoundDetected");
        if (!canReactToNewDetections)
            return;

        float distance = Vector3.Distance(coll.transform.position, transform.position);
        if (coll.CompareTag("Player")) {
            Player player = coll.GetComponent<Player>();
            if ((player.moving && distance < footstepHearingDistance && !player.sneaking) || (player.recentlyShot > 0))
            {
                detected = true;
                entityPos = coll.transform.position;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (detected)
        {
            Gizmos.DrawSphere(entityPos, 1);
        }
    }
}
