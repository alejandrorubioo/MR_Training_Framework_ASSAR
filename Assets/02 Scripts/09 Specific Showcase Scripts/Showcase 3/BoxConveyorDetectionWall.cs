using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game4automation;
using MR_Training_ASSAR.ShowcaseCommunications;
using Leap.Unity.Interaction;
using HiS.XR.Utils;
using MR_Training_ASSAR.ProcessSystem;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class BoxConveyorDetectionWall : MonoBehaviour
    {
        public GameObject conveyor1;
        public GameObject conveyor2;
        // Start is called before the first frame update

        private void OnTriggerEnter(Collider other)
        {
            //In case it detecs a box
            if (other.gameObject.tag == GLV.BoxDowntag || other.gameObject.tag == GLV.BoxUptag || other.gameObject.layer == 16)
            {
                conveyor1.GetComponent<Drive>().JogForward = false;
                conveyor2.GetComponent<Drive>().JogForward = false;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            conveyor1.GetComponent<Drive>().JogForward = true;
            conveyor2.GetComponent<Drive>().JogForward = true;

        }
    }
}


