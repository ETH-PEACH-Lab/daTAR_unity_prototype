using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AddRow : MonoBehaviour
{
    public TMP_Dropdown dataTypeDropdown;
    public TMPro.TextMeshProUGUI rowNameField;
    public ARTableManager tableManager;

    public TMP_Dropdown arg1Dropdown;
    public TMP_Dropdown arg2Dropdown;

    public TMP_Dropdown expressionDropdown;

    private Collection collection = null;
    private string[] dataTypeOptions = new string[2] {"REAL","TEXT" };
    private string[] expressionOptions = new string[3] {"ADD","MULT","DIV"};

    private string[] columnNames;


    public void commitRow()
    {
        collection = tableManager.collection;
        string rowName = rowNameField.text;
        rowName = Regex.Replace(rowName, @"\W", "_");
        //rowName = Regex.Replace(rowName, " ", "_");
        Debug.Log("add row row name " + rowName);
        string datatype = dataTypeOptions[dataTypeDropdown.value];
        

        string tableName = collection.Name + collection.Id;
        string updateQuery = getUpdateQuery(rowName);
        try
        {
            collection.Attributes = collection.Attributes + ", " + rowName;
            CollectionManager.Instance.updateCollection(collection, collection.Name);
            CollectionManager.Instance.addColumn(tableName, rowName, datatype);
            CollectionManager.Instance.updateDataTable(tableName, updateQuery);
            Debug.Log("add row succ");
            tableManager.populate(collection);

        }
        catch(Exception e)
        {
            Debug.Log("add row fail");
            Debug.Log(e.ToString());
        }
        gameObject.SetActive(false);
    }

    public void setOptions()
    {
        arg1Dropdown.ClearOptions();
        arg2Dropdown.ClearOptions();

        rowNameField.text = "";

        collection = tableManager.collection;
        columnNames = collection.Attributes.Split(", ");
        Debug.Log("set op " + collection.Attributes);

        foreach(string option in columnNames)
        {
            Debug.Log("set op " + option);
            arg1Dropdown.options.Add(new TMP_Dropdown.OptionData() { text = option });
            arg2Dropdown.options.Add(new TMP_Dropdown.OptionData() { text = option });
        }
    }
    
    private string getUpdateQuery(string rowName)
    {
        string operation = expressionOptions[expressionDropdown.value];
        string argument1 = columnNames[arg1Dropdown.value];
        string argument2 = columnNames[arg2Dropdown.value];
        string query = rowName + " = ";

        switch (operation)
        {
            case "ADD":
                query += (argument1 + " + " + argument2);
                break;
            case "MULT":
                query += (argument1 + " * " + argument2);
                break;
            case "DIV":
                query += (argument1 + " / " + argument2);
                break;
        }

        return query;
    }
    
}
