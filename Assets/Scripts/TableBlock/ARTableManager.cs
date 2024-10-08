using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ARTableManager : MonoBehaviour
{
    public Transform canvas;

    public Transform columnTemplate;
    public Transform columnContainer;

    public Transform cellContainer;
    public Transform cellTemplate;

    public VisNode connectedVisNode = null;

    public Collection collection = null;
    private float tableHeight = 0;
    private float tableWidth = 0;

    public List<Dictionary<string, string>> table = null;

    public LoadTable opNodeManager = null;

    //data struct to accumulate all connected operation blocks _> be able to concat queryies i.e WHERE col_1.... ORDER BY col_2
    //key = query type i.e where, orderby 
    private Dictionary<string, string> queries = new Dictionary<string, string>() {
                                                    {"ORDER BY","" },
                                                    {"WHERE","" }
                                                 };
    private List<string> newAttributes = new List<string>();

    /// <summary>
    /// renders all entries in the data table
    /// </summary>
    /// <param name="collection">associated collection summary for data table to render</param>
    public void populate(Collection collection)
    {
        clear();
        this.collection = collection;

        string[] columnNames = collection.Attributes.Split(", ");
        foreach (string columnName in columnNames)
        {
            Transform clone = Instantiate(columnTemplate, columnContainer);
            clone.name = columnName;
            clone.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = columnName;
            clone.gameObject.SetActive(true);
            
            if (newAttributes.Contains(columnName))
            { //put newly added rows at the top of the table
                clone.SetSiblingIndex(1);
            }
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
                    foreach (string newAttr in newAttributes)
                    {
                        if (row.ContainsKey(newAttr))
                        { //put newly added rows at the top of the table
                            Transform firstRowCell = Instantiate(cellTemplate, cellContainer);
                            firstRowCell.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = row[newAttr];
                            firstRowCell.gameObject.SetActive(true);
                        }
                    }
                    
                    foreach (KeyValuePair<string, string> cell in row)
                    {
                        if (cell.Key != "id" && !newAttributes.Contains(cell.Key) )
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

    /// <summary>
    /// modifies the displayed data table based on a user sql query, changes in the data table are not reflected in the local db
    /// </summary>
    /// <param name="operation">type of sql operation to perfom (ORDER BY or WHERE)</param>
    /// <param name="condition"> empty for ORDER BY operation</param>
    /// <param name="columnName"> data table column to use for executing the sql query on</param>
    public void executeOperation(string operation, string condition, string columnName)
    {
        clearCells();

        queries[operation] = " " + operation + " " + columnName + " " + condition;

        string tableName = collection.Name + collection.Id;
        string query = "SELECT * FROM " + tableName  + queries["WHERE"] + queries["ORDER BY"];
        Debug.Log("hit "+query);
        table = CollectionManager.Instance.executeQuery(query);

        if (table != null)
        {
            foreach (Dictionary<string, string> row in table)
            {
                foreach (string newAttr in newAttributes)
                {
                    if (row.ContainsKey(newAttr))
                    { //put newly added rows at the top of the table
                        Transform firstRowCell = Instantiate(cellTemplate, cellContainer);
                        firstRowCell.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = row[newAttr];
                        firstRowCell.gameObject.SetActive(true);
                    }
                }
                foreach (KeyValuePair<string, string> cell in row)
                {
                    if (cell.Key != "id" && !newAttributes.Contains(cell.Key))
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

        updateVisBlock();
    }

    /// <summary>
    /// for loading the AR table new 
    /// </summary>
    public void edit()
    {
        if(opNodeManager != null)
        {
            opNodeManager.edit();
        }
        
    }

    /// <summary>
    /// collapsing UI data table element
    /// </summary>
    public void collapse()
    {
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(25, tableHeight);
    }

    /// <summary>
    /// extend UI data table element
    /// </summary>
    public void extend()
    {
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(tableWidth, tableHeight);
    }

    /// <summary>
    /// reset all the operation and data table entries
    /// </summary>
    private void clear()
    {
        columnTemplate.gameObject.SetActive(false);
        cellTemplate.gameObject.SetActive(false);

        queries = new Dictionary<string, string>() {
                                                    {"ORDER BY","" },
                                                    {"WHERE","" }
                                                 };

        clearCells();

        for (int i = 0; i < columnContainer.childCount; i++)
        {
            if(i > 0)
            {
                Destroy(columnContainer.GetChild(i).gameObject);
            }
          
        }

    }

  public void updateVisBlock()
    {
        if (connectedVisNode != null)
        {
            connectedVisNode.updateDataTable(table,collection);
        }
    }
 public void initVisBlock()
    {
        if (connectedVisNode != null)
        {
            connectedVisNode.setDataTable(table, collection);
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

    public void addAttribute(string attr)
    {
        newAttributes.Add(attr);
    }
}
