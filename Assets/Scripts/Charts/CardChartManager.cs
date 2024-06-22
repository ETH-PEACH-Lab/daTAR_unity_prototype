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
    

    
    public void populateChart(List<string> data)
    {
        textTemplate = cardContainer.Find("text_template");
        textTemplate.gameObject.SetActive(false);

        for(int i = 1; i < cardContainer.childCount; i++)
        {
            Destroy(cardContainer.GetChild(i).gameObject);
        }

        Collection collection = CollectionManager.Instance.getCollection(collectionName);
        //Debug.Log("searching ");
        string[] attributes = null;
        if(collection != null )
        {
            //Debug.Log(collection.Name);
            attributes = collection.Attributes.Split(", ");
        }

        for (int i = 0; i < data.Count; i++)
        {
            Transform listItem = Instantiate(textTemplate, cardContainer);
            listItem.gameObject.SetActive(true);
            if( attributes != null )
            {
                //Debug.Log("attr value " + attributes[i].Trim());
                listItem.GetComponent<TMPro.TextMeshProUGUI>().text = attributes[i].Trim() + " : ";

            }
            //Debug.Log("data value " + data[i]);
            listItem.GetComponent<TMPro.TextMeshProUGUI>().text += data[i];
            //Debug.Log("list item "+listItem.GetComponent<TMPro.TextMeshProUGUI>().text);
        }

    }
}
