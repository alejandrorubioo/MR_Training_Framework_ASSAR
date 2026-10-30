using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game4automation;
using MR_Training_ASSAR;

public class AttachBattery : MonoBehaviour
{
    public PLCOutputBool CloseGripperBattery;
    public PLCOutputBool CloseGripperBox;
    public PLCOutputBool OpenGripper;
    public GameObject Gripper;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        //In case it detecs a battery
        if (other.gameObject.layer == 16)
        {
            other.gameObject.transform.SetParent(this.gameObject.transform);
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(WaitToSetParentBox(other));

        }
        else if (other.gameObject.layer == 25)
        {

            StartCoroutine(WaitToSetParentBattery(other));
            other.gameObject.layer = 0;
        }

    }
    private IEnumerator WaitToSetParentBox(Collider other)
    {
        yield return new WaitUntil(() => (CloseGripperBox.Value == true));

  
        Gripper.GetComponent<Gripper>().GripperWidth = 150;
        Gripper.GetComponent<Gripper>().TimeClosing = 6;
        Gripper.GetComponent<Gripper>().TimeOpening = 6;
        Gripper.GetComponent<Gripper>().Open = false;
        Gripper.GetComponent<Gripper>().Close = true;
        other.gameObject.transform.SetParent(this.gameObject.transform);
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(WaitToResetParent(other));

        yield break;
    }
    private IEnumerator WaitToSetParentBattery(Collider other)
    {
        yield return new WaitUntil(() => (CloseGripperBattery.Value == true));// For the real robot follower to attach the battery-->|| GripperBehaviour.closeGripper);
        GripperBehaviour.closeGripper = false;

        Gripper.GetComponent<Gripper>().GripperWidth = 45;
        Gripper.GetComponent<Gripper>().TimeClosing = 2;
        Gripper.GetComponent<Gripper>().TimeOpening = 2;
        Gripper.GetComponent<Gripper>().Open = false;
        Gripper.GetComponent<Gripper>().Close = true;
        other.gameObject.transform.SetParent(this.gameObject.transform);
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(WaitToResetParent(other));

        yield break;
    }
    private IEnumerator WaitToResetParent(Collider other)
    {
        yield return new WaitUntil(() => OpenGripper.Value == true );//For the real robot follower to detach the battery-->|| GripperBehaviour.openGripper);
        GripperBehaviour.openGripper = false;


        Gripper.GetComponent<Gripper>().Close = false;
        Gripper.GetComponent<Gripper>().Open = true;
        if (other.gameObject.layer == 16)
        {
            other.gameObject.transform.SetParent(null);


        }
        else if (other.gameObject.layer == 0)
        {
           

            other.gameObject.transform.SetParent(GameObject.FindGameObjectWithTag(GLV.DinamicParenttag).transform);
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            other.gameObject.GetComponent<Rigidbody>().isKinematic=true;
        }
        yield break;
    }

}
