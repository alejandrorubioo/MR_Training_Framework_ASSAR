using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachInvisibleWO : MonoBehaviour
{
    public game4automation.PLCOutputBool outputBool;
    public GameObject visibleWO;
    public GameObject pallet1;


    private GameObject _actualPallet;
    private int _actualPalletIndex = 1;
    private Transform startParent;
    // Start is called before the first frame update
    void Start()
    {
        startParent = gameObject.transform.parent;
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (outputBool.Value)
        {
            _actualPallet = GameObject.Find(pallet1.name + "-" + _actualPalletIndex);
            Debug.Log("Paller: " + _actualPallet.name);
            _actualPalletIndex++;

            switch (visibleWO.name)
            {
                case "Rod1":
                    visibleWO = _actualPallet.transform.GetChild(0).GetChild(1).Find("Rod1").gameObject;
                    break;
                case "Rod2":
                    visibleWO = _actualPallet.transform.GetChild(0).GetChild(1).Find("Rod2").gameObject;
                    break;

                case "Top":
                    visibleWO = _actualPallet.transform.GetChild(0).GetChild(1).Find("Top").gameObject;
                    break;

            }

            gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            gameObject.transform.parent = visibleWO.transform.parent;
            gameObject.transform.position = visibleWO.transform.position;
            gameObject.transform.rotation = visibleWO.transform.rotation;
            gameObject.transform.parent = startParent;

            //Off
            gameObject.GetComponent<AttachInvisibleWO>().enabled = false;
        }
    }
}
