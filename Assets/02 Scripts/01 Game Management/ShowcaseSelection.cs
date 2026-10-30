using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.GameManagement
{
    public class ShowcaseSelection : MonoBehaviour
    {
        public int currentShowcase;
        public static GameObject showcaseSelection;

        ChangeScene _changeScene;
        public bool start = false;

        private void Update()
        {
            if(start)
            {
                start = false;
                StartShowcase();
            }
        }

        public void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            showcaseSelection = gameObject;
            _changeScene = ChangeScene.changeScene;
        }

        public void StopSelection()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public void StartSelection()
        {
            currentShowcase = 0;
            SelectShowcase(currentShowcase);
        }

        public void StartShowcase()
        {
                _changeScene.changetoscene(currentShowcase + 1);
        }

        private void SelectShowcase(int _index)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i == _index);

            }
        }
        // Start is called before the first frame update

        public void NextButton()
        {
            currentShowcase++;
            if (currentShowcase > transform.childCount - 1)
            {
                currentShowcase = 0;
            }
            SelectShowcase(currentShowcase);
        }

        public void PreviousButton()
        {
            currentShowcase--;
            if (currentShowcase < 0)
            {
                currentShowcase = transform.childCount - 1;
            }
            SelectShowcase(currentShowcase);
        }

        /*
        public void ChangeShowcase(int _change)
        {
            currentShowcase += _change;
            SelectShowcase(currentShowcase);
        }*/
    }
}