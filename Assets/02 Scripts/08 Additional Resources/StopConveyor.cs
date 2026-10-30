using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game4automation;

public class StopConveyor : MonoBehaviour
{
    public Sensor sensor;
    public GameObject conveyor1;

    private void Update()
    {
        if (sensor.Occupied)
        {
            conveyor1.GetComponent<Drive>().JogForward = false;
        }
        else
        {
            conveyor1.GetComponent<Drive>().JogForward = true;
        }
    }
}