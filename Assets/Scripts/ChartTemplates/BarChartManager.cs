using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BarChartManager : MonoBehaviour, IChart
{
    public Transform container;
    public Transform dataTemplate;

    private string category = "none"; //column name for data labels
    private string values = "none"; //column name for plotted values
    private Boolean settingApplied = false;
    private float scalingFactor = 20f;
    Dictionary<string, string> settings =
             new Dictionary<string, string>(){
                                  {"values", "tabel_column"},
                                  {"category", "tabel_column"}};

    private float spacing = 2f;
    public string collectionName { get; set; }
    public int selectedRowId { get; set; }

    private List<Dictionary<string, string>> dataTable = null;

    public void populateChart(string rowId)
    {
        clear();
    }

    //for entire collection visulization
    public void populateChart(Collection collection)
    {
        collectionName = collection.Name;

        string tableName = collection.Name + collection.Id;
        //List<int> activeUnits = UnitManager.Instance.getUnits(collectionName);
        List<Dictionary<string, string>> dataTable = CollectionManager.Instance.getDataTable(tableName);

       populateChart(dataTable);



    }

    public void populateChart(List<Dictionary<string, string>> table)
    {
        clear();
        if (dataTable == null)
        {
            dataTable = table;
        }
        if(!settingApplied)
        {
            //take first entry for the attributes and assume same attributes for all entries , skip first attribute id
            string[] attributes = table[0].Keys.Skip(1).ToArray();
            //default settings
            if (attributes.Length == 1)
            {
                values = attributes[0];

            }
            else if (attributes.Length >= 2)
            {
                category = attributes[0];
                values = attributes[1];
            }
        }
        



        Vector3 pos = new Vector3(0, 0, 0);
        //store created bars for normalization
        List<Transform> bars = new List<Transform>();
        List<float> heights = new List<float>();
        float minValue = 0;
        float maxValue = 1f;

        foreach (Dictionary<string, string> row in table)
        {
            if (row.ContainsKey(values))
            {

                pos += new Vector3(spacing, 0, 0);

                Transform newData = Instantiate(dataTemplate, container);
                newData.localPosition = pos;

                float height = 0f;
                float.TryParse(row[values], out height);
                heights.Add(height);
                bars.Add(newData.GetChild(0));
                if (height < minValue) minValue = height;
                if (height > maxValue) maxValue = height;

                if (row.ContainsKey(category))
                {
                    string label = row[category];
                    newData.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = label;
                }

                newData.gameObject.SetActive(true);
            }
        }

        //normalize data points
        for (int i = 0; i < bars.Count; i++)
        {
            float normalizedHeight = scalingFactor * ((heights[i] - minValue) / (maxValue - minValue));
            bars[i].localPosition = new Vector3(0, normalizedHeight / 2, 0);
            bars[i].localScale = new Vector3(1f, normalizedHeight, 1f);
        }
    } 

    private void clear()
    {
        dataTemplate.gameObject.SetActive(false);

        for (int i = 0; i < container.childCount; i++)
        {
            if (i > 0)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }

    private void setAxis(string axisName, string axisValue)
    {
        if(axisName == "values")
        {
            values = axisValue;
        }
        if(axisName == "category")
        {
            category = axisValue;
        }
    }

    public Dictionary<string,string> getSettings()
    {
        return settings;
    }

    public void applySetting(string settingName, string value)
    {
        settingApplied = true;
        setAxis(settingName, value);
        populateChart(dataTable);
    }
}
