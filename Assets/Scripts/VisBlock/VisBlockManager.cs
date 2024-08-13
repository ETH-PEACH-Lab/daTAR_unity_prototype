using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class VisBlockManager : MonoBehaviour
{
    public GameObject typeMenu;
    public GameObject typeContainer;
    public GameObject settingMenu;
    public GameObject settingContainer;
    public GameObject fromDropdown;
    public GameObject fromText;
    public GameObject menu;

    public List<ArrowAnimation> arrowAnimationList;

    public List<Transform> chartTemplates;

    public FromSelection fromSelection;

    private Collection selectedCollection = null;
    private List<Dictionary<string, string>> dataTable = null;
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

    public void setDynamicData(List<Dictionary<string, string>> table, Collection collection)
    {
        if (chart != null)
        {
            IChart c = chart.GetComponent<IChart>();
            c.collectionName = collection.Name;
            c.populateChart(table);
        }

        dynamicInput = true;
        dataTable = table;
        selectedCollection = collection;
        menu.SetActive(true);
        typeMenu.SetActive(true);
        settingMenu.SetActive(true);

        fromDropdown.SetActive(false);
        typeContainer.SetActive(false);
        settingContainer.SetActive(false);
        
        fromText.GetComponent< TMPro.TextMeshProUGUI>().text = "From Table Block input";
        fromText.SetActive(true);

        resetArrows();
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
            string tableName = selectedCollection.Name + selectedCollection.Id;
            dataTable = CollectionManager.Instance.getDataTable(tableName);
        }
            switch (type)
            {
                case "scatter_plot":
                    chart = Instantiate(chartTemplates[0], transform);
                    IChart c = chart.GetComponent<IChart>();
                    c.collectionName = selectedCollection.Name;
                    c.populateChart(dataTable);
                    chart.gameObject.SetActive(true);
                    break;
                case "bar_chart":
                    chart = Instantiate(chartTemplates[1], transform);
                    IChart c3 = chart.GetComponent<IChart>();
                    c3.collectionName = selectedCollection.Name;
                    c3.populateChart(dataTable);
                    chart.gameObject.SetActive(true);
                    break;
            }
            
        
    }

    public Dictionary<string, string> getSettings()
    {
        if(chart == null) return null;

        IChart c = chart.GetComponent<IChart> ();
        return c.getSettings();
    }

    public void applySetting(string settingName, string settingValue)
    {
        if(chart != null)
        {
            IChart c = chart.GetComponent<IChart> ();
            c.applySetting(settingName, settingValue);
        }
    }

    public List<string> getTableColumns()
    {
        if(dataTable == null || dataTable.Count < 1) return null;
        List<string> columnNames = new List<string>();
        foreach(KeyValuePair<string, string> kvp in dataTable[0])
        {
            if (kvp.Key != "id")
            {
                columnNames.Add(kvp.Key);
            }
            
        }
        return columnNames;
    }

    private void initMenu()
    {
        typeMenu.SetActive(false);
        typeContainer.SetActive(false);

        settingMenu.SetActive(false);
        settingContainer.SetActive(false);

        fromSelection.resetSelection();

        resetArrows();
    }

    private void resetArrows()
    {
        foreach(ArrowAnimation arrow in  arrowAnimationList)
        {
            arrow.toggleAnimation();
        }
    }
}
