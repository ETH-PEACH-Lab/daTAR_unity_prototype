using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class DataPointsView : MonoBehaviour
{
    public Transform attrTemplate;
    public Transform headersContainer;
    public Transform rowTemplate;

    public ChartViewManager chartViewManager;

    private Transform previousSelectedRow = null;
    void Start()
    {
        attrTemplate.gameObject.SetActive(false);
        rowTemplate.gameObject.SetActive(false);
    }
    
    public void populate(Collection collection)
    {
        clear();
        string[] headers = collection.Attributes.Split(", ");
        foreach (string header in headers)
        {
            Transform clone = Instantiate(attrTemplate, headersContainer);
            clone.name = header;
            clone.gameObject.SetActive(true);
            clone.Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = header;
        }
        Debug.Log("populate "+collection.Name);
        string[] fields = CollectionManager.Instance.getDataTable(collection.Name);
        if( fields != null )
        {
            Transform newRow = null;
            List<string> row = new List<string>();
            for (int i = 0; i < fields.Length; i++)
            {
                if(i % headers.Length == 0)
                {
                    row = new List<string>();
                    row.Add(fields[i]);

                    newRow = Instantiate(rowTemplate,transform);
                    newRow.gameObject.SetActive(true);
                    newRow.Find("cell_template").Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = fields[i];
                    newRow.name = "index " + i;

                    Transform captured = newRow;
                    newRow.GetComponent<Button>().onClick.AddListener(() => selectRow(captured));
                }
                else
                {
                    if(newRow != null)
                    {
                        row.Add(fields[i]);

                        Transform newCell = Instantiate(newRow.Find("cell_template"), newRow);
                        newCell.gameObject.SetActive(true);
                        newCell.Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = fields[i];

                    }
                    
                }
            }
        }
    }

    private void selectRow(Transform row)
    {
        if(previousSelectedRow != null)
        {
            for (int i = 0; i < previousSelectedRow.childCount; i++)
            {
                Transform child = previousSelectedRow.GetChild(i);
                child.GetComponent<Image>().color = new Color32(245, 245, 245, 255);
            }
        }

        List<string> rowData = new List<string>();
        for(int i = 0; i < row.childCount; i++)
        {
            Transform child = row.GetChild(i);

            child.GetComponent<Image>().color = new Color32(179, 225, 251, 255);
            rowData.Add(child.Find("text").GetComponent<TMPro.TextMeshProUGUI>().text);
        }

        chartViewManager.populateChart(rowData);
        previousSelectedRow = row;
    }

    public void clear()
    {
        for(int i = 0; i < headersContainer.childCount; i++)
        {
            if(i > 0)
            {
                Destroy(headersContainer.GetChild(i).gameObject);
            }
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            if(i > 1)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
