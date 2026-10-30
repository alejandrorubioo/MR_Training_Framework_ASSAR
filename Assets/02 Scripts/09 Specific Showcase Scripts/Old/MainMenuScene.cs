using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : MonoBehaviour
{
    public bool _Menu;
    public GameObject MainMenu;
    // Start is called before the first frame update
    void Awake()
    {
        MainMenu.transform.gameObject.SetActive(false);
        _Menu = false;
    }


    public void Menu()
    {
        Debug.Log("Dentro1");

        if (_Menu)
        {
            Debug.Log("Dentro2");
            MainMenu.transform.gameObject.SetActive(false);
            _Menu = false;

        }
        else
        {
            Debug.Log("Dentro3");
            MainMenu.transform.gameObject.SetActive(true);
            _Menu = true;
        }


    }    
}
