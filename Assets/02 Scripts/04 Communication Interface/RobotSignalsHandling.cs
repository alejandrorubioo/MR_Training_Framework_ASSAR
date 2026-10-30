using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game4automation;

public class RobotSignalsHandling : MonoBehaviour
{
    public PLCInputBool StartBatteryProcess;
    public PLCInputBool StartWeldingProcess;
    public PLCInputBool BatteryType1;
    public PLCInputBool BatteryType2;
    public PLCInputBool BatteryType3;
    public PLCOutputBool ack;
    public bool BatteryProcess;
    public bool WeldingProcess;

    void Start()
    {
        StartBatteryProcess.Value = false;
        StartWeldingProcess.Value = false;
        BatteryType1.Value = false;
        BatteryType2.Value = false;
        BatteryType3.Value = false;

        if(BatteryProcess)
        {
            StartBatteryTask();

        }else if (WeldingProcess)
        {
            StartWeldingTask();
        }
    }
    public void StartBatteryTask()
    {
        StartBatteryProcess.Value = true;
        StartWeldingProcess.Value = false;
    }
    public void StartWeldingTask()
    {
        StartBatteryProcess.Value = false;
        StartWeldingProcess.Value = true ;
    }
    public void PickBatteryType1()
    {
        StartCoroutine(ResetBatteryType());
        BatteryType1.Value = true;
        BatteryType2.Value = false;
        BatteryType3.Value = false;

    }

    public void PickBatteryType2()
    {
        StartCoroutine(ResetBatteryType());
        BatteryType1.Value = false;
        BatteryType2.Value = true;
        BatteryType3.Value = false;
        
    }

    public void PickBatteryType3()
    {
        StartCoroutine(ResetBatteryType());
        BatteryType1.Value = false;
        BatteryType2.Value = false;
        BatteryType3.Value = true;
        
    }

    private IEnumerator ResetBatteryType()
    {
        yield return new WaitUntil(() => ack.Value == true);
        BatteryType1.Value = false;
        BatteryType2.Value = false;
        BatteryType3.Value = false;
        gameObject.SetActive(false);
        yield break;
    }
}
