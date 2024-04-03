using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision Entered {collision}");
    }

    public void OnCollisionExit(Collision collision)
    {
        Debug.Log($"Collision Exited {collision}");
    }


    public void OnCollisionStay(Collision collision)
    {
        //Debug.Log($"Collision Stayed {collision}");
    }
    
    
    
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger Entered {other}");
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log($"Trigger Exited {other}");
    }


    public void OnTriggerStay(Collider other)
    {
        //Debug.Log("Trigger Stayed");
    }

}
