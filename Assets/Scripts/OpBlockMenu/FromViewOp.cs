using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FromViewOp : MonoBehaviour
{
    public GameObject placeholder;
    public TMP_Dropdown dropdown;
    public OpNodeManager opNodeManager;

    private List<Collection> collectionOptions;
    private Collection selecetdCollection = null;
    public void selectCollection(int index)
    {
        //Debug.Log("selected " + index);
        if (index > 0)
        {
            placeholder.SetActive(false);
            selecetdCollection = collectionOptions[index -1];
            opNodeManager.setCollection(collectionOptions[index - 1]);
            //change color of node
        }
        else
        {
            selecetdCollection = null;
        }


    }

    

    public void resetSelection()
    {
        collectionOptions = CollectionManager.Instance.getAllCollections();
        dropdown.ClearOptions();
        dropdown.options.Add(new TMP_Dropdown.OptionData() { text = " " });
        foreach (Collection c in collectionOptions)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = c.Name });
        }

        placeholder.SetActive(true);
        dropdown.value = 0;
    }
}
