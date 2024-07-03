using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputNodeManager : MonoBehaviour
{
    public GameObject typeMenu;
    public GameObject settingMenu;

    public FromSelection fromSelection;

    private Collection selectedCollection = null;
    private bool dynamicInput = true; //input data through connecting nodes from other analytical pieces
    void Start()
    {
        initMenu();
    }

    public void setCollection(Collection collection)
    {
        Debug.Log("selected " + collection.Name);
        dynamicInput = true;
        selectedCollection = collection;
        typeMenu.SetActive(true);
        settingMenu.SetActive(true);
        
    }

    private void initMenu()
    {
        typeMenu.SetActive(false);
        settingMenu.SetActive(false);
        fromSelection.resetSelection();
    }
}
