using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ProcessSystem;
public class OnCollisionStopWaiting : MonoBehaviour
{

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.GetComponent<WaitBehaviour>().enabled)
        {
            gameObject.GetComponent<WaitBehaviour>().StopWaiting();
        }
        
    }
}
