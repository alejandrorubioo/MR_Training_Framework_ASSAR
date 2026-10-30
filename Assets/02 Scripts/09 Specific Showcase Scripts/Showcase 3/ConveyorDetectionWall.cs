using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game4automation;
using MR_Training_ASSAR.ShowcaseCommunications;
using Leap.Unity.Interaction;
using HiS.XR.Utils;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class ConveyorDetectionWall : MonoBehaviour
    {
        public GameObject conveyor1;
        public GameObject WaitStep;
        // Start is called before the first frame update
        private void OnTriggerEnter(Collider other)
        {
            //In case it detecs a box
            if (other.gameObject.tag == GLV.BoxDowntag || other.gameObject.tag == GLV.BoxUptag || other.gameObject.layer == 16)
            {
                WaitStep.GetComponent<WaitBehaviour>().StopWaiting();
                conveyor1.GetComponent<Drive>().JogForward = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            //In case it detecs a box
            if (other.gameObject.tag == GLV.BoxDowntag || other.gameObject.tag == GLV.BoxUptag || other.gameObject.layer == 16)
            {
                WaitStep.GetComponent<WaitBehaviour>().StopWaiting();
                conveyor1.GetComponent<Drive>().JogForward = false;
            }
        }
    }
}


