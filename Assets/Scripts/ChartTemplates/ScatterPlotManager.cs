using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class ScatterPlotManager : MonoBehaviour, IChart
{
    public TMPro.TextMeshProUGUI labelX;
    public TMPro.TextMeshProUGUI labelY;
    public TMPro.TextMeshProUGUI labelZ;

    public Transform pointTemplate;
    public string collectionName { get; set; }

    private string[] axisLabels = new string[3] {"none","none","none"};


    private void Start()
    {
        pointTemplate.gameObject.SetActive(false);
    }
    public void populateChart(string rowId)
    {

    }

    public void populateChart(Collection collection)
    {
        clear();
        collectionName = collection.Name;
        string[] attributes = collection.Attributes.Split(", ");
        for(int i = 0; i < 3 && i < attributes.Length;i++)
        {
            if (i == 1)
            {
                //attributes[i] = "Fruitlenght";
            }
            changeAxis(attributes[i],i);
        }

        string tableName = collection.Name + collection.Id;
        List<int> activeUntis = UnitManager.Instance.getUnits(collectionName);
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

            if (activeUntis.Contains(rowId))
            {
                Debug.Log("highligh unit "+pos);
                MeshRenderer mr = newPoint.GetComponent<MeshRenderer>();
                mr.material.color = Color.red;
            }
        }

    }

    private void clear()
    {
        for(int i = 0; i< transform.childCount; i++)
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
    }
}
