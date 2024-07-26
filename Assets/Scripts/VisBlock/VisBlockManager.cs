using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisBlockManager : MonoBehaviour
{
    public GameObject typeMenu;
    public GameObject settingMenu;
    public GameObject fromMenu;
    public GameObject menu;
    public List<Transform> chartTemplates;

    public FromSelection fromSelection;

    private Collection selectedCollection = null;
    private List<Dictionary<string, string>> dynamicData = null;
    private bool dynamicInput = true; //input data through connecting nodes from other analytical pieces
    private Transform chart = null;
    private string chartType = "none";
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

    public void setDynamicData(List<Dictionary<string, string>> table)
    {
        dynamicInput = true;
        dynamicData = table;
        menu.SetActive(true);
        typeMenu.SetActive(true);
        settingMenu.SetActive(true);
        fromMenu.SetActive(false);
    }

    public void addChart(string type)
    {
        chartType = type;
        if(chart !=null)
        {
            Destroy(chart.gameObject);
        }

        if(!dynamicInput)
        {
            switch (type)
            {
                case "scatter_plot":
                    chart = Instantiate(chartTemplates[0], transform);
                    IChart c = chart.GetComponent<IChart>();
                    c.populateChart(selectedCollection);
                    chart.gameObject.SetActive(true);
                    break;
                case "bar_chart":
                    chart = Instantiate(chartTemplates[1], transform);
                    IChart c2 = chart.GetComponent<IChart>();
                    c2.populateChart(selectedCollection);
                    chart.gameObject.SetActive(true);
                    break;
            }
        }else
        {
            switch (type)
            {
                case "scatter_plot":
                    chart = Instantiate(chartTemplates[0], transform);
                    IChart c = chart.GetComponent<IChart>();
                    c.populateChart(dynamicData);
                    chart.gameObject.SetActive(true);
                    break;
                case "bar_chart":
                    chart = Instantiate(chartTemplates[1], transform);
                    IChart c3 = chart.GetComponent<IChart>();
                    c3.populateChart(dynamicData);
                    chart.gameObject.SetActive(true);
                    break;
            }
            
        }
    }

    public Dictionary<string, string> getSettings()
    {
        if(chart == null) return null;

        IChart c = chart.GetComponent<IChart> ();
        return c.getSettings();
    }

    private void initMenu()
    {
        typeMenu.SetActive(false);
        settingMenu.SetActive(false);
        fromSelection.resetSelection();
    }
}
