using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR;
using Leap.Unity.Interaction;

public class AssembleObj : MonoBehaviour
{
    public void Assembleobj()
    {
        GameObject obj=GameObject.FindGameObjectWithTag(GLV.DinamicParenttag);
        obj.GetComponent<InteractionBehaviour>().enabled = true;
        obj.GetComponent<Collider>().enabled = true;
    }
}
