using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game4automation;

public class GripperBehaviour : MonoBehaviour
{
    private TCPTestClient _TCP;
    public GameObject TCPCom;
    public GameObject Gripper;
    public static bool openGripper = false;
    public static bool closeGripper = false;
    void Start()
    {
        
        _TCP = TCPCom.GetComponent<TCPTestClient>();
        openGripper = false;
        closeGripper = false;

    }
    private void Update()
    {
        if (_TCP.openGripper)
        {
            OpenGripper();

        }
        else if (_TCP.closeGripper)
        {
            CloseGripper();

        }
    }
    private void OpenGripper()
    {
        _TCP.openGripper = false;
        Gripper.GetComponent<Gripper>().Close = false;
        Gripper.GetComponent<Gripper>().Open = true;
        openGripper = true;
    }

    private void CloseGripper()
    {
        _TCP.closeGripper = false;
        Gripper.GetComponent<Gripper>().Open = false;
        Gripper.GetComponent<Gripper>().Close = true;
        closeGripper = true;
    }
}
