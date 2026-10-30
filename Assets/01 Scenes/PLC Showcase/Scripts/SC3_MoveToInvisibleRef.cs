using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC3_MoveToInvisibleRef : MonoBehaviour
{
    public List<GameObject> InvisibleObjects;
    public GameObject Pallet1;

    private int index = 1;

    // Update is called once per frame
    public void MoveToRef()
    {
        GameObject currentPallet = GameObject.Find(Pallet1.name + "-" + index); index++;
        Transform invisibleRefParent = currentPallet.transform.Find("InvisibleReference");

        for (int i = 0; i < InvisibleObjects.Count; i++)
        {
            Transform initialParent = InvisibleObjects[i].transform.parent;

            InvisibleObjects[i].transform.parent = invisibleRefParent;
            InvisibleObjects[i].transform.position = invisibleRefParent.GetChild(i).transform.position;
            InvisibleObjects[i].transform.rotation = invisibleRefParent.GetChild(i).transform.rotation;
            InvisibleObjects[i].transform.parent = initialParent;
        } 
    }
}

