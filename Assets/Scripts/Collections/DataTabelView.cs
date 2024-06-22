using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using System.Net.Http.Headers;
using SimpleFileBrowser;
using System.IO;

public class DataTabelView : MonoBehaviour
{
    public Transform collectionName;
    public TMP_InputField inputField;
    public SummaryView summaryView;

    public Transform cellTemplate;
    public Transform container;

    public TextAsset exampleCSV;

    private CollectionManager collectionManager;
    public Collection collection = null;

    private void Start()
    {
        collectionManager = CollectionManager.Instance;
        cellTemplate.gameObject.SetActive(false);
        Debug.Log("staring datatableview");
    }
    public void populate(Collection collection)
    {
        this.collection = collection;
        collectionName.GetComponent<TMPro.TextMeshProUGUI>().text = collection.Name + " (id:" +collection.Id+" )";
        inputField.text = collection.Name;
        gameObject.SetActive(true);
        for (int i = 0; i < container.childCount; i++)
        {
            if(i> 0)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
        collectionManager = CollectionManager.Instance;
        Debug.Log("checking" + collection.Name);
        string[] headers = collection.Attributes.Split(", ");
        if(headers.Length > 0)
        {
            Debug.Log(headers.Length+"l2");
            container.GetComponent<GridLayoutGroup>().constraintCount = headers.Length;
            renderRow(headers);
            //collectionManager.getDataTable(collection.Name);
            string[] fields = collectionManager.getDataTable(collection.Name);
            if(fields != null)
            {
                renderRow(fields);
            }
            
        }
    }

    public void updateName()
    {
        //call db
        //call summary view
        Collection update = new Collection { Id = collection.Id, Name = inputField.text, Entries = collection.Entries, LastMod = DateTime.Now, Attributes = collection.Attributes };
        //Debug.Log("before update");
        if (collectionManager.updateCollection(update, collection.Name) > 0)
        {
           populate(update);
           summaryView.updateRow(update);
            //Debug.Log("after up" + update.Name);
        }
    }

    private void loadData(string data, string fileName)
    {
        // Read the CSV content
        string[] csvLines = data.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Parse the CSV file
        if (csvLines.Length < 2)
        {
            Debug.LogError("CSV file is empty or too short.");
            return;
        }
        // Create the table
        string tableName = fileName;
        inputField.text = tableName;
        

        // Assume the first line contains column headers
        string[] headers = csvLines[0].Split(',');
        Debug.Log(headers.Length+"lll");
        //container.GetComponent<GridLayoutGroup>().constraintCount = headers.Length;
        if(collectionManager.createDataTable(tableName) > 0)
        {
            //renderRow(headers);
            // Insert the data
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] fields = csvLines[i].Split(',');
                if (collectionManager.addData(tableName, fields) > 0)
                {
                    //renderRow(fields);
                }


            }
            collection.Attributes = string.Join(", ", headers);
            
        }
        updateName();



        Debug.Log("CSV import completed.");
    }

    private void renderRow(string[] data)
    {
        foreach (string field in data)
        {
            Transform clone = Instantiate(cellTemplate, container);
            clone.gameObject.SetActive(true);
            clone.Find("data").GetComponent<TMPro.TextMeshProUGUI>().text = field;
        }
        
    }

    public void OpenFilePicker()
    {
        // Show a file picker dialog
        FileBrowser.SetFilters(true, new FileBrowser.Filter("CSV Files", ".csv"));
        FileBrowser.SetDefaultFilter(".csv");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        // Coroutine to handle the file picking
        StartCoroutine(ShowFilePicker());
    }

    private IEnumerator ShowFilePicker()
    {
        // Show the file picker and wait for a response
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Select Files", "Load");
        Debug.Log("loaded " + FileBrowser.Success);
        if (FileBrowser.Success)
        {
            // Get the path of the selected file
            //Debug.Log("succ " + FileBrowser.Result[0]);
            string path = FileBrowser.Result[0];
            Debug.Log("filepath"+ path);
            //string fileName = Path.GetFileName(path).Split('.')[0];
            string fileName = "";
            if (path.StartsWith("content:"))
            {
                string[] s = path.Split('F');
                fileName = s[s.Length - 1].Split(".")[0];
            }
            else
            {
                fileName = Path.GetFileName(path).Split(".")[0];
            }
            
            Debug.Log("filename: " + fileName);

            string fileContent = "";

            fileContent = FileBrowserHelpers.ReadTextFromFile(path);
            

            Debug.Log("fileconent "+fileContent);
            loadData(fileContent, fileName);
        }
    }

   

}

