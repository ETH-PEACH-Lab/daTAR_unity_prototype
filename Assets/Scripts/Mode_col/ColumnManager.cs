using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnManager : MonoBehaviour
{
    public ChartViewManager chartViewManager;
    private List<string> data = new List<string>();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i > 1)
            {
                string s = transform.GetChild(i).GetComponent<TMPro.TextMeshProUGUI>().text;
                data.Add(s);
            }
        }
    }

    public void sendData()
    {

        chartViewManager.populateChart(data);
        
    }
}
