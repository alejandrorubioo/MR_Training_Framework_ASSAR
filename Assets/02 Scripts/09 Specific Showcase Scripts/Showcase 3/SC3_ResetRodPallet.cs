using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC3_ResetRodPallet : MonoBehaviour
{
    private bool prevValue=false;
    public ModbusTest Modbus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //THIS IS NOT THE BEST SO

    public void DissapearPallets()
    {
        GameObject[] Pallets = GameObject.FindGameObjectsWithTag("Pallet");
        GameObject[] Rod1 = GameObject.FindGameObjectsWithTag("Rod1");
        GameObject[] Rod2 = GameObject.FindGameObjectsWithTag("Rod2");
        GameObject[] Screw = GameObject.FindGameObjectsWithTag("Screw");

        foreach (GameObject obj in Pallets) obj.SetActive(false);
        foreach (GameObject obj in Rod1)    obj.SetActive(false);
        foreach (GameObject obj in Rod2)    obj.SetActive(false);
        foreach (GameObject obj in Screw)   obj.SetActive(false);
    }

    public void ResetObjects()
    {
        GameObject[] Pallets = GameObject.FindGameObjectsWithTag("Pallet");
        GameObject[] Rod1 = GameObject.FindGameObjectsWithTag("Rod1");
        GameObject[] Rod2 = GameObject.FindGameObjectsWithTag("Rod2");
        GameObject[] Screw = GameObject.FindGameObjectsWithTag("Screw");

        /*
        foreach (GameObject obj in Pallets) obj.SetActive(true);
        foreach (GameObject obj in Rod1) obj.SetActive(true);
        foreach (GameObject obj in Rod2) obj.SetActive(true);
        foreach (GameObject obj in Screw) obj.SetActive(true);
        */

        foreach (GameObject obj in Pallets) if (obj.name != "Pallet 1") obj.transform.position = new Vector3(100,0,0);
        foreach (GameObject obj in Rod1)    if (obj.name != "Rod1 Visible") obj.transform.position = new Vector3(100, 0, 0);
        foreach (GameObject obj in Rod2)    if (obj.name != "Rod2 Visible") obj.transform.position = new Vector3(100, 0, 0);
        foreach (GameObject obj in Screw)    if (obj.name != "Screw1 Visible") obj.transform.position = new Vector3(100, 0, 0);

        Modbus.DisconnectClient();
        Modbus.ConnectModBus();

        prevValue = GetComponent<VarjoMarkerManager>().isActiveAndEnabled;
    }
}
