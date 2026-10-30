using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MR_Training_ASSAR.ProcessSystem
{
    public class BrushLoadingBar : MonoBehaviour
{
        private Image imageComp;
        GameObject loadingBar;
        // Use this for initialization
        void Awake()
        {
            loadingBar = transform.parent.gameObject;
            imageComp = gameObject.GetComponent<Image>();
            imageComp.fillAmount = 0;
        }

        // Update is called once per frame
        public void OnDisable()
        {
            imageComp.fillAmount = 0;
            imageComp.enabled = false;
            loadingBar.GetComponent<Image>().enabled = false;
        }
        public void OnEnable()
        {
            imageComp.fillAmount = 0;
            imageComp.enabled = true;
            loadingBar.GetComponent<Image>().enabled = true;

        }

        public void call()
        {
            imageComp.fillAmount += 0.34f;

            if (imageComp.fillAmount < 0.35)
            {
                imageComp.color = Color.red;
            }
            else if (imageComp.fillAmount >= .02 && imageComp.fillAmount < 0.7)
            {
                imageComp.color = Color.yellow;
            }
            else if (imageComp.fillAmount > 0.5 && imageComp.fillAmount < 1)
            {
                imageComp.color = Color.green;
            }
        }
    }
}
