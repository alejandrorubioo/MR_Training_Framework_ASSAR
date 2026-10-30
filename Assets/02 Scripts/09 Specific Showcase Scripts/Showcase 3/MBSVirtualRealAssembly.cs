using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBSVirtualRealAssembly : MonoBehaviour
{
    public game4automation.PLCOutputBool HMI_Inspecting;
    public List<GameObject> ObjectsToDissapear;
    public Material onMaterial;
    public MR_Training_ASSAR.ProcessSystem.SetParentForOPeration palletFinder;

    private bool MBSisReal = false;
    GameObject[] RemainingPallets;
    GameObject[] RemainingRod1;
    GameObject[] RemainingRod2;
    GameObject[] Screws;

    private void OnEnable()
    {
        //ToggleRealVirtualMBS(); //Debug
    }

    // SE QUE HAY OTRAS FORMAS PERO SON LAS 12 DE LA NOCHE
    void Update()
    {

    }

    public void ToggleRealVirtualMBS()
    {
        MBSisReal = !MBSisReal;

        if (MBSisReal)
        {
            foreach (GameObject child in ObjectsToDissapear)
                child.SetActive(false);

            RemainingPallets = GameObject.FindGameObjectsWithTag("Pallet"); foreach (GameObject obj in RemainingPallets) obj.SetActive(false);
            RemainingRod1 = GameObject.FindGameObjectsWithTag("Rod1"); foreach (GameObject obj in RemainingRod1) obj.SetActive(false);
            RemainingRod2 = GameObject.FindGameObjectsWithTag("Rod2"); foreach (GameObject obj in RemainingRod2) obj.SetActive(false);
            Screws = GameObject.FindGameObjectsWithTag("Screw"); foreach (GameObject obj in Screws) obj.SetActive(false);
        }
        else
        {
            foreach (GameObject obj in ObjectsToDissapear)
                obj.SetActive(true);

            foreach (GameObject obj in RemainingPallets) obj.SetActive(true);
            foreach (GameObject obj in RemainingRod1) obj.SetActive(true);
            foreach (GameObject obj in RemainingRod2) obj.SetActive(true);
            foreach (GameObject obj in Screws) obj.SetActive(true);

            StopAllCoroutines();
        }
    }

    public void EndMBSProcess()
    {
        //Appear mounted MBS
        Transform InvisibleRef = palletFinder.currentDinamicParent.transform.parent.Find("InvisibleReference");
        foreach (Transform child in InvisibleRef)
            child.gameObject.GetComponentInChildren<MeshRenderer>().material = onMaterial;

        foreach (GameObject obj in ObjectsToDissapear)
            obj.SetActive(true);

        foreach (GameObject obj in RemainingPallets) obj.SetActive(true);
        foreach (GameObject obj in RemainingRod1) obj.SetActive(true);
        foreach (GameObject obj in RemainingRod2) obj.SetActive(true);
        foreach (GameObject obj in Screws) obj.SetActive(true);


    }
}
