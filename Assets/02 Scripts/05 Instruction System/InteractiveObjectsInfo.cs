using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will centralize all the information of the HT, RT, WO and Static Objects

public class InteractiveObjectsInfo : MonoBehaviour
{
    #region Variables

    private List<GameObject> _HT_Visible, _HT_Invisible, _WO_Invisible, _WO_Visible;
    public GameObject steps, HT_Visible, HT_Invisible, WO_Visible, WO_Invisible;

    #endregion

    #region Setters
    private void FillInLists()
    {
        //Lists start up and filling
        Add2HTVisibleList();
        Add2HTInvisibleList();
        Add2WOVisibleList();
        Add2WOInvisibleList();
    }

    //List of Visible Human Tools
    private void Add2HTVisibleList()  
    {
        _HT_Visible = new List<GameObject>();

        foreach (Transform child in HT_Visible.transform)
        {
            _HT_Visible.Add(child.gameObject);
        }
    }
    //List of  Invisible Human Tools
    private void Add2HTInvisibleList()
    {
        _HT_Invisible = new List<GameObject>();

        foreach (Transform child in HT_Invisible.transform)
        {
            _HT_Invisible.Add(child.gameObject);
        }
    }
    //List of Visible Work Objects
    private void Add2WOVisibleList()
    {
        _WO_Visible = new List<GameObject>();

        foreach (Transform child in WO_Visible.transform)
        {
            _WO_Visible.Add(child.gameObject);
        }
    }
    //List of Invisible Work Objects
    private void Add2WOInvisibleList()
    {
        _WO_Invisible = new List<GameObject>();

        foreach (Transform child in WO_Invisible.transform)
        {
            _WO_Invisible.Add(child.gameObject);
        }
    }
    #endregion
    #region Getters
    //Depending on the tag sent by the ShowcaseData, it will provide one list or other
    public List<GameObject> GetGOList(string list)
    {
        List<GameObject> objectList = new List <GameObject> ();
        switch (list){
            case "HT_Visible":
                objectList= GetHTVisibleList();
                break;
            case "HT_Invisible":
                objectList= GetHTInvisibleList();
                break;
            case "WO_Visible":
                objectList= GetWOVisibleList();
                break;
            case "WO_Invisible":
                objectList= GetWOInvisibleList();
                break;
        }
        return objectList;
    }
   
  
    //Will provide the infomration when needed
    public List<GameObject> GetHTVisibleList() { return _HT_Visible; }
    public List<GameObject> GetHTInvisibleList() { return _HT_Invisible; }
    public List<GameObject> GetWOVisibleList() { return _WO_Visible; }
    public List<GameObject> GetWOInvisibleList() { return _WO_Invisible; }
    #endregion

}