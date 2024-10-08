using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public List<Transform> chartTemplates; 
    public DataPointsView dataPointsView;
    public Transform tableContainer;

    public GameObject dataPointsSelection;
    public GameObject existingPointSelection;
    public GameObject newPoint;

    public FromView fromView;
    public NewPointView newPointView;
    public Transform newFormContainer;

    public ArrowAnimation[] arrowAnimations;
    public GameObject addMenu;

    public ScanBarcode scanner;
    public GameObject scannerContainer;

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

    /// <summary>
    /// prepares all the UI elements for the user to attach a data point to the object
    /// </summary>
    private void initMenu()
    {
        tableContainer.gameObject.SetActive(false);
        //chartContainer.gameObject.SetActive(false);
        newFormContainer.gameObject.SetActive(false);
        fromView.resetSelection();
        dataPointsView.clear();
        newPointView.clear();
        dataPointsSelection.SetActive(false);
        existingPointSelection.SetActive(false);
        newPoint.SetActive(false);
        scannerContainer.SetActive(false);

        foreach(ArrowAnimation arrow in arrowAnimations)
        {
            arrow.toggleAnimation();
        }
    }

    /// <summary>
    /// sets collection summary associated to the data point attached to the object
    /// </summary>
    /// <param name="collection">collection summary</param>
    public void setCollection(Collection collection)
    {
        fromCollection = collection;
        dataPointsView.populate(collection);
        newPointView.populate(collection);
        scanner.setAttributes(collection);
        dataPointsSelection.SetActive(true);

        if (selectedChart != null)
        {
            IChart c = selectedChart.GetComponent<IChart>();
            c.collection = collection;
        }
        Debug.Log("from "+collection.Name);
    }

    /// <summary>
    /// initialises an empty AR text overlay (cardChart)
    /// </summary>
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

        newPos += new Vector3(0, 0, 8);
        selectedChart = chart;
    }

    /// <summary>
    /// user can remove any added AR text overlay (card chart)
    /// </summary>
    /// <param name="chart"> selected card chart to remove</param>
    private void removeChart(Transform chart)
    {
        if (chart == selectedChart)
        {
            addMenu.SetActive(false);
        }
        IChart c = chart.GetComponent<IChart>();
        if(fromCollection != null)
        {
            UnitManager.Instance.removeUnit(c.collection.Name, c.selectedRowId);
        }
        charts.Remove(chart);
        Destroy(chart.gameObject);
        newPos = new Vector3(4, 0, 2);
        for (int i = 0; i < charts.Count; i++)
        {
            charts[i].transform.localPosition = newPos;
            newPos += new Vector3(0, 0, 8);
        } 
        
    }

    public void onCancel()
    {
        removeChart(selectedChart);
        selectedChart = null;
    }

    /// <summary>
    /// populates the AR text overlay (card chart)
    /// </summary>
    /// <param name="rowId">data table row id corresponding to the data point the user wants to attach to the object</param>
    public void populateChart(string rowId)
    {
        if(selectedChart != null)
        {
            Debug.Log("populate" + selectedChart.gameObject.name);
            IChart c = selectedChart.GetComponent<IChart>();
            c.populateChart(rowId);
            
        }
    }

    public void changeChart(string chartName) //not in use,potentialy drop (as for choosing different data vis for attached data point user connects to block i.e. pie chart)
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

    /// <summary>
    /// used when user scans bar code for automatic data extraction
    /// </summary>
    /// <param name="data">data extracted</param>
    public void dataExtraction(Dictionary<string, string> data)
    {
        newPointView.populateData(data);
        newFormContainer.gameObject.SetActive(true);
    }
}
