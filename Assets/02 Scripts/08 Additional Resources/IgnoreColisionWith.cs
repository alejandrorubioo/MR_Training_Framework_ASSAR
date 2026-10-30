using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class IgnoreColisionWith : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.HasComponent<InteractionBehaviour>() && collision.gameObject.tag!=GLV.TopTag)
            {
                Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            }
        }
    }
}