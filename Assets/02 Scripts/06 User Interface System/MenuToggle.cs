using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR_Training_ASSAR.UserInterfaceSystem
{
    public class MenuToggle : MonoBehaviour
    {
        private GameObject _ImplementedMenus;
        private string _UIMtag = "Implemented Menus";
        UIM uim;

        MenuToggleManager ThisSubMenusManager;
        GameObject ParentGameObject;
        MenuToggleManager ParentMenusManager;

        [HideInInspector] public bool currentMenu = false; // getter (read only)
        private bool _HasSubmenus = false;


        private void Awake()
        {
            _ImplementedMenus = GameObject.FindGameObjectWithTag(_UIMtag);
            uim = _ImplementedMenus.GetComponent<UIM>();

            ParentGameObject = gameObject.transform.parent.gameObject;
            ParentMenusManager = ParentGameObject.GetComponent<MenuToggleManager>();

            if (gameObject.HasComponent<MenuToggleManager>())
            {
                ThisSubMenusManager = gameObject.GetComponent<MenuToggleManager>();
                if (ThisSubMenusManager.menuToggles != null)
                {
                    _HasSubmenus = true;
                }
            }
        }

        public void ButtonPressed()
        {
            currentMenu = !currentMenu;
            if (_HasSubmenus)
            {
                foreach (MenuToggle menuToggle in ThisSubMenusManager.menuToggles)
                {
                    menuToggle.currentMenu = false;
                }
                ThisSubMenusManager.ManageMenus();
            }
            if (currentMenu && uim.m_Single)
            {
                ParentMenusManager.DeactivateMenus();
                currentMenu = true;
            }
            ParentMenusManager.ManageMenus();
        }
    }
}

