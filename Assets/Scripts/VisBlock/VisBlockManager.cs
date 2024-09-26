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

    //UI dropdown icons
    public List<ArrowAnimation> arrowAnimationList;
    //all available types of data visualisations (bar chart, scatter plot)
    public List<Transform> chartTemplates;

    public FromSelection fromSelection;
    //associated collection for data table
    private Collection selectedCollection = null;
    //data table to visualise
    private List<Dictionary<string, string>> dataTable = null;
    //input data through connecting nodes from other analytical pieces
    private bool dynamicInput = true; 
    //UI element holding data visualization
    private Transform chart = null;
    
    public string chartType {  get; set; }
    void Start()
    {
        initMenu();
    }

    /// <summary>
    /// sets the collection associated with the data table to visualize, when selecting from dropdown not through connecting nodes
    /// </summary>
    /// <param name="collection">collection to set for data visualization</param>
    public void setCollection(Collection collection)
    {
        dynamicInput = false; //data visualization was set by selection dropdown not connecting nodes
        selectedCollection = collection;
        settingMenu.SetActive(true);
        addChart();
        
    }

    /// <summary>
    /// sets the collection and data table, when connecting block node
    /// </summary>
    /// <param name="collection">collection to set for data visualization</param>
    public void setDynamicData(List<Dictionary<string, string>> table, Collection collection)
    {

        dynamicInput = true; //data visualization was set by connection nodes
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

    /// <summary>
    /// initialises the data visualisation (chart) based on the chartType (scatter_plot, bar_chart)
    /// </summary>
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
        }
            
        
    }

    /// <summary>
    /// updates the data visulaisation if connected to a AR table block and the data table changes
    /// </summary>
    /// <param name="collection">collection summary associated with data  table</param>
    /// <param name="table">data table with new values</param>
    public void updateChart(List<Dictionary<string, string>> table, Collection collection)
    {
        Debug.Log("update chart");
        dataTable = table;
        selectedCollection = collection;
        if(chart != null)
        {
            Debug.Log("update chart2");
            IChart c = chart.GetComponent<IChart>();
            c.collection = selectedCollection;
            c.populateChart(dataTable);
            chart.gameObject.SetActive(true);

        }
    }
    /// <summary>
    /// looks up settings specific to each type of data visualization (chart)
    /// </summary>
    /// <returns>dictionary with setting of the data vis with matching chartType, key= name of setting, value = type of setting (tabel_column_none or table_column)</returns>
    public Dictionary<string, string> getSettings()
    {
        if(chart == null) return null;

        IChart c = chart.GetComponent<IChart> ();
        return c.getSettings();
    }

    /// <summary>
    /// if data vis initialized calls applySetting on the chart object
    /// </summary>
    /// <param name="settingName"> name of the setting to apply</param>
    /// <param name="settingValue">value of the corresponding setting to apply</param>
    public void applySetting(string settingName, string settingValue)
    {
        if(chart != null)
        {
            IChart c = chart.GetComponent<IChart> ();
            c.applySetting(settingName, settingValue);
        }
    }

    /// <summary>
    /// looks up data table attribute if one is set skippin "id" attribute
    /// </summary>
    /// <returns>list of data table attribute if data table has at least one entry otherwise empty list</returns>
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

    /// <summary>
    /// reset UI elements of block
    /// </summary>
    private void initMenu()
    {

        settingMenu.SetActive(false);
        settingContainer.SetActive(false);

        fromSelection.resetSelection();

        resetArrows();
    }

    /// <summary>
    /// reset dropdown UI elements
    /// </summary>
    private void resetArrows()
    {
        foreach(ArrowAnimation arrow in  arrowAnimationList)
        {
            arrow.toggleAnimation();
        }
    }
}
