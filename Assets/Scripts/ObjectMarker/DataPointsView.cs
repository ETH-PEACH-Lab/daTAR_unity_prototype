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

    public ObjectManager objectManager;

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
        string tableName = collection.Name + collection.Id;
        List<Dictionary<string,string>> table = CollectionManager.Instance.getDataTable(tableName);
        if( table != null )
        {

            foreach (Dictionary<string, string> row in table)
            {
                Transform newRow = Instantiate(rowTemplate, transform);
                Transform cellTemplate = newRow.Find("cell_template");
                cellTemplate.gameObject.SetActive(false);

                foreach (KeyValuePair<string, string> cell in row)
                {

                    if (cell.Key != "id")
                    {
                        Transform newCell = Instantiate(cellTemplate, newRow);
                        newCell.Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = cell.Value;
                        newCell.gameObject.SetActive(true);
                    }
                    else
                    {
                        newRow.name = cell.Value;
                    }
                }

                Transform captured = newRow;
                newRow.GetComponent<Button>().onClick.AddListener(() => selectRow(captured));
                newRow.gameObject.SetActive(true);

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

        for(int i = 0; i < row.childCount; i++)
        {
            Transform child = row.GetChild(i);

            child.GetComponent<Image>().color = new Color32(179, 225, 251, 255);
            //rowData.Add(child.Find("text").GetComponent<TMPro.TextMeshProUGUI>().text);
        }

        objectManager.populateChart(row.name);
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
