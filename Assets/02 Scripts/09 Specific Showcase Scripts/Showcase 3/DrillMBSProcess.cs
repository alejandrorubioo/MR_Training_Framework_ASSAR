using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MR_Training_ASSAR.ProcessSystem;

public class DrillMBSProcess : MonoBehaviour
{
    public bool startProcess = false;   //DEBUG
    public bool resetProcess = false;
    public List<GameObject> screws;

    private GameObject visibleFolder;

    private int currentChild = 0;

    //At first, is all shutten down
    private void Awake()
    {
        visibleFolder = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG START PROCESS
        if (startProcess) { startProcess = false; StartProcess();  }
        if (resetProcess) { resetProcess = false; ResetProcess(); }


        //If current child is attached, enable and go to the next child.
        if (screws[currentChild].GetComponent<ScrewBehaviour>().attached)
        {
            //Check if currentchild is not in the limit
            if (currentChild < screws.Count - 1)
            {
                currentChild++;
                //Enable next child
                screws[currentChild].GetComponent<ScrewBehaviour>().enabled = true;
                screws[currentChild].GetComponent<Collider>().enabled = true;
            }
        }
    }

    //When it enables, the process start
    public void StartProcess()
    {
        //Disable all colliders from screws
        foreach (GameObject screw in screws)
        {
            screw.GetComponent<Collider>().enabled = false;
            screw.GetComponent<ScrewBehaviour>().enabled = false;
        }
        //Enable de first collider
        screws[0].GetComponent<ScrewBehaviour>().enabled = true;
        screws[0].GetComponent<Collider>().enabled = true;
    }


    public void ResetProcess()
    {
        foreach (GameObject screw in screws)
        {
            screw.GetComponent<ScrewBehaviour>().ResetValues();
        }
        //Enable de first collider
        screws[0].GetComponent<ScrewBehaviour>().enabled = true;
        screws[0].GetComponent<Collider>().enabled = true;

        //Current Child index 0
        currentChild = 0;
    }
}