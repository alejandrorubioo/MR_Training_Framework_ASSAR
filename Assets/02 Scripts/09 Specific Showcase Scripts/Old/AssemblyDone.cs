using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using MR_Training_ASSAR;


public class AssemblyDone : MonoBehaviour
{
    [SerializeField] private bool activated=false;
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag==GLV.AssembleDonetag)
        {
            GameObject obj = GameObject.FindGameObjectWithTag(GLV.AssembleDonetag);
            obj.GetComponent<BoxCollider>().enabled = true;
            other.gameObject.GetComponent<SH3_DisableGravityWhenGrasped>().enabled = false;
            other.gameObject.layer = 16;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            other.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
            
        

    }

}
