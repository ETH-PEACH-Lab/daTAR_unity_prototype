using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ExtractCollision : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool connected = false;
    private bool chartdrawn = false;
    private List<string> columnData = new List<string>();
    private string columnName = "";
    private Transform barTemplate;
    private Transform labelTemplate;
    private Transform stringTemplate;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        barTemplate = transform.parent.Find("bar_template");
        barTemplate.gameObject.SetActive(false);
        stringTemplate = transform.parent.Find("string_template");
        stringTemplate.gameObject.SetActive(false);
        labelTemplate = transform.parent.Find("label_template");
        labelTemplate.gameObject.SetActive(false);


    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("col11 " + other.name);
        if(other.name == "operation_node")
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.material.color = Color.blue;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, other.transform.position);
            connected = true;

            GroupByCollision groupByCollision = other.gameObject.GetComponent<GroupByCollision>();
            if (groupByCollision != null)
            {
                columnData = groupByCollision.columnData;
                columnName = groupByCollision.selectedLabel;
                Debug.Log("colext " + string.Join(", ", columnData));
                if (!chartdrawn) {
                    chartdrawn = true;
                    drawChart();
                }
                    
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        //MeshRenderer mr = GetComponent<MeshRenderer>();
        //mr.material.color = Color.gray;
    }

    private void drawChart()
    {
        Transform label = Instantiate(labelTemplate,transform);
        label.gameObject.SetActive(true);
        label.GetComponent<TextMeshPro>().text = columnName;

        float x = 0;
        foreach(string s in columnData)
        {
            float h = 0;

            if(float.TryParse(s, out h))
            {
                Transform bar = Instantiate(barTemplate,transform.parent);
                bar.gameObject.SetActive(true);
                bar.transform.localPosition += new Vector3(x, h/2, 0);
                bar.transform.localScale = new Vector3(1f, h, 1f);
            }
            else
            {
                Transform stringValue = Instantiate(stringTemplate,transform.parent);
                stringValue.gameObject.SetActive(true);
                stringValue.transform.localPosition += new Vector3(x, 0, 0);
                stringValue.GetChild(1).transform.GetComponent<TextMeshPro>().text = s;
            }

            x -= 2f;
        }
    }

    private void Update()
    {
        if (connected)
        {
            lineRenderer.SetPosition(0,transform.position);
        }
    }
}
