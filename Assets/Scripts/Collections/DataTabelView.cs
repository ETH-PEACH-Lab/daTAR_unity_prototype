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
using UnityEngine.Windows;
using Unity.VisualScripting;
using System.Text.RegularExpressions;

public class DataTabelView : MonoBehaviour
{
    public Transform collectionName;
    public TMP_InputField inputField;
    public SummaryView summaryView;

    public Transform cellTemplate;
    public Transform container;
    public Transform addRowBtn;

    public TextAsset exampleCSV;

    public Collection collection = null;

    public AddRowView addRowView;

    private void Start()
    {
        cellTemplate.gameObject.SetActive(false);
        Debug.Log("staring datatableview");
    }
    public void populate(Collection col)
    {
        collection = CollectionManager.Instance.getCollection(col.Name); // get newest instance workaround for handling updates
        collectionName.GetComponent<TMPro.TextMeshProUGUI>().text = collection.Name; // + " (id:" +collection.Id+" )";
        inputField.text = collection.Name;
        gameObject.SetActive(true);
        for (int i = 0; i < container.childCount; i++)
        {
            if (i > 1)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }

        Debug.Log("checking" + collection.Name);
        string[] headers = collection.Attributes.Split(", ");
        if (headers.Length > 0)
        {
            container.GetComponent<GridLayoutGroup>().constraintCount = headers.Length;
            renderRow(headers);
            //collectionManager.getDataTable(collection.Name);
            string tableName = collection.Name + collection.Id;
            List<Dictionary<string,string>> table = CollectionManager.Instance.getDataTable(tableName);
            if (table != null)
            {
                foreach(Dictionary<string,string> row in table)
                {
                    List<string> fields = new List<string>();
                    foreach (KeyValuePair<string, string> cell in row)
                    {
                        if(cell.Key != "id")
                        {
                            fields.Add(cell.Value);
                        }
                    }
                    renderRow(fields.ToArray());   
                }

                //emptyRow(headers.Length);
                //change btn for adding to directly enable data cell edit like in a traditional spread sheet
                //activateAddRow();
            }

        }
    }

    public void updateName()
    {
        //call db
        //call summary view
        Collection update = new Collection { Id = collection.Id, Name = inputField.text, Entries = collection.Entries, LastMod = DateTime.Now, Attributes = collection.Attributes };
        //Debug.Log("before update");
        string oldName = collection.Name + collection.Id;
        if (CollectionManager.Instance.updateCollection(update, oldName) > 0)
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
        //remove non word character
        fileName = Regex.Replace(fileName, @"\W", "_");
        Debug.Log(fileName);
        string tableName = fileName + collection.Id;
        inputField.text = fileName;


        // Assume the first line contains column headers
        string[] headers = csvLines[0].Split(',');
        for (int i = 0; i < headers.Length; i++)
        {
            //clean up the attributes name for sql to be readable
            string clean = new string(headers[i].Where(c => !char.IsControl(c)).ToArray());
            headers[i] = clean;
            headers[i] = Regex.Replace(headers[i], @"\W", "_");
            //Debug.Log(headers[i]);
        }
        // Assume the second line contains the type
        string[] types = new string[headers.Length];
        for (int i = 0; i< types.Length; i++)
        {
            if (csvLines.Length > 1)
            {
                if (float.TryParse(csvLines[1].Split(',')[i], out float value))
                {
                    types[i] = "REAL";
                }
                else
                {
                    types[i] = "TEXT";
                }
            }
            else
            {
                types[i] = "TEXT";
            }
        }
        
        

        if (CollectionManager.Instance.createDataTable(tableName, headers, types) >= 0)
        {
            // Insert the data
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] fields = csvLines[i].Split(',');
                
                if (CollectionManager.Instance.addData(tableName, fields, headers) < 0)
                {
                    Debug.Log("error adding data");
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

    private void emptyRow(int cellCount)
    {
        for(int i = 0; i < cellCount; i++)
        {
            Transform clone = Instantiate(cellTemplate, container);
            clone.gameObject.SetActive(true);
            clone.Find("data").GetComponent<TMPro.TextMeshProUGUI>().text = "";
            Button b = clone.AddComponent<Button>();
            b.onClick.AddListener(() => Debug.Log("clicked"));
        }
    }

    private void activateAddRow()
    {
        Transform clone = Instantiate(addRowBtn, container);
        clone.gameObject.SetActive(true);
        Button b = clone.GetComponent<Button>();
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => addRowView.populate(collection));
        Debug.Log("activate add row");
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
            Debug.Log("filepath" + path);
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


            Debug.Log("fileconent " + fileContent);
            loadData(fileContent, fileName);
        }
    }



}

