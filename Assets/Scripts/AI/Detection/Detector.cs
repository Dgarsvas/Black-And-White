using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public delegate void Detected(Collider coll);
    public event Detected OnDetected;

    private void OnTriggerEnter(Collider other)
    {
        OnDetected?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnDetected?.Invoke(other);
    }
}
