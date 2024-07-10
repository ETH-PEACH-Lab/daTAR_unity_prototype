using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARTableManager : MonoBehaviour
{
    public Transform columnTemplate;
    public Transform columnContainer;

    private Collection collection = null;
    public void populate(Collection collection)
    {
        clear();
        this.collection = collection;
        Debug.Log("table02 " + collection.Name);

        string[] columnNames = collection.Attributes.Split(", ");
        foreach (string columnName in columnNames)
        {
            Debug.Log("table03");
            Transform clone = Instantiate(columnTemplate, columnContainer);
            Debug.Log("table04");
            clone.name = columnName;
            clone.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = columnName;
            clone.gameObject.SetActive(true);
        }
    }
    private void clear()
    {
        columnTemplate.gameObject.SetActive(false);
        
        for (int i = 0; i < columnContainer.childCount; i++)
        {
            if(i > 0)
            {
                Destroy(columnContainer.GetChild(i));
            }
          
        }

    }
}
