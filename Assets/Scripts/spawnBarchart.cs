using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static TableManager;

public class spawnBarchart : MonoBehaviour
{
    private Transform unitObject;
    private Transform labelTemplate;
    private Transform sphereTemplate;
    private int collectionSize = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        unitObject = transform.Find("unit_template");
        unitObject.gameObject.SetActive(false);
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
            addLabels(x,0f);
            x++;
            foreach (Entry entry in allEntries.entryList)
            {
                float z = 0f;
                float h = 0f;
                
                if (float.TryParse(entry.atr1, out h))
                {
                    initBar(h, x, z);

                    if(float.TryParse(entry.atr2, out h))
                    {
                        z += 6f;
                        initBar(h, x, z);
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

        Transform clone = Instantiate(unitObject, transform);
        clone.gameObject.SetActive(true);
        clone.transform.localScale = scale;
        clone.transform.localPosition = pos;

    }

    

    private void addLabels(float x, float z)
    {
        Vector3 pos = new Vector3(0f, 0f, 6f);

        Transform label = Instantiate(labelTemplate, transform);
        label.gameObject.SetActive(true);
        Debug.Log(label.gameObject.GetComponent<TextMeshPro>());
        label.GetComponent<TextMeshPro>().text = "attr 1";

        Transform selectionSphere = Instantiate(sphereTemplate, transform);
        selectionSphere.gameObject.SetActive(true);

        Transform label2 = Instantiate(labelTemplate, transform);
        label2.gameObject.SetActive(true);
        //Debug.Log(label.gameObject.GetComponent<TextMeshPro>());
        label2.GetComponent<TextMeshPro>().text = "attr 2";
        label2.transform.localPosition += pos;

        Transform selectionSphere2 = Instantiate(sphereTemplate, transform);
        selectionSphere2.gameObject.SetActive(true);
        selectionSphere2.transform.localPosition += pos;

        //label.gameObject.AddComponent<TMPro.TextMeshProUGUI>();
        //label.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "attr 1";

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
