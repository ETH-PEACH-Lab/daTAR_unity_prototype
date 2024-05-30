using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static TableManager;

public class spawnBarchart : MonoBehaviour
{
    private Transform barTemplate;
    private Transform labelTemplate;
    private Transform sphereTemplate;
    private Transform stringTemplate;
    private int collectionSize = 0;
    private float rowSpacing = 6f;
    
    // Start is called before the first frame update
    void Start()
    {
        barTemplate = transform.Find("bar_template");
        barTemplate.gameObject.SetActive(false);
        stringTemplate = transform.Find("string_template");
        stringTemplate.gameObject.SetActive(false);
        labelTemplate = transform.Find("label_template");
        labelTemplate.gameObject.SetActive(false);
        sphereTemplate = transform.Find("sphere_template");
        sphereTemplate.gameObject.SetActive(false);
        loadBarchart();   
    }


    private void loadBarchart()
    {
        // Load saved Entries
        //should not use classes from TableManger make db service to serve data to table view and 3d visualization
        string jsonString = PlayerPrefs.GetString("collectionTable");
        AllEntries allEntries = JsonUtility.FromJson<AllEntries>(jsonString);
       
        if(allEntries != null)
        {
            float x = -1f;
            collectionSize = allEntries.entryList.Count;
            addLabels();
            x++;
            foreach (Entry entry in allEntries.entryList)
            {
                float z = -6f;
                float h = 0f;
                
                if (float.TryParse(entry.attr1, out h))
                {
                    initBar(h, x, z);

                    if(float.TryParse(entry.attr2, out h))
                    {
                        z += rowSpacing;
                        initBar(h, x, z);

                        z += rowSpacing;
                        initStringValue(entry.attr3,x,z);
                    }
                    

                }

                x+=2f;
            }
        }
        
    }

    private void initBar(float h, float x, float z)
    {
       
        Vector3 pos = new Vector3(x, h/2, z);
        Vector3 scale = new Vector3(1f, h, 1f);

        Transform clone = Instantiate(barTemplate, transform);
        clone.gameObject.SetActive(true);
        clone.transform.localScale = scale;
        clone.transform.localPosition = pos;

    }

    private void initStringValue(string str,float x, float z)
    {
        Vector3 pos = new Vector3(x, 0, z);
        Transform clone = Instantiate(stringTemplate, transform);
        clone.gameObject.SetActive(true);
        clone.transform .localPosition = pos;
        clone.GetChild(1).transform.GetComponent<TextMeshPro>().text = str;
    }

    

    private void addLabels()
    {
        string[] labelNames = new string[3] {"attr1","attr2","attr3" };

        for(int i = 0; i<labelNames.Length;i++)
        {
            Vector3 pos = new Vector3(0f, 0f, rowSpacing*i);
            Transform label = Instantiate(labelTemplate, transform);
            label.gameObject.SetActive(true);
            //Debug.Log(label.gameObject.GetComponent<TextMeshPro>());
            label.GetComponent<TextMeshPro>().text = labelNames[i];
            label.transform.localPosition += pos;

            Transform selectionSphere = Instantiate(sphereTemplate, transform);
            selectionSphere.gameObject.SetActive(true);
            selectionSphere.transform.localPosition += pos;
            selectionSphere.name = labelNames[i];
        }

    }

    public void addEntry()
    {

        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (i > 2) //leave out templates
            {
                GameObject.Destroy(child.gameObject);
            }
            
        }
        loadBarchart();
    }
}
