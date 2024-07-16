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
        string[] attributes = table[0].Keys.Skip(1).ToArray();
        for (int i = 0; i < 3 && i < attributes.Length; i++)
        {

            changeAxis(attributes[i], i);
            if (i == 2)
            {
                zAxis.SetActive(true);
            }
        }

        foreach (Dictionary<string, string> row in table)
        {
            Transform newPoint = Instantiate(pointTemplate, transform);
            Vector3 pos = Vector3.zero;

            for (int i = 0; i < 3; i++)
            {
                if (axisLabels[i] != "none")
                {
                    float value = 0f;
                    Debug.Log("dynamic1");
                    if (row.ContainsKey(axisLabels[i]))
                    {
                        Debug.Log("dynamic2");
                        if (float.TryParse(row[axisLabels[i]], out value))
                        {
                            Debug.Log("dynamic3");
                            switch (i)
                            {
                                case 0:
                                    pos.x = value / 10.0f;
                                    break;
                                case 1:
                                    pos.y = value / 50.0f;
                                    break;
                                case 2:
                                    pos.z = value / 5.0f;
                                    break;
                            }
                        }
                    }

                }
            }

            newPoint.localPosition = pos;
            Debug.Log("dynamic4 " + pos);
            //normalization does not work as intended

            newPoint.gameObject.SetActive(true);

            
        }
    }

    public void populateChart(Collection collection)
    {
        
        clear();
        Debug.Log("cleared");
        collectionName = collection.Name;
        string[] attributes = collection.Attributes.Split(", ");
        for(int i = 0; i < 3 && i < attributes.Length;i++)
        {
            
            changeAxis(attributes[i],i);
            if (i == 2)
            {
                zAxis.SetActive(true);
            }
        }

        string tableName = collection.Name + collection.Id;
        List<int> activeUnits = UnitManager.Instance.getUnits(collectionName);
        List<Dictionary<string, string>> dataTable = CollectionManager.Instance.getDataTable(tableName);
        

        foreach(Dictionary<string,string> row in dataTable)
        {
            Transform newPoint = Instantiate(pointTemplate,transform);
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
                                    pos.x = value /10.0f;
                                    break;
                                case 1:
                                    pos.y = value;
                                    break;
                                case 2:
                                    pos.z = value;
                                    break;
                            }
                        }
                    }
                   
                }
            }
            
            newPoint.localPosition = pos;
            //normalization does not work as intended
           
            newPoint.gameObject.SetActive(true);

            //highlight active units
            int rowId = int.Parse(row["id"]);

            if (activeUnits != null && activeUnits.Contains(rowId))
            {
                //Debug.Log("highligh unit "+pos);
                MeshRenderer mr = newPoint.GetComponent<MeshRenderer>();
                mr.material.color = Color.red;
                newPoint.localScale = new Vector3(0.7f,0.7f,0.7f);
            }
        }

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
    public void changeAxis(string axisName, int axis)
    {
        axisLabels[axis] = axisName;
        labelX.text = axisLabels[0];
        labelY.text = axisLabels[1];
        labelZ.text = axisLabels[2];
        zAxis.SetActive(false);
    }
}
