using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game4automation;

public class Robot_Welding_Spark : MonoBehaviour
{
    public PLCOutputBool Welding_Signal;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Welding_Signal.Value)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
