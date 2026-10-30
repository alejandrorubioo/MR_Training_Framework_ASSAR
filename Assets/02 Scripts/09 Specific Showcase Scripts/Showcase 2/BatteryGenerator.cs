using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR;


public class BatteryGenerator : MonoBehaviour
{
    public GameObject Battery;
    private GameObject newBattery;

    public void Spawn()
    {
        newBattery = (GameObject)Instantiate(Battery, this.gameObject.transform);
        newBattery.transform.position = this.gameObject.transform.position;
        newBattery.transform.rotation = this.gameObject.transform.rotation;
        newBattery.gameObject.name = "Battery";
        newBattery.gameObject.tag = GLV.Batterytag;
    }
}

