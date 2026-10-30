using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class OpenPLCServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<InteractionButton>().isPressed)
        {
            Application.OpenURL("http://192.0.2.15/cgi-bin/bgh.cgi?reboot");
            gameObject.SetActive(false);
        }
    }
}
