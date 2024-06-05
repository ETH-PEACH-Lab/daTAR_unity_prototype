using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartViewManager : MonoBehaviour
{
    public List<Transform> chartTemplates; 
    

    private List<Transform> charts = new List<Transform>();
    private Vector3 newPos = new Vector3(0,0,2);
    private Transform selectedChart = null;
    
    // Start is called before the first frame update
    void Start()
    {
        if(charts.Count > 0)
        {
            //inti all charts
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addChart()
    {
        Transform chart = Instantiate(chartTemplates[0],transform);
        charts.Add(chart);
        chart.gameObject.SetActive(true);
        chart.transform.localPosition = newPos;

        newPos += new Vector3(8, 0, 0);
        selectedChart = chart;
    }
}
