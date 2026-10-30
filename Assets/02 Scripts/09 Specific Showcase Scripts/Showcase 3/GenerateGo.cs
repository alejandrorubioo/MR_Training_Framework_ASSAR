using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game4automation;

public class GenerateGo : MonoBehaviour
{
    public Sensor sensor;
    public PLCOutputBool generateBox;
    void Update()
    {
        if (sensor.Occupied)
        {
            generateBox.Value = true;
        }
        else
        {
            generateBox.Value = false;
        }
    }
}
