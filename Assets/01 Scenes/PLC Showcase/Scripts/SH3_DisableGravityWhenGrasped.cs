using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class SH3_DisableGravityWhenGrasped : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForGrasped());
        //gameObject.layer = 7;
    }

    IEnumerator WaitForGrasped()
    {
        yield return new WaitUntil(() => GetComponent<InteractionBehaviour>().isGrasped && GetComponent<Rigidbody>().useGravity);
        GetComponent<Rigidbody>().useGravity = false;
        yield break;
    }
}
