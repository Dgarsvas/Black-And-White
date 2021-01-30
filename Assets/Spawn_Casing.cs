using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Casing : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Casing;
    public Transform SpawnPoint;
    Quaternion rot = new Quaternion();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 spawn = new Vector3(SpawnPoint.position.x, SpawnPoint.position.y, SpawnPoint.position.z);
            Instantiate(Casing,spawn, rot);
            YeetOut();
        }
    }
    void YeetOut()
    {
        Vector3 Directions = new Vector3(Casing.transform.position.x+0.1f, Casing.transform.position.y+ 0.1f, Casing.transform.position.z+ 0.1f);
        Casing.GetComponent<Rigidbody>().AddExplosionForce(20, Directions, 1);
    }
}
