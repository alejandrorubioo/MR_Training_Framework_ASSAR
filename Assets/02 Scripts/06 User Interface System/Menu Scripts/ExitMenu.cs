using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.GameManagement
{
    public class ExitMenu : MonoBehaviour
    {
        private GameManager _gameManager;
        private bool _exitMenu;

        [Header("GameObject Needed")]
        public GameObject GameManager;
        public GameObject m_ExitMenu;

        private void Start()
        {
            _gameManager = GameManager.GetComponent<GameManager>();
        }

        #region Exit Game Calls
        public void ExitButton()
        {
            _exitMenu = !_exitMenu;
            m_ExitMenu.SetActive(m_ExitMenu);
            // Exit Menu Animation and Pause
        }

        public void ConfirmExit()
        {
            _gameManager.QuitShowcase();
        }

        private void Update()
        {
            if (Input.GetKeyDown("escape"))
            {
                Debug.Log("escape");
                ExitButton();
            }

            if (m_ExitMenu.activeSelf && Input.GetKeyDown("enter"))
            {
                Debug.Log("enter");
                ConfirmExit();
            }
        }
        #endregion
    }
}