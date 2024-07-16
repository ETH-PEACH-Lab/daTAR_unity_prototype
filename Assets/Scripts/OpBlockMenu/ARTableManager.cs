using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARTableManager : MonoBehaviour
{
    public Transform canvas;

    public Transform columnTemplate;
    public Transform columnContainer;

    public Transform cellContainer;
    public Transform cellTemplate;

    private Collection collection = null;
    private float tableHeight = 0;
    private float tableWidth = 0;
    private List<Dictionary<string, string>> table = null;
    public void populate(Collection collection)
    {
        clear();
        this.collection = collection;
        Debug.Log("table02 " + collection.Name);

        string[] columnNames = collection.Attributes.Split(", ");
        foreach (string columnName in columnNames)
        {
            Transform clone = Instantiate(columnTemplate, columnContainer);
            clone.name = columnName;
            clone.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = columnName;
            clone.gameObject.SetActive(true);
        }
        int rowCount = 0;

        if (columnNames.Length > 0)
        {
            cellContainer.GetComponent<GridLayoutGroup>().constraintCount = columnNames.Length;
       
            string tableName = collection.Name + collection.Id;
            table = CollectionManager.Instance.getDataTable(tableName);

            if (table != null)
            {
                foreach (Dictionary<string, string> row in table)
                {
                    foreach (KeyValuePair<string, string> cell in row)
                    {
                        if (cell.Key != "id")
                        {
                            //render cell
                            Transform newCell = Instantiate(cellTemplate, cellContainer);
                            newCell.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = cell.Value;
                            newCell.gameObject.SetActive(true);
                        }
                    }

                    rowCount++;
                }

            }

        }

        //set table dimensions depending on the entries
        tableHeight = columnNames.Length * 40 + 5;
        tableWidth = 50 * rowCount + 15;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2 (tableWidth, tableHeight);


    }

    public void executeOperation(string operation, string columnName)
    {
        clearCells();
       
        string tableName = collection.Name + collection.Id;
        string query = "SELECT * FROM " + tableName + " " + operation + " " + columnName;
        Debug.Log("hit "+query);
        table = CollectionManager.Instance.executeQuery(query);

        if (table != null)
        {
            foreach (Dictionary<string, string> row in table)
            {
                foreach (KeyValuePair<string, string> cell in row)
                {
                    if (cell.Key != "id")
                    {
                        //render cell
                        Transform newCell = Instantiate(cellTemplate, cellContainer);
                        newCell.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = cell.Value;
                        newCell.gameObject.SetActive(true);
                        Debug.Log("cell " + cell.Value);
                    }
                }

                
            }

        }


    }

    public void collapse()
    {
        Debug.Log("collapsing ffffff");
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(22, tableHeight);
    }
    public void extend()
    {
        Debug.Log("extending ffffffff");
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(tableWidth, tableHeight);
    }
    private void clear()
    {
        columnTemplate.gameObject.SetActive(false);
        cellTemplate.gameObject.SetActive(false);

        clearCells();

        for (int i = 0; i < columnContainer.childCount; i++)
        {
            if(i > 0)
            {
                Destroy(columnContainer.GetChild(i).gameObject);
            }
          
        }

    }

    private void clearCells()
    {
        for (int i = 0; i < cellContainer.childCount; i++)
        {
            if (i > 0)
            {
                Destroy(cellContainer.GetChild(i).gameObject);
            }

        }
    }
}
