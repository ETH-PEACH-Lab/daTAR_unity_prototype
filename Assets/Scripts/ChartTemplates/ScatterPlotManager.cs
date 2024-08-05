using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Unity.Properties;
using UnityEngine;

public class ScatterPlotManager : MonoBehaviour, IChart
{
    public TMPro.TextMeshProUGUI labelX;
    public TMPro.TextMeshProUGUI labelY;
    public TMPro.TextMeshProUGUI labelZ;
    public GameObject zAxis;

    public Transform pointTemplate;
    public string collectionName { get; set; }
    public int selectedRowId { get; set; }

    private string[] axisLabels = new string[3] {"none","none","none"};
    private float scalingFactor = 15f;
    Dictionary<string, string> settings =
              new Dictionary<string, string>(){
                                  {"x-axis", "tabel_column"},
                                  {"y-axis", "tabel_column"},
                                  {"z-axis", "tabel_column"} };

    private void Start()
    {
        pointTemplate.gameObject.SetActive(false);
    }
    public void populateChart(string rowId)
    {

    }

    public void populateChart(List<Dictionary<string, string>> table)
    {
        if(table == null || table.Count == 0)
        {
            return;
        }
        clear();
        //take first entry for the attributes and assume same attributes for all entries , skip first attribute id
        string[] attributes = table[0].Keys.Skip(1).ToArray();
        for (int i = 0; i < 3 && i < attributes.Length; i++)
        {

            changeAxis(attributes[i], i);
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
                MeshRenderer mr = newPoint.GetComponent<MeshRenderer>();
                mr.material.color = Color.red;
                newPoint.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
        }
        //data normalization
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 normalizedPosition = new Vector3((positions[i].x - minValue.x)/(maxValue.x - minValue.x), (positions[i].y - minValue.y)/(maxValue.y - minValue.y), (positions[i].z - minValue.z)/(maxValue.z - minValue.z));
            normalizedPosition = scalingFactor * normalizedPosition;
            Debug.Log("pos scatter plot " + normalizedPosition);
            points[i].localPosition = normalizedPosition;
        }
    }

    public void populateChart(Collection collection)
    {
        

        string tableName = collection.Name + collection.Id;
        collectionName = collection.Name;
        
        List<Dictionary<string, string>> dataTable = CollectionManager.Instance.getDataTable(tableName);
        populateChart(dataTable);

    }

    private void clear()
    {
        axisLabels = new string[3] { "none", "none", "none" };
        for (int i = 0; i< transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if(i > 3)
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

    public Dictionary<string, string> getSettings()
    {
        return settings;
    }

    public void applySetting(string settingName, string value)
    {

    }
}
