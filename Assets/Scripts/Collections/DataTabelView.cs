using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DataTabelView : MonoBehaviour
{
    public Transform collectionName;
    public TMP_InputField inputField;
    public SummaryView summaryView;

    public TextAsset exampleCSV;

    private CollectionManager collectionManager;
    public Collection collection = null;

    private void Start()
    {
        collectionManager = CollectionManager.Instance;
    }
    public void populate(Collection collection)
    {
        this.collection = collection;
        collectionName.GetComponent<TMPro.TextMeshProUGUI>().text = collection.Name + " (id:" +collection.Id+" )";
        inputField.text = collection.Name;
        gameObject.SetActive(true);
    }

    public void updateName()
    {
        //call db
        //call summary view
        Collection update = new Collection { Id = collection.Id, Name = inputField.text, Entries = collection.Entries, LastMod = DateTime.Now };
        if (collectionManager.updateCollection(update) > 0)
        {
           populate(update);
           summaryView.updateRow(update);
        }
    }

    public void loadExample()
    {
        // Read the CSV content
        string[] csvLines = exampleCSV.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Parse the CSV file
        if (csvLines.Length < 2)
        {
            Debug.LogError("CSV file is empty or too short.");
            return;
        }

        // Assume the first line contains column headers
        string[] headers = csvLines[0].Split(',');
        //Debug.Log(headers[0]);

        // Create the table
        string tableName = exampleCSV.name;
        inputField.text = tableName;
        updateName();

        // Insert the data
        for (int i = 1; i < csvLines.Length; i++)
        {
            string[] fields = csvLines[i].Split(',');
            //Debug.Log(fields[0]);
        }

        Debug.Log("CSV import completed.");
    }
}
