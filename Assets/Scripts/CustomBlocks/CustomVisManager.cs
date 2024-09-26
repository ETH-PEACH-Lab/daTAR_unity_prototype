using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomVisManager : MonoBehaviour, IBlockManager
{
    //template for custom data visualisation (pie chart)
    public Transform chartTemplate;

    private IChart chart;
    private Collection collection;
    private Dictionary<string, string> dataRow;

    /// <summary>
    /// instansiate data visualisation and stores in chart variable
    /// </summary>
    public void createVis()
    {
        Transform c = Instantiate(chartTemplate, transform);
        chart = c.GetComponent<IChart>();
        chart.collection = collection;
        string rowId = dataRow["id"];
        chart.populateChart(rowId);
    }

    /// <summary>
    /// specify data to visualise based on connected data point by the user
    /// </summary>
    /// <param name="userInput">data table to visualize</param>
    /// <param name="fromCollection">collection summary associated to the data table</param>
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
