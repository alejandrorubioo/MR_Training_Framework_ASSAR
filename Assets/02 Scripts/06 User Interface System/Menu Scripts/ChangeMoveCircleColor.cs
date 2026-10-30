using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class ChangeMoveCircleColor : MonoBehaviour
{

    private Material OffMaterial;
    public Material OnMaterial;

    // Start is called before the first frame update
    void Start()
    {
        OffMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent.transform.parent.GetComponent<InteractionBehaviour>().isGrasped)
        {
            gameObject.GetComponent<MeshRenderer>().material = OnMaterial;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = OffMaterial;
        }
    }
}
