using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ManualIPCommunication : MonoBehaviour
{
    public string IPToChange;
    public string subnetMask;
    public string gateway;
    public ModbusTest modbusInterface;
    // Start is called before the first frame update
    void Start()
    {
        //Check this command in internet: "Change IP through CMD"
        ProcessStartInfo processStartInfo = new ProcessStartInfo("powershell.exe", "netsh interface ipv4 set address name=\"Ethernet\" static "+IPToChange + " " + subnetMask + " " + gateway); 
        processStartInfo.CreateNoWindow = true;
        processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processStartInfo.UseShellExecute = true;
        processStartInfo.Verb = "runas";
        Process test = Process.Start(processStartInfo);
        test.WaitForExit();
        
        //UnityEngine.Debug.Log("netsh interface ipv4 set address name=\"Ethernet\" static " + IPFromPLC + " 255.255.255.0 192.0.2.1");
        UnityEngine.Debug.Log("Local IP changed to " + IPToChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        //Disconnect from server
        if(modbusInterface!=null)
            modbusInterface.DisconnectClient();

        //Check this command in internet: "Change IP through CMD"
        ProcessStartInfo processStartInfo = new ProcessStartInfo("powershell.exe", "netsh interface ipv4 set address name=\"Ethernet\" source=dhcp");
        processStartInfo.CreateNoWindow = true;
        processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processStartInfo.UseShellExecute = true;
        processStartInfo.Verb = "runas";
        Process test = Process.Start(processStartInfo);
        test.WaitForExit();

        Process.Start(processStartInfo);
        UnityEngine.Debug.Log("IP changed to default");
    }
}
