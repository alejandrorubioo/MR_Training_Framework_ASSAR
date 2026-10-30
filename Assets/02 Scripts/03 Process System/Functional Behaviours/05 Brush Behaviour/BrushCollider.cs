using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.ProcessSystem
{
    [RequireComponent(typeof(BoxCollider))]
    public class BrushCollider : MonoBehaviour
    {
        BrushBehaviour brushBehaviour;

        public GameObject loadingGO;
        private bool initialized;
        int i = 0;

        public void Initialize(GameObject obj)
        {
            loadingGO.GetComponent<BrushLoadingBar>().enabled = true;
            brushBehaviour = obj.GetComponent<BrushBehaviour>();
            initialized = true;
            i = 0;
        }

        public void TurnOff()
        {
            initialized = false;
            loadingGO.GetComponent<BrushLoadingBar>().enabled=false;
            i = 0;
        }

        void OnTriggerEnter(Collider c)
        {
            if (c.gameObject.tag == GLV.Batterytag && initialized)
            {
                loadingGO.GetComponent<BrushLoadingBar>().call();
                i++;
                if(i>=3)
                {
                    brushBehaviour.PullTrigger(c.gameObject);
                    i = 0;
                }
            }

        }

        /*void OnTriggerExit(Collider c)
        {
            if (c.gameObject == brushBehaviourGO && initialized)
            {


                initialized = false;
            }

        }*/

    }
}