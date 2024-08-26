using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PieChartManager : MonoBehaviour, IChart
{
    public Transform sliceTemplate;
    public Transform container;
    public Collection collection { get; set; }

    public int selectedRowId { get; set; }

    private Dictionary<string, string> settings =
            new Dictionary<string, string>(){
                                  {"values", "xxx"},
                                  {"category", "xxx"}};

    private List<Dictionary<string, string>> dataTable = null;

    //custom attributes to visualize
    private string[] attributesToShow = new string[5]{ "fat_100g", "carbohydrates_100g", "salt_100g", "proteins_100g","fiber_100g" };

    private void Start()
    {
        sliceTemplate.gameObject.SetActive(false);
    }
    public void applySetting(string settingName, string value)
    {
        
    }

    public Dictionary<string, string> getSettings()
    {
        return settings;
    }

    public void populateChart(string rowId)
    {
        string tableName = collection.Name + collection.Id;
        Debug.Log("piechart2 " + tableName);
        Dictionary<string, string> data = CollectionManager.Instance.getDataTableRow(tableName, rowId);
        Debug.Log("piechart1 " + data.Count);
        
        Color32[] sliceColors = new Color32[5] { new Color32(252, 186, 3, 255), new Color32(252, 40, 3, 255), new Color32(17, 35, 237, 255), new Color32(65, 224, 29, 255), new Color32(230, 26, 237, 255) };
        int counter = 0;
        //store in first pass for normalization in second pass
        Dictionary<Transform,float> tmp = new Dictionary<Transform,float>();
        float total = 0;

        foreach (KeyValuePair<string, string> kvp in data)
        {
            Debug.Log("piechart3 " + kvp.Key);
            if (attributesToShow.Contains(kvp.Key))
            {
                float value = 0;
                if(float.TryParse(kvp.Value, out value))
                {
                    Transform slice = Instantiate(sliceTemplate, container);

                    // Assign color to the slice
                    slice.Find("graphic").GetComponent<Image>().color = sliceColors[counter % 5];
                    Debug.Log("pie chart c " + counter % 5 + kvp.Key);
                    slice.Find("label").GetComponent<TMPro.TextMeshProUGUI>().text = kvp.Key;
                    slice.gameObject.SetActive(true);
                    tmp[slice] = value;
                    total += value;
                    counter++;
                }
            }
        }

        //data normalization
        float zRotation = 0;
        foreach (KeyValuePair<Transform,float> kvp in tmp)
        {
            float percentage = kvp.Value / total;
            float anglePer = percentage * 360f;
            kvp.Key.transform.localRotation = Quaternion.Euler(0f, 0f, -zRotation);
            kvp.Key.Find("graphic").GetComponent<Image>().fillAmount = percentage;
            zRotation += anglePer;
        }
    }

    public void populateChart(Collection collection)
    {
        this.collection = collection;

        string tableName = collection.Name + collection.Id;
        dataTable = CollectionManager.Instance.getDataTable(tableName);

        populateChart(dataTable);
    }

    public void populateChart(List<Dictionary<string, string>> table)
    {
        clear();
        Debug.Log("piechart0 ");
        populateChart("1");
    }

    private void clear()
    {
        for (int i = 0; i < container.childCount; i++)
        {
            if(i>0)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }
}
