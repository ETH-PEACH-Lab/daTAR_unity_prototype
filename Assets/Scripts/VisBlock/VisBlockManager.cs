using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class VisBlockManager : MonoBehaviour
{
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
    
    public string chartType {  get; set; }
    void Start()
    {
        initMenu();
    }

    public void setCollection(Collection collection)
    {
        Debug.Log("selected " + collection.Name);
        dynamicInput = false;
        selectedCollection = collection;
        settingMenu.SetActive(true);
        addChart();
        
    }

    public void setDynamicData(List<Dictionary<string, string>> table, Collection collection)
    {
        if (chart != null)
        {
            IChart c = chart.GetComponent<IChart>();
            c.collection = collection;
            c.populateChart(table);
        }

        dynamicInput = true;
        dataTable = table;
        selectedCollection = collection;
        menu.SetActive(true);
        settingMenu.SetActive(true);

        fromDropdown.SetActive(false);
        settingContainer.SetActive(false);
        
        fromText.GetComponent< TMPro.TextMeshProUGUI>().text = "From Table Block input";
        fromText.SetActive(true);

        resetArrows();

        addChart();
    }

    private void addChart()
    {
        //chartType = type;
        if(chart !=null)
        {
            Destroy(chart.gameObject);
        }

        if(!dynamicInput)
        {
            string tableName = selectedCollection.Name + selectedCollection.Id;
            dataTable = CollectionManager.Instance.getDataTable(tableName);
        }

        IChart c;
        switch (chartType)
            {
                case "scatter_plot":
                    chart = Instantiate(chartTemplates[0], transform);
                    c = chart.GetComponent<IChart>();
                    c.collection = selectedCollection;
                    c.populateChart(dataTable);
                    chart.gameObject.SetActive(true);
                    break;
                case "bar_chart":
                    chart = Instantiate(chartTemplates[1], transform);
                    c = chart.GetComponent<IChart>();
                    c.collection = selectedCollection;
                    c.populateChart(dataTable);
                    chart.gameObject.SetActive(true);
                    break;
            case "pie_chart":
                chart = Instantiate(chartTemplates[2], transform);
                c = chart.GetComponent<IChart>();
                c.collection = selectedCollection;
                c.populateChart(dataTable);
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
