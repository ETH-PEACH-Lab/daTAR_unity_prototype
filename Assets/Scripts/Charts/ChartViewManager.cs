using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartViewManager : MonoBehaviour
{
    public List<Transform> chartTemplates; 
    public DataPointsView dataPointsView;
    

    private List<Transform> charts = new List<Transform>();
    private Vector3 newPos = new Vector3(0,0,2);
    private Transform selectedChart = null;
    private List<string> chartData = new List<string>();

    private CollectionManager collectionManager;
    private Collection fromCollection = null;
    // Start is called before the first frame update
    void Start()
    {
        collectionManager = CollectionManager.Instance;
        if(charts.Count > 0)
        {
            //inti all charts
        }
    }

    public void setCollection(Collection collection)
    {
        fromCollection = collection;
        dataPointsView.populate(collection);
        Debug.Log("from "+collection.Name);
    }

    public void addChart()
    {
        Transform chart = Instantiate(chartTemplates[0],transform);
        charts.Add(chart);
        chart.gameObject.SetActive(true);
        chart.transform.localPosition = newPos;
        chart.gameObject.name = "unit_card";

        newPos += new Vector3(8, 0, 0);
        selectedChart = chart;
    }

    public void populateChart(List<string> data)
    {
        if(selectedChart != null)
        {
            Debug.Log("populate" + selectedChart.gameObject.name);
            IChart c = selectedChart.GetComponent<IChart>();
            c.populateChart(data);
            
            chartData = data;
        }
    }

    public void changeChart(string chartName)
    {
        //Debug.Log("change"+ chartName);
        Vector3 pos = selectedChart.transform.position;
        charts.Remove(selectedChart);
        Destroy(selectedChart.gameObject);
        
        if (chartName == "unit_card")
        {
            Transform chart = Instantiate(chartTemplates[0], transform);
            chart.gameObject.SetActive(true);
            chart.transform.position = pos;
            charts.Add(chart);
            selectedChart = chart;
            chart.gameObject.name = chartName;
            populateChart(chartData);
        }else if (chartName == "stacked_bar")
        {
            Transform chart = Instantiate(chartTemplates[1], transform);
            chart.gameObject.SetActive(true);
            chart.transform.position = pos;
            charts.Add(chart);
            selectedChart = chart;
            chart.gameObject.name = chartName;
            populateChart(chartData);
        }
    }
}
