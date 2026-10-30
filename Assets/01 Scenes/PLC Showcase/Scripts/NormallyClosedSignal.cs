using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormallyClosedSignal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<game4automation.PLCInputBool>().Value = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
