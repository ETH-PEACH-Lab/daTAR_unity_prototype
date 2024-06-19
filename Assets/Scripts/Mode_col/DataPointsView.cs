using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPointsView : MonoBehaviour
{
    public Transform attrTemplate;
    public Transform headersContainer;
    public Transform rowTemplate;

    private CollectionManager collectionManager;
    void Start()
    {
        attrTemplate.gameObject.SetActive(false);
        rowTemplate.gameObject.SetActive(false);
        collectionManager = CollectionManager.Instance;
    }
    
    public void populate(Collection collection)
    {
        clear();
        string[] headers = collection.Attributes.Split(", ");
        foreach (string header in headers)
        {
            Transform clone = Instantiate(attrTemplate, headersContainer);
            clone.name = header;
            clone.gameObject.SetActive(true);
            clone.Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = header;
        }

        string[] fields = collectionManager.getDataTable(collection.Name);
        if( fields != null )
        {
            Transform newRow = null;
            for (int i = 0; i < fields.Length; i++)
            {
                if(i % headers.Length == 0)
                {
                    newRow = Instantiate(rowTemplate,transform);
                    newRow.gameObject.SetActive(true);
                    newRow.Find("cell_template").Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = fields[i];
                }else
                {
                    if(newRow != null)
                    {
                        Transform newCell = Instantiate(newRow.Find("cell_template"), newRow);
                        newCell.gameObject.SetActive(true);
                        newCell.Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = fields[i];
                    }
                    
                }
            }
        }
    }

    private void clear()
    {
        for(int i = 0; i < headersContainer.childCount; i++)
        {
            if(i > 0)
            {
                Destroy(headersContainer.GetChild(i).gameObject);
            }
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            if(i > 1)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
