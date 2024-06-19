using System.Collections;
using System.Collections.Generic;
using TMPro;
//using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FromView : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public ChartViewManager chartViewManager;

    private CollectionManager collectionManager;
    private List<Collection> collectionOptions;
    void Start()
    {
        collectionManager = CollectionManager.Instance;
        collectionOptions = collectionManager.getAllCollections();
        foreach (Collection c in collectionOptions)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(){ text = c.Name});
        }
        
    }

    public void selectCollection(int index)
    {
        chartViewManager.setCollection(collectionOptions[index]);
        //Debug.Log("selected "+ index);  
    }

}
