using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class AddRowView : MonoBehaviour
{
    public Transform attrTemplate;
    public Transform inputTemplate;
    public Transform container;

    public DataTabelView dataTableView;

    private Collection collection;
    private List<TMPro.TMP_InputField> inputs;
    
    public void populate(Collection col)
    {
        clear();
        inputs = new List<TMPro.TMP_InputField>();
        gameObject.SetActive(true);
        collection = col;
        string[] attributes = collection.Attributes.Split(", ");
        foreach (string attribute in attributes)
        {
            Transform cloneAttr = Instantiate(attrTemplate, container);
            cloneAttr.GetComponent< TMPro.TextMeshProUGUI > ().text = attribute;
            cloneAttr.gameObject.SetActive(true);

            Transform cloneInput = Instantiate(inputTemplate, container);
            cloneInput.gameObject.SetActive(true);

            var captured = cloneInput.GetComponent<TMPro.TMP_InputField>();
            inputs.Add(captured);
        }
    }
    private void clear()
    {
        for (int i = 0; i < container.childCount; i++)
        {
            if (i > 1)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }
    public void addRow()
    {
        string tableName = collection.Name + collection.Id.ToString();
        string[] attributes = collection.Attributes.Split(", ");
        List<string> fields = new List<string>();

        foreach (TMP_InputField field in inputs)
        {
            if (field.text != null || field.text == "")
            {
                fields.Add(field.text);
            }
            else
            {
                fields.Add("none");
            }
        }

        string rowId = CollectionManager.Instance.addData(tableName, fields.ToArray(), attributes).ToString();
        dataTableView.populate(collection);
        Debug.Log("added id " + rowId);
        gameObject.SetActive(false);
    }
}
