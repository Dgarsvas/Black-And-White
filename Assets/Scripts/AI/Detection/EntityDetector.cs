using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDetector : MonoBehaviour
{
    public Detector soundDetector, visionDetector, peripheralDetector;

    public bool canReactToNewDetections;
    public bool detected;

    public Vector3 entityPos;
    public Transform entity;

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

    private void PeripheralDetected(Collider coll)
    {
        Debug.Log("PeripheralDetected");
        if (!canReactToNewDetections)
            return;

        if (coll.CompareTag("Player"))
        {
            detected = true;
            entityPos = coll.transform.position;
        }
    }

    private void VisionDetected(Collider coll)
    {
        Debug.Log("VisionDetected");
        if (!canReactToNewDetections)
            return;

        if (coll.CompareTag("Player"))
        {
            entity = coll.transform;
        }
    }

    private void SoundDetected(Collider coll)
    {
        Debug.Log("SoundDetected");
        if (!canReactToNewDetections)
            return;

        if (coll.CompareTag("Player"))
        {
            detected = true;
            entityPos = coll.transform.position;
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
