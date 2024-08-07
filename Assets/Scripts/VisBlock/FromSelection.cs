using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FromSelection : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Transform placeholder;
    public VisBlockManager visBlockManager;

    private List<Collection> collectionOptions;

    public void selectCollection(int index)
    {
        Debug.Log("selected " + index);
        if (index > 0)
        {
            placeholder.gameObject.SetActive(false);
            visBlockManager.setCollection(collectionOptions[index - 1]);
            //change color of node
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

        placeholder.gameObject.SetActive(true);
        dropdown.value = 0;
    }
}
