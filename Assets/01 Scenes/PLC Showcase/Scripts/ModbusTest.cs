using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyModbus;
using System;
using NaughtyAttributes;

public class ModbusTest : MonoBehaviour
{
    public string IPAdress = "192.0.2.15";
    public bool connectModbus = false;
    public float communicationfrequenceHZ = 10f;
    //Inputs and Outputs from Game4Automation
    public game4automation.PLCInputBool[] inputs    = new game4automation.PLCInputBool[16];
    public game4automation.PLCOutputBool[] outputs  = new game4automation.PLCOutputBool[16];
    //Bool inputs and outputs to save the data from Codesys
    private bool[] bool_outputs   = new bool[32]; 
    private bool[] bool_inputs    = new bool[32];
    //Values for data transfer
    private float prevTime = 0;
    public bool connected = false;

    private ModbusClient modbusClient;

    // Start is called before the first frame update
    void Start()
    {
        communicationfrequenceHZ = 1 / communicationfrequenceHZ;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug
        if (connectModbus) ConnectModBus(); connectModbus = false;

        //If it is connected it start to update values
        if (connected)
        {
            if (Time.time - prevTime > 0.1f)
            {
                WriteInputs();
                ReadOutputs();
                prevTime = Time.time;
            }
        }
        
    }

    public void ConnectModBus()
    {
        if (!connected)
        {
            try
            {
                modbusClient = new ModbusClient(IPAdress, 502);
                modbusClient.ConnectionTimeout = 5000;  //Increase Timeout 
                modbusClient.Connect();
                Debug.Log($"Modbus Client succesfully connected to {IPAdress}");
                connected = true;
            }
            catch (Exception e)
            {
                Debug.Log("Error Connection: " + e.Message + "\t" + "Restart the PLC turning OFF and ON the swithces inside");
                connected = false;
            }
        } 
    }

    void WriteInputs()
    {
        //Write
        for (int i=0; i<inputs.Length; i++)
        {   if(inputs[i] != null)
            {
                bool_inputs[i] = inputs[i].Value;
            }
        }

        //Connection succesfull bit 31
        bool_inputs[31] = true;
        //Write inputs in codesys
        modbusClient.WriteMultipleCoils(0, bool_inputs);
    }

    void ReadOutputs()
    {
        bool[] _output_bools = modbusClient.ReadDiscreteInputs(0, outputs.Length);
        bool_outputs = _output_bools;

        //Read
        for (int i = 0; i < outputs.Length; i++)
        {
            if (outputs[i] != null)
            {
                outputs[i].Value = bool_outputs[i];
            }      
        }
    }

    public void DisconnectClient()
    {
        if (connected)
        {
            //All to 0
            for (int i = 0; i < inputs.Length; i++)
            {
                    bool_inputs[i] = false;
            }
            modbusClient.WriteMultipleCoils(0, bool_inputs);

            modbusClient.Disconnect();
            modbusClient = null;
        }
        connected = false;
        Debug.Log("Modbus Client disconnected");

    }
}
