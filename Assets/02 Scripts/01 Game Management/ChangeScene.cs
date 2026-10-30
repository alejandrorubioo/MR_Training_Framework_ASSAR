using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MR_Training_ASSAR.GameManagement
{
    public class ChangeScene : MonoBehaviour
    {
        private ShowcaseSelection showcaseSelection;
        public bool changesc;
        public int scenenumber;
        private GameManager _gameManager;
        public static ChangeScene changeScene;
        private int numberOfScenes;

        // Start is called before the first frame update
        void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {
            if (changesc)
            {
                changetoscene(scenenumber);
                changesc = false;
            }
        }

        public void changetoscene(int numberscene)
        {
            numberOfScenes = ShowcaseSelection.showcaseSelection.transform.childCount;
            if (numberOfScenes > numberscene)
            {
                GameManager.gameManager.ChangeShowcase(numberscene);
            }
            else
            {
                ShowcaseSelection.showcaseSelection.GetComponent<ShowcaseSelection>().StopSelection();
                ShowcaseSelection.showcaseSelection.GetComponent<ShowcaseSelection>().StartSelection();
            }
        }
    }
}