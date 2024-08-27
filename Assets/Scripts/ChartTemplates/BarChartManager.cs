using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BarChartManager : MonoBehaviour, IChart
{
    public Transform container;
    public Transform dataTemplate;

    public Transform colorCodeContainer;
    public Transform colorCodeTemplate;

    private string category = "none"; //column name for data labels
    private string values = "none"; //column name for plotted values
    private Boolean settingApplied = false;
    private float scalingFactor = 20f;

    private Dictionary<string, Color32> colorCodes = new Dictionary<string, Color32>();
    private string colorCodeColumn = "none";

    Dictionary<string, string> settings =
             new Dictionary<string, string>(){
                                  {"Werte", "tabel_column"},
                                  {"Kategorie", "tabel_column"},
                                  {"color-code","tabel_column"} };

    private float spacing = 2f;
    public Collection collection { get; set; }
    public int selectedRowId { get; set; }

    private List<Dictionary<string, string>> dataTable = null;

    public void populateChart(string rowId)
    {
        clear();
    }

    //for entire collection visulization
    public void populateChart(Collection collection)
    {
        this.collection = collection;

        string tableName = collection.Name + collection.Id;
        //List<int> activeUnits = UnitManager.Instance.getUnits(collectionName);
        List<Dictionary<string, string>> dataTable = CollectionManager.Instance.getDataTable(tableName);

       populateChart(dataTable);



    }

    public void populateChart(List<Dictionary<string, string>> table)
    {
        clear();

        List<int> activeUnits = UnitManager.Instance.getUnits(collection.Name);

        if (dataTable == null)
        {
            dataTable = table;
        }
        if(!settingApplied)
        {
            //take first entry for the attributes and assume same attributes for all entries , skip first attribute id
            string[] attributes = table[0].Keys.Skip(1).ToArray();
            //default settings
            if (attributes.Length >= 1)
            {
                values = attributes[0];
                category = attributes[0];

            }
        }
  
        Vector3 pos = new Vector3(0, 0, 0);
        //store created bars for normalization
        List<Transform> bars = new List<Transform>();
        List<float> heights = new List<float>();
        float minValue = 0;
        float maxValue = 0f;

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
                //highlight active units
                int rowId = int.Parse(row["id"]);
                Debug.Log("active unit" + rowId);
                if (activeUnits != null && activeUnits.Contains(rowId))
                {
                    Debug.Log("highligh unit ");
                    newData.GetChild(1).GetChild(1).gameObject.SetActive(true);
                }

                //apply color code
                if (row.ContainsKey(colorCodeColumn))
                {
                    string cleanData = row[colorCodeColumn];
                    cleanData = Regex.Replace(cleanData, @"\W", "");
                    MeshRenderer mr = newData.GetChild(0).GetComponent<MeshRenderer>();
                    mr.material.color = colorCodes[cleanData];
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
        colorCodeTemplate.gameObject.SetActive(false);

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
        if(axisName == "Werte")
        {
            values = axisValue;
        }
        if(axisName == "Kategorie")
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
        if(settingName == "color-code")
        {
            setColorCodes(value);
        }
        else
        {
            setAxis(settingName, value);
        }
        
        populateChart(dataTable);
    }

    private void setColorCodes(string columnName)
    {
        //for nutri score A-E
        Color32[] possiblecolors = new Color32[5] { new Color32(15, 138, 74, 255), new Color32(115, 200, 44, 255), new Color32(251, 200, 7, 255), new Color32(244, 114, 22, 255), new Color32(240, 49, 32, 255) };
        int index = 0;
        colorCodes = new Dictionary<string, Color32>();
        colorCodeColumn = columnName;
        //parse data table for all possible values
        foreach (Dictionary<string, string> row in dataTable)
        {
            if (row.ContainsKey(columnName))
            {
                string cleanData = row[columnName];
                cleanData = Regex.Replace(cleanData, @"\W", "");
                if (!colorCodes.ContainsKey(cleanData))
                {
                    //hardcode for nutri score coloring
                    switch (cleanData)
                    {
                        case "a":
                            colorCodes[cleanData] = possiblecolors[0];
                            break;
                        case "b":
                            colorCodes[cleanData] = possiblecolors[1];
                            break;
                        case "c":
                            colorCodes[cleanData] = possiblecolors[2];
                            break;
                        case "d":
                            colorCodes[cleanData] = possiblecolors[3];
                            break;
                        case "e":
                            colorCodes[cleanData] = possiblecolors[4];
                            break;
                        default:
                            colorCodes[cleanData] = possiblecolors[index % possiblecolors.Length];
                            break;
                    }

                    index++;
                }
            }
        }
        //clear any previus color codes
        for (int i = 0; i < colorCodeContainer.childCount; i++)
        {
            if (i > 0)
            {
                Destroy(colorCodeContainer.GetChild(i).gameObject);
            }
        }
        //render legend for color codes
        foreach (KeyValuePair<string, Color32> pair in colorCodes)
        {
            Transform c = Instantiate(colorCodeTemplate, colorCodeContainer);
            c.Find("label").GetComponent<TMPro.TextMeshProUGUI>().text = pair.Key;
            c.Find("color").GetComponent<Image>().color = pair.Value;
            c.gameObject.SetActive(true);
        }

    }
}
