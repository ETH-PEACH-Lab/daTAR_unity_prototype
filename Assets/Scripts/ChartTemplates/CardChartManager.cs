using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class CardChartManager : MonoBehaviour, IChart
{
    public Transform cardContainer;
    public string collectionName {  get; set; }
    private Transform textTemplate;
    

    
    public void populateChart(string rowId)
    {
        //add unit for highlighting
        UnitManager.Instance.addUnit(collectionName, int.Parse(rowId));
        Debug.Log("added unit " + collectionName + " " + int.Parse(rowId));

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
}
