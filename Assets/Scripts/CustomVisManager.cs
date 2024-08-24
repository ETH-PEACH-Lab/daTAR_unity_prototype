using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomVisManager : MonoBehaviour, IBlockManager
{
    public Transform chartTemplate;

    private IChart chart;
    private Collection collection;
    private Dictionary<string, string> dataRow;
    private void Start()
    {
        
        
    }

    public void createVis()
    {
        Transform c = Instantiate(chartTemplate, transform);
        chart = c.GetComponent<IChart>();
        chart.collection = collection;
        string rowId = dataRow["id"];
        chart.populateChart(rowId);
    }

    public void addUserInput(List<Dictionary<string, string>> userInput, Collection fromCollection)
    {
        Debug.Log("pie chart " + fromCollection.Name);
        Debug.Log("pie chart 2 " + userInput.Count);
        collection = fromCollection;
        if (userInput.Count > 0)
        {
            //assume user input only contains one data row
            dataRow = userInput[0];
        }
        createVis();
    }
}
