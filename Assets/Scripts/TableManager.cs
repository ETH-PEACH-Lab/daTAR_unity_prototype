using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TableManager : MonoBehaviour
{
    private Transform tableContainer;
    private Transform rowTemplate;
    private int id = 0;
    private void Awake()
    {
        tableContainer = transform.Find("table_container");
        rowTemplate = tableContainer.Find("row_template");

        rowTemplate.gameObject.SetActive(false);

        //clearAll();

        string jsonString = PlayerPrefs.GetString("collectionTable");
        AllEntries allEntries = JsonUtility.FromJson<AllEntries>(jsonString);

        //addEntry("3", "2.4");

        if(allEntries != null)
        {
            foreach(Entry entry in allEntries.entryList)
            {
                renderEntry(entry);
                id = entry.id+1;
            }
        }


    }

    public void addEntry(string atr1, string atr2, string atr3)
    {
        // Create Entry
        Entry entry = new Entry{ attr1 = atr1, attr2 = atr2, attr3 = atr3, id = id };
        id++;
        renderEntry(entry);

        // Load saved Entries
        string jsonString = PlayerPrefs.GetString("collectionTable");
        AllEntries allEntries = JsonUtility.FromJson<AllEntries>(jsonString);

        if (allEntries == null)
        {
            // There's no stored table, initialize
            allEntries = new AllEntries()
            {
                entryList = new List<Entry>()
            };
        }

        // Add new entry to list
        allEntries.entryList.Add(entry);

        // Save updated list
        string json = JsonUtility.ToJson(allEntries);
        PlayerPrefs.SetString("collectionTable", json);
        PlayerPrefs.Save();
    }

    private void renderEntry(Entry entry)
    {
        Transform newRow = Instantiate(rowTemplate, tableContainer);
        newRow.gameObject.SetActive(true);
        newRow.Find("id_text").GetComponent<TMPro.TextMeshProUGUI>().text = entry.id.ToString();
        newRow.Find("attr01_text").GetComponent<TMPro.TextMeshProUGUI>().text = entry.attr1;
        newRow.Find("attr02_text").GetComponent<TMPro.TextMeshProUGUI>().text = entry.attr2;
        newRow.Find("attr03_text").GetComponent<TMPro.TextMeshProUGUI>().text = entry.attr3;
    }

    public void clearAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static List<string> getColumn(string columnName)
    {
        string jsonString = PlayerPrefs.GetString("collectionTable");
        AllEntries allEntries = JsonUtility.FromJson<AllEntries>(jsonString);
        List<string> result = new List<string>();
        if (allEntries == null ) { return result; }

        foreach ( Entry entry in allEntries.entryList)
        {
            switch(columnName)
            {
                case "attr1": result.Add(entry.attr1); 
                    break;
                case "attr2": result.Add(entry.attr2); 
                    break;
                case "attr3": result.Add(entry.attr3);
                    break;
            }
        }
        return result;
    }

    

    public class AllEntries
    {
        public List<Entry> entryList;
    }

    /*
     * Represents a single High score entry
     * */
    [System.Serializable]
    public class Entry
    {
        public string attr1;
        public string attr2;
        public string attr3;
        public int id;
    }
}
