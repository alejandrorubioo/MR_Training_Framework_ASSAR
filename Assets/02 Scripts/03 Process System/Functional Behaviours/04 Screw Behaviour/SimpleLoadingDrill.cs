using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MR_Training_ASSAR.ProcessSystem
{
    public class SimpleLoadingDrill : MonoBehaviour
    {
        private Image imageComp;
        public GameObject loadingBar;
        private float CurrentTime;
        public float timer;
        public bool Loading= false;
        // Use this for initialization
        void Awake()
        {

            if (loadingBar == null)
            {
                loadingBar = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            }
            imageComp = loadingBar.GetComponent<Image>();
            imageComp.fillAmount = 0;
            timer = 0f;
            Loading = false;
        }

        public void Update()
        {
            if (Loading)
            {
                timer += 0.01f;
                call(timer);
            }
        }

        // Update is called once per frame
        public void OnDisable()
        {
            imageComp.fillAmount = 0;
            imageComp.enabled = false;
        }
        public void OnEnable()
        {
            imageComp.fillAmount = 0;
            imageComp.enabled = true;

        }



        public void call(float timerlo)
        {
                imageComp.fillAmount = timerlo;

                if (imageComp.fillAmount < 0.3)
                {
                    imageComp.color = Color.red;
                }
                else if (imageComp.fillAmount >= .02 && imageComp.fillAmount < 0.5)
                {
                    imageComp.color = Color.yellow;
                }
                else if (imageComp.fillAmount > 0.5 && imageComp.fillAmount<1)
                {
                        imageComp.color = Color.green;
                }
                else if(timerlo >= 1)
                {
                    Loading = false;
                    timer = 0;
                }
        }
    }
}