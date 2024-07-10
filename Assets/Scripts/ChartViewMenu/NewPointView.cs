using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NewPointView : MonoBehaviour
{
    public Transform headerTemplate;
    public Transform inputTemplate;
    public Transform btnTemplate;
    public Transform container;

    public ChartViewManager chartViewManager;

    private Collection fromCollection = null;
    private List<TMP_InputField> inputs;
    void Start()
    {
        headerTemplate.gameObject.SetActive(false);
        inputTemplate.gameObject.SetActive(false);
        btnTemplate.gameObject.SetActive(false);
      
    }

    public void populate(Collection collection)
    {
        fromCollection = collection;
        string[] headers = collection.Attributes.Split(", ");
        inputs = new List<TMP_InputField>();

        foreach (string header in headers)
        {
            Transform headerClone = Instantiate(headerTemplate, container);
            headerClone.GetComponent<TMPro.TextMeshProUGUI>().text = header;
            headerClone.gameObject.SetActive(true);

            Transform inputClone = Instantiate(inputTemplate, container);
            inputClone.name = header;
            inputClone.gameObject.SetActive(true);
            
            var captured = inputClone.GetComponent<TMPro.TMP_InputField>();
            inputs.Add(captured);
        }

        Transform cloneBtn = Instantiate(btnTemplate, container);
        cloneBtn.gameObject.SetActive(true);
    }

    public void clear() 
    {
        for (int i = 0; i < container.childCount; i++)
        {
            if (i > 2)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }

    public void addRow()
    {
        //collect user input and add to db
        if(fromCollection != null)
        {
            string tableName = fromCollection.Name + fromCollection.Id.ToString();
            string[] attributes = fromCollection.Attributes.Split(", ");
            List<string> fields = new List<string>();
            
            foreach(TMP_InputField field in inputs)
            {
                if(field.text != null || field.text== "")
                {
                    fields.Add(field.text);
                }
                else
                {
                    fields.Add ("-");
                }
            }

            string rowId = CollectionManager.Instance.addData(tableName, fields.ToArray(), attributes).ToString();
            Debug.Log("added id "+rowId);
            chartViewManager.populateChart(rowId);
        }
        
    }
}
