using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLCConnectedScript : MonoBehaviour
{
    public ModbusTest ModbusManager;
    public MeshRenderer squareBorder;
    public TMPro.TextMeshPro text;
    public GameObject buttonBase;

    [Header("On/Off Materials")]
    public Material greenMaterial;
    public Material redMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ModbusManager.connected)
        {
            squareBorder.material = greenMaterial;
            text.text = "PLC CONNECTED";
            buttonBase.SetActive(false);

            //Disable this component
            gameObject.GetComponent<PLCConnectedScript>().enabled = false;
        }
        else
        {
            squareBorder.material = redMaterial;
            text.text = "PLC NOT CONNECTED";
            buttonBase.SetActive(true);
        }
    }
}
