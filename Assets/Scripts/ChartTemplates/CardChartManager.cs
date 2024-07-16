using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class CardChartManager : MonoBehaviour, IChart
{
    public Transform cardContainer;
    public Transform unitHighlight;
    public string collectionName {  get; set; }

    public int selectedRowId { get; set; } = -1;
    private Transform textTemplate;
    
    
    
    public void populateChart(string rowId)
    {
        //add unit for highlighting
        if(selectedRowId > -1)
        {
            UnitManager.Instance.removeUnit(collectionName, selectedRowId);
        }
        selectedRowId = int.Parse(rowId);
        UnitManager.Instance.addUnit(collectionName, selectedRowId);
        unitHighlight.gameObject.SetActive(true);
        MeshRenderer mr = unitHighlight.GetComponent<MeshRenderer>();
        mr.material.color = Color.red;
        //Debug.Log("added unit " + collectionName + " " + int.Parse(rowId));

        textTemplate = cardContainer.Find("text_template");
        textTemplate.gameObject.SetActive(false);

        for(int i = 1; i < cardContainer.childCount; i++)
        {
            Destroy(cardContainer.GetChild(i).gameObject);
        }

        Collection collection = CollectionManager.Instance.getCollection(collectionName);
        //Debug.Log("searching ");
        string tableName = collection.Name + collection.Id.ToString();
        Dictionary<string,string> data = CollectionManager.Instance.getDataTableRow(tableName,rowId);

        if(data != null )
        {
            foreach(KeyValuePair<string,string> cell in data)
            {
                if(cell.Key != "id")
                {
                    Transform listItem = Instantiate(textTemplate, cardContainer);
                    listItem.GetComponent<TMPro.TextMeshProUGUI>().text = cell.Key.Trim() + " : " + cell.Value;
                    listItem.gameObject.SetActive(true);
                }
            }
        }

        

    }

    public void populateChart(Collection collection)
    {

    }

    public void populateChart(List<Dictionary<string, string>> table)
    {

    }
}
