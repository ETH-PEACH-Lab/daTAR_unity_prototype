using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardChartManager : MonoBehaviour, IChart
{
    public Transform cardContainer;

    private Transform textTemplate;
    private string[] attributeNames = new string[3] { "attr 1", "attr 2", "attr 3" };
    

    
    public void populateChart(List<string> data)
    {
        textTemplate = cardContainer.Find("text_template");
        textTemplate.gameObject.SetActive(false);

        for(int i = 1; i < cardContainer.childCount; i++)
        {
            Destroy(cardContainer.GetChild(i).gameObject);
        }

        for (int i = 0; i < data.Count && i < 3; i++)
        {
            Transform listItem = Instantiate(textTemplate, cardContainer);
            listItem.gameObject.SetActive(true);
            listItem.GetComponent<TMPro.TextMeshProUGUI>().text = attributeNames[i] + ": " + data[i];
        }

    }
}
