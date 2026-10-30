using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.ShowcaseCommunications
{
    public class PalletCounter : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == GLV.PalletTag)
            {
                GLV.PLC_MU_index++; // Call Showcase Manager Instead
                foreach (Transform child in transform)
                {
                    if (child.gameObject.tag == GLV.WOTag)
                    {
                        child.gameObject.name += "-" + GLV.PLC_MU_index;
                    }
                }
            }
        }
    }
}

