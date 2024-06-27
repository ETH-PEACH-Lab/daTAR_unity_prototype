using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartViewManager : MonoBehaviour
{
    public List<Transform> chartTemplates; 
    public DataPointsView dataPointsView;
    public Transform tableContainer;
    public FromView fromView;
    public Transform chartContainer;
    public ArrowAnimation[] arrowAnimations;
    public GameObject addMenu;

    private List<Transform> charts = new List<Transform>();
    private Vector3 newPos = new Vector3(4,0,2);
    private Transform selectedChart = null;
    //private List<string> chartData = new List<string>();

    private CollectionManager collectionManager;
    private Collection fromCollection = null;
    // Start is called before the first frame update
    void Start()
    {
        initMenu();

        collectionManager = CollectionManager.Instance;
        if(charts.Count > 0)
        {
            //inti all charts
        }
    }

    private void initMenu()
    {
        tableContainer.gameObject.SetActive(false);
        chartContainer.gameObject.SetActive(false);
        fromView.resetSelection();
        dataPointsView.clear();

        foreach(ArrowAnimation arrow in arrowAnimations)
        {
            arrow.toggleAnimation();
        }
    }

    public void setCollection(Collection collection)
    {
        fromCollection = collection;
        dataPointsView.populate(collection);
        if(selectedChart != null)
        {
            IChart c = selectedChart.GetComponent<IChart>();
            c.collectionName = collection.Name;
        }
        Debug.Log("from "+collection.Name);
    }

    public void addChart()
    {
        initMenu();

        Transform chart = Instantiate(chartTemplates[0],transform);
        charts.Add(chart);
        chart.gameObject.SetActive(true);
        chart.transform.localPosition = newPos;
        chart.gameObject.name = "unit_card";
        Button b = chart.GetChild(0).Find("minus_btn").GetComponent<Button>();
        b.onClick.RemoveAllListeners();
        Transform captured = chart;
        b.onClick.AddListener(() => removeChart(captured));

        newPos += new Vector3(11, 0, 0);
        selectedChart = chart;
    }

    private void removeChart(Transform chart)
    {
        if (chart == selectedChart)
        {
            addMenu.SetActive(false);
        }

        charts.Remove(chart);
        Destroy(chart.gameObject);
        newPos = new Vector3(4, 0, 2);
        for (int i = 0; i < charts.Count; i++)
        {
            charts[i].transform.localPosition = newPos;
            newPos += new Vector3(11, 0, 0);
        } 
        
    }

    public void onCancel()
    {
        removeChart(selectedChart);
        selectedChart = null;
    }

    public void populateChart(List<string> data)
    {
        if(selectedChart != null)
        {
            Debug.Log("populate" + selectedChart.gameObject.name);
            IChart c = selectedChart.GetComponent<IChart>();
            c.populateChart(data);
            
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
            //populateChart(chartData); instead take data from Ichart component
        }else if (chartName == "stacked_bar")
        {
            Transform chart = Instantiate(chartTemplates[1], transform);
            chart.gameObject.SetActive(true);
            chart.transform.position = pos;
            charts.Add(chart);
            selectedChart = chart;
            chart.gameObject.name = chartName;
            //populateChart(chartData);
        }
    }
}
