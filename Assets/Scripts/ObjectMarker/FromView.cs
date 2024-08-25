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
    public ObjectManager objectManager;
    public Transform placeholder;

    private CollectionManager collectionManager;
    private List<Collection> collectionOptions;
    void Start()
    {
        //resetSelection();
        
    }

    public void selectCollection(int index)
    {
        Debug.Log("selected " + index);
        if(index > 0)
        {
           placeholder.gameObject.SetActive(false);
           objectManager.setCollection(collectionOptions[index - 1]);
        }
        
        //Debug.Log("selected "+ index);  
    }

    public void resetSelection()
    {
        Debug.Log("from view00");
        collectionManager = CollectionManager.Instance;
        collectionOptions = collectionManager.getAllCollections();
        dropdown.ClearOptions();
        dropdown.options.Add(new TMP_Dropdown.OptionData() { text = " " });
        foreach (Collection c in collectionOptions)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = c.Name });
            Debug.Log("from view 11");
        }

        placeholder.gameObject.SetActive(true);
        dropdown.value = 0;
    }

}
