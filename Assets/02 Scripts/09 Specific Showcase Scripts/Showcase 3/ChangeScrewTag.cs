using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MR_Training_ASSAR.ProcessSystem
{
    public class ChangeScrewTag : MonoBehaviour
    {
        public void ScrewAttached()
        {
            GameObject obj = gameObject.GetComponent<MagneticBehaviour>().TypeofObject;
            if(obj.name!="Screw1 Visible")
            {
                obj.tag = GLV.ScrewAssambledTag;
            }
            
        }
    }
}
