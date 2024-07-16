using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputNodeManager : MonoBehaviour
{
    public GameObject typeMenu;
    public GameObject settingMenu;
    public List<Transform> chartTemplates;

    public FromSelection fromSelection;

    private Collection selectedCollection = null;
    private bool dynamicInput = true; //input data through connecting nodes from other analytical pieces
    private Transform chart = null;
    void Start()
    {
        initMenu();
    }

    public void setCollection(Collection collection)
    {
        Debug.Log("selected " + collection.Name);
        dynamicInput = false;
        selectedCollection = collection;
        typeMenu.SetActive(true);
        settingMenu.SetActive(true);
        if(chart != null)
        {
            IChart c = chart.GetComponent<IChart>();
            c.populateChart(selectedCollection);
        }
        
    }

    public void addChart(string type)
    {
        if(!dynamicInput && chart==null)
        {
            if(type == "scatter_plot")
            {
                chart = Instantiate(chartTemplates[0],transform);
                IChart c = chart.GetComponent<IChart>();
                c.populateChart(selectedCollection);
                chart.gameObject.SetActive(true);
            }
        }
    }

    private void initMenu()
    {
        typeMenu.SetActive(false);
        settingMenu.SetActive(false);
        fromSelection.resetSelection();
    }
}
