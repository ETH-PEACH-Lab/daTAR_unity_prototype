using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class BarChartManager : MonoBehaviour, IChart
{
    public Transform container;
    public Transform dataTemplate;

    private string category = "none"; //column name for data labels
    private string values = "none"; //column name for plotted values

    private float spacing = 2f;
    public string collectionName { get; set; }
    public int selectedRowId { get; set; }

    public void populateChart(string rowId)
    {
        clear();
    }

    //for entire collection visulization
    public void populateChart(Collection collection)
    {
        clear();
        collectionName = collection.Name;
        string[] attributes = collection.Attributes.Split(", ");
        //default settings
        if (attributes.Length == 1 )
        {
            values = attributes[0];

        } else if (attributes.Length >= 2 )
        {
            category = attributes[0];
            values = attributes[1];
        }

        string tableName = collection.Name + collection.Id;
        //List<int> activeUnits = UnitManager.Instance.getUnits(collectionName);
        List<Dictionary<string, string>> dataTable = CollectionManager.Instance.getDataTable(tableName);

        Vector3 pos = new Vector3 (0, 0, 0);
        Debug.Log("barchart1");
        foreach (Dictionary<string, string> row in dataTable)
        {
            if (row.ContainsKey(values))
            {
                
                pos += new Vector3(spacing, 0, 0);
                
                Transform newData = Instantiate(dataTemplate, container);
                newData.localPosition = pos;

                float height = 0f;
                float.TryParse(row[values], out height);
                if (height > 300)
                {
                    height = height / 20f; //quick fix for testing to do proper data normalization
                }
                newData.GetChild(0).localPosition = new Vector3(0,height/2, 0);
                newData.GetChild(0).localScale = new Vector3(1f,height,1f);
                //Debug.Log("barchart2 " + newData.GetChild(0).position);

                if (row.ContainsKey(category))
                {
                    string label = row[category];
                    newData.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = label;
                }

                newData.gameObject.SetActive(true);
            }
        }



    }

    //for dynamic data table visulaization
    public void populateChart(List<Dictionary<string, string>> table)
    {
        clear();
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

        

        Vector3 pos = new Vector3(0, 0, 0);
        Debug.Log("barchart1");
        foreach (Dictionary<string, string> row in table)
        {
            if (row.ContainsKey(values))
            {

                pos += new Vector3(spacing, 0, 0);

                Transform newData = Instantiate(dataTemplate, container);
                newData.localPosition = pos;

                float height = 0f;
                float.TryParse(row[values], out height);
                if (height > 300)
                {
                    height = height / 20f; //quick fix for testing to do proper data normalization
                }
                newData.GetChild(0).localPosition = new Vector3(0, height / 2, 0);
                newData.GetChild(0).localScale = new Vector3(1f, height, 1f);
                //Debug.Log("barchart2 " + newData.GetChild(0).position);

                if (row.ContainsKey(category))
                {
                    string label = row[category];
                    newData.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = label;
                }

                newData.gameObject.SetActive(true);
            }
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
}
