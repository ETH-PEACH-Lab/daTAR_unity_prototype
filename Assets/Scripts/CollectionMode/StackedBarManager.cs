using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedBarManager : MonoBehaviour
{
    public Transform valueTemplate;
    public void populateChart(List<string> data)
    {
        valueTemplate.gameObject.SetActive(false);
        Vector3 pos = new Vector3(0,0,0);
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
}
