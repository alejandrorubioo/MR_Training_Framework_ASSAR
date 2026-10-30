using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;


namespace MR_Training_ASSAR.ProcessSystem
{
    public class ScrewCollider : MonoBehaviour
    {
        ScrewBehaviour screwBehaviour;
        SimpleLoadingDrill simpleLoading;
        private bool initialize = false;
        // Start is called before the first frame update

        public void Initialize(GameObject obj)
        {
            screwBehaviour = obj.GetComponent<ScrewBehaviour>();
            gameObject.MustHaveComponent<BoxCollider>();
            simpleLoading = gameObject.GetComponent<SimpleLoadingDrill>();
            initialize = true;
            this.enabled = true;
        }

        public void OnEnable()
        {
            if(initialize)
            {
                simpleLoading.enabled = true;
            }
        }

        public void OnDisable()
        {
            gameObject.GetComponent<SimpleLoadingDrill>().enabled=false;
            initialize = false;
        }
        // Update is called once per frame

        private void OnTriggerEnter(Collider c)
        { 
            if (c.gameObject.tag == GLV.ScrewAssambledTag && initialize)
            {
                c.gameObject.GetComponent<Collider>().enabled = false;
                simpleLoading.Loading = true;
                screwBehaviour.PullTrigger(c.gameObject);
            }
        }
    }
}
