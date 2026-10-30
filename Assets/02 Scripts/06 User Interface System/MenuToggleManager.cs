using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.UserInterfaceSystem
{
    public class MenuToggleManager : MonoBehaviour
    {
        #region Variables
        private string _MenuContainerTag = "Menu Container";
        public List<GameObject> _MenuContainers = new List<GameObject>();
        public List<MenuToggle> menuToggles { get; } = new List<MenuToggle>();

        private string _MenuDisplayTag = "Menu Display";
        private GameObject _MenuDisplay;
        public List<GameObject> _MenuDisplays = new List<GameObject>();
        #endregion

        private void Awake()
        {
            
        }

        private void Start()
        {
            _MenuContainers = gameObject.GetChildrenWithTag(_MenuContainerTag);
            foreach (GameObject MenuContainer in _MenuContainers)
            {
                _MenuDisplays.AddRange(MenuContainer.GetChildrenWithTag(_MenuDisplayTag));
                MenuContainer.MustHaveComponent<MenuToggle>();
                menuToggles.Add(MenuContainer.GetComponent<MenuToggle>());
            }
        }

        public void ManageMenus()
        {
            for (int i = 0; i < _MenuDisplays.Count; i++)
            {
                if (menuToggles[i].currentMenu)
                {
                    _MenuDisplays[i].SetActive(true);
                }
                else
                {
                    _MenuDisplays[i].SetActive(false);
                }
            }
        }

        public void DeactivateMenus()
        {
            foreach (MenuToggle menuToggle in menuToggles)
            {
                if (menuToggle.currentMenu)
                {
                    menuToggle.currentMenu = false;
                }
            }
            
        }

    }

}

