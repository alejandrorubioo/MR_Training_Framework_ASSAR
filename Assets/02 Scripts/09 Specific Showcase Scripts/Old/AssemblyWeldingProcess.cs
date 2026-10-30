using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

/*
public class AssemblyWeldingProcess : MonoBehaviour
{
    private List<GameObject> visibleFolder;
    private GameObject VisibleComponent;
    public GameObject ShowcaseData;
    public GameObject ShowcaseManager;
    //private MagneticBehaviour magneticScript;

    private int currentChild = 0;

    //At first, is all shutten down
    private void Awake()
    {
        DeactivatePutOnPlace();
        magneticScript = visibleFolder[currentChild].GetComponent<MagneticBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        EnableNextChildren();
    }
    public void DeactivatePutOnPlace()
    {
        visibleFolder = ShowcaseData.GetComponent<ShowcaseDataInterface>().GetStepsList(); //Current gameObject in which the script is attached

        for (int i = 0; i < visibleFolder.Count; i++)
        {
            visibleFolder[i].GetComponent<MagneticBehaviour>().PutOnPlace.SetActive(false);
        }
    }

    //When it enables, the process start
    public void StartProcess()
    {
        //Disable all invisible objects
        for (int i = 0; i < visibleFolder.Count; i++)
        {
            visibleFolder[i].GetComponent<MagneticBehaviour>().PutOnPlace.SetActive(false);
        }
        //Enabling the first invisible component
        visibleFolder[0].GetComponent<MagneticBehaviour>().PutOnPlace.SetActive(true);
    }

    /*
    public void EnableNextChildren() // No es necesario
    {
        //If current child is attached, enable and go to the next child.
        if (visibleFolder[currentChild].GetComponent<MagneticBehaviour>().attached)
        {
            //Check if currentchild is not in the limit
            if (currentChild < visibleFolder.Count - 1)
            {
                currentChild++;
                //Enable next child
                visibleFolder[currentChild].GetComponent<MagneticBehaviour>().PutOnPlace.SetActive(true);
                //Enables the next step in the showcase Manager
                ShowcaseManager.GetComponent<ShowcaseManager>().NextStep();
            }
        }
    }*//*

    public void SkipStep()
    {
        //Both objects must have the same components
        visibleFolder[currentChild].transform.position = magneticScript.GetComponent<MagneticBehaviour>().PutOnPlace.transform.position;
        visibleFolder[currentChild].transform.rotation = magneticScript.GetComponent<MagneticBehaviour>().PutOnPlace.transform.rotation;

        //Interaction feaures deactivated
        visibleFolder[currentChild].GetComponent<Collider>().enabled = false;
        magneticScript.GetComponent<MagneticBehaviour>().GetComponent<Collider>().enabled = false;
        //Disable grasping
        visibleFolder[currentChild].GetComponent<InteractionBehaviour>().enabled = false;
        //Activate collision sensor 
        magneticScript.GetComponent<MagneticBehaviour>().collision = true;
        magneticScript.attached = true;


        //The next child is enabled

        if (currentChild < visibleFolder.Count - 1)
        {
            currentChild++;
            //Enable next child
            visibleFolder[currentChild].GetComponent<MagneticBehaviour>().PutOnPlace.SetActive(true);
            //Enables the next step in the showcase Manager
            ShowcaseManager.GetComponent<ShowcaseManager>().NextStep();
        }
    }
    public void DecreaseStep()
    {
        if (currentChild > 0)
        {
            currentChild--;
            //Both objects must have the same components
            visibleFolder[currentChild].transform.position = magneticScript.GetComponent<MagneticBehaviour>().PutOnPlace.transform.position;
            visibleFolder[currentChild].transform.rotation = magneticScript.GetComponent<MagneticBehaviour>().PutOnPlace.transform.rotation;

        }
        
        //The next child is enabled

        if (currentChild < visibleFolder.Count - 1)
        {
            currentChild++;
            //Enable next child
            visibleFolder[currentChild].GetComponent<MagneticBehaviour>().PutOnPlace.SetActive(true);
            //Enables the next step in the showcase Manager
            ShowcaseManager.GetComponent<ShowcaseManager>().NextStep();
        }
    }

    public void ResetProcess()
    {
        //Updates the Showcase manager
        ShowcaseManager.GetComponent<ShowcaseManager>().ResetProcess();

        //Reset visible objects and disable all invisible objects
        for (int i = 0; i < visibleFolder.Count; i++)
        {
            visibleFolder[i].GetComponent<MagneticBehaviour>().ResetValues();
            visibleFolder[i].GetComponent<MagneticBehaviour>().PutOnPlace.SetActive(false);
        }

        //Enabling the first invisible component
        visibleFolder[0].GetComponent<MagneticBehaviour>().PutOnPlace.SetActive(true);

        //Current Child index 0
        currentChild = 0;


    }

}*/
