using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC3_ButtonLightPLC : MonoBehaviour
{
    public  Material materialLight;
    public game4automation.PLCOutputBool outputSignal;

    private Material materialOff;
    // Start is called before the first frame update
    void Start()
    {
        materialOff = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (outputSignal.Value)
        {
            GetComponent<MeshRenderer>().material = materialLight;
        }
        else
        {
            GetComponent<MeshRenderer>().material = materialOff;
        }
    }
}
