using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMenuPos : MonoBehaviour
{
    Vector3 initialPos;
    Quaternion initialRot;
    
    // Start is called before the first frame update
    private void Awake()
    {
        initialPos = gameObject.transform.position; //Transform del menu inicial
        initialRot = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPos()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;
    }
}
