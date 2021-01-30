using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCasing : MonoBehaviour
{
    public float KillTime=3.0f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, KillTime);
    }
}
