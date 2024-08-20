using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedBarManager : MonoBehaviour, IChart
{
    public Transform valueTemplate;

    public Collection collection { get; set; }
    public int selectedRowId { get; set; }
    public void populateChart(string rowId)
    {
        valueTemplate.gameObject.SetActive(false);
        Vector3 pos = new Vector3(0,0,0);
        List<string> data = new List<string>();
        for (int i = 0; i<data.Count; i++)
        {
            float h = 0f;
            if (float.TryParse(data[i], out h))
            {
                Transform clone = Instantiate(valueTemplate,transform);
                clone.gameObject.SetActive(true);
                clone.transform.localScale = new Vector3(2f,h, 2f);
                clone.transform.localPosition = pos + new Vector3(0, 0, h / 2);
                MeshRenderer mr = clone.GetComponent<MeshRenderer>();
                if (i == 0)
                {
                    mr.material.color = Color.red;
                }
                else
                {
                    mr.material.color = Color.green;
                }
                

                pos += new Vector3(0, 0, h);
            }
        }
    }

    public void populateChart(Collection collection)
    {

    }

    public void populateChart(List<Dictionary<string, string>> table)
    {

    }

    public Dictionary<string, string> getSettings()
    {
        return null;
    }

    public void applySetting(string settingName, string value)
    {

    }
}
