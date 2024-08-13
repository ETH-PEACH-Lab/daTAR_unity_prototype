using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;

public class ScatterPlotManager : MonoBehaviour, IChart
{
    public TMPro.TextMeshProUGUI labelX;
    public TMPro.TextMeshProUGUI labelY;
    public TMPro.TextMeshProUGUI labelZ;
    public GameObject zAxis;

    public Transform colorCodeContainer;
    public Transform colorCodeTemplate;

    public Transform pointTemplate;
    public string collectionName { get; set; }
    public int selectedRowId { get; set; }

    private List<Dictionary<string, string>> dataTable = new List<Dictionary<string, string>>();
    private string[] axisLabels = new string[3] {"none","none","none"};
    private Dictionary<string, Color32> colorCodes = new Dictionary<string, Color32>();
    private string colorCodeColumn = "none";
    private float scalingFactor = 15f;
    Dictionary<string, string> settings =
              new Dictionary<string, string>(){
                                  {"x-axis", "tabel_column"},
                                  {"y-axis", "tabel_column"},
                                  {"z-axis", "tabel_column"},
                                  {"color-code" , "tabel_column"} 
              };

    private void Start()
    {
        pointTemplate.gameObject.SetActive(false);
        colorCodeTemplate.gameObject.SetActive(false);
    }
    public void populateChart(string rowId)
    {

    }

    public void populateChart(List<Dictionary<string, string>> table)
    {
        dataTable = table;
        if(table == null || table.Count == 0)
        {
            Debug.Log("empty data table 0000");
            return;
        }
        clear();
        //take first entry for the attributes and assume same attributes for all entries , skip first attribute id
        string[] attributes = table[0].Keys.Skip(1).ToArray();
        for (int i = 0; i < 3 && i < attributes.Length; i++)
        {
            if (axisLabels[i] == "none")
            {
                changeAxis(attributes[i], i);
            }
            
            if (i == 2)
            {
                zAxis.SetActive(true);
            }
        }
        List<int> activeUnits = UnitManager.Instance.getUnits(collectionName);
        //store created points for normalization
        List<Transform> points = new List<Transform>();
        List<Vector3> positions = new List<Vector3>();
        Vector3 minValue = new Vector3(0,0,0);
        Vector3 maxValue = new Vector3(1f,1f,1f);

        foreach (Dictionary<string, string> row in table)
        {
            Transform newPoint = Instantiate(pointTemplate, transform);
            Vector3 pos = Vector3.zero;
            //apply color code
            if (row.ContainsKey(colorCodeColumn))
            {
                string cleanData = row[colorCodeColumn];
                cleanData = Regex.Replace(cleanData, @"\W", "");
                MeshRenderer mr = newPoint.Find("data_point").GetComponent<MeshRenderer>();
                mr.material.color = colorCodes[cleanData];
            }
            for (int i = 0; i < 3; i++)
            {
                if (axisLabels[i] != "none")
                {
                    float value = 0f;
                    if (row.ContainsKey(axisLabels[i]))
                    {
                        if (float.TryParse(row[axisLabels[i]], out value))
                        {
                            switch (i)
                            {
                                case 0:
                                    pos.x = value;
                                    if (value < minValue.x) minValue.x = value;
                                    if (value > maxValue.x) maxValue.x = value;
                                    break;
                                case 1:
                                    pos.y = value;
                                    if (value < minValue.y) minValue.y = value;
                                    if (value > maxValue.y) maxValue.y = value;
                                    break;
                                case 2:
                                    pos.z = value;
                                    if (value < minValue.z) minValue.z = value;
                                    if (value > maxValue.z) maxValue.z = value;
                                    break;
                            }
                        }
                    }

                }
            }

            //newPoint.localPosition = pos;
            points.Add(newPoint);
            positions.Add(pos);
            newPoint.gameObject.SetActive(true);
            //highlight active units
            int rowId = int.Parse(row["id"]);
            Debug.Log("active unit" + rowId);
            if (activeUnits != null && activeUnits.Contains(rowId))
            {
                Debug.Log("highligh unit "+pos);
                newPoint.Find("highlight").gameObject.SetActive(true);
            }
        }
        //data normalization
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 normalizedPosition = new Vector3((positions[i].x - minValue.x)/(maxValue.x - minValue.x), (positions[i].y - minValue.y)/(maxValue.y - minValue.y), (positions[i].z - minValue.z)/(maxValue.z - minValue.z));
            normalizedPosition = scalingFactor * normalizedPosition;
            //Debug.Log("pos scatter plot " + normalizedPosition);
            points[i].localPosition = normalizedPosition;
        }
    }

    public void populateChart(Collection collection)
    {
        

        string tableName = collection.Name + collection.Id;
        collectionName = collection.Name;
        
        dataTable = CollectionManager.Instance.getDataTable(tableName);
        
        populateChart(dataTable);

    }

    private void clear()
    {
        //axisLabels = new string[3] { "none", "none", "none" };
        for (int i = 0; i< transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if(i > 4) //IMPROTANAT WHEN ADDING NEW CHILD TRANSFORMS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            {
                Destroy(child.gameObject);
            }
        }
    }
    private void changeAxis(string axisName, int axis)
    {
        axisLabels[axis] = axisName;
        labelX.text = axisLabels[0];
        labelY.text = axisLabels[1];
        labelZ.text = axisLabels[2];
        zAxis.SetActive(false);
    }

    private void setColorCodes(string columnName)
    {
        Color32[] possiblecolors = new Color32[5] {new Color32(252, 186, 3,255), new Color32(252, 40, 3, 255), new Color32(17, 35, 237, 255), new Color32(201, 201, 201, 255), new Color32(230, 26, 237, 255) };
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
                    colorCodes[cleanData] = possiblecolors[index % possiblecolors.Length];

                    index++;
                } 
            }
        }
        //clear any previus color codes
        for (int i = 0; i < colorCodeContainer.childCount; i++)
        {
            if(i>0)
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
    public Dictionary<string, string> getSettings()
    {
        return settings;
    }

    public void applySetting(string settingName, string value)
    {
        switch (settingName)
        {
            case "x-axis":
                changeAxis(value, 0);
                break;
            case "y-axis":
                changeAxis(value, 1);
                break;
            case "z-axis":
                changeAxis(value, 2);
                break;
            case "color-code":
                setColorCodes(value);
                break;
        }

        populateChart(dataTable);
    }
}
