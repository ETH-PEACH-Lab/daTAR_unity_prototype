using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryView : MonoBehaviour
{
    public Transform rowTemplate;
    public Transform container;
    public DataTabelView dataTabelView;

    private List<Collection> collections;
    void Awake()
    {
        Debug.Log("started overview");
        rowTemplate.gameObject.SetActive(false);
        collections = CollectionManager.Instance.getAllCollections();
        foreach (Collection coll in collections)
        {
            renderRow(coll);
        }
    }

    public void addRow()
    {
        Collection newCollection = CollectionManager.Instance.addCollection("Untitled");
        if (newCollection != null)
        {
            renderRow(newCollection);
            dataTabelView.populate(newCollection);
        }
    }

    public void updateRow(Collection collection)
    {
        Transform toUpdate = container.Find(collection.Id.ToString());
        if (toUpdate != null)
        {
            toUpdate.Find("text_container").Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = collection.Name;
            toUpdate.Find("text_container").Find("mod").GetComponent<TMPro.TextMeshProUGUI>().text = collection.LastMod.ToString().Split(' ')[0];

            Button b = toUpdate.GetComponent<Button>();
            Collection captured = collection;
            Debug.Log("update sumview " + collection.Name);
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => dataTabelView.populate(captured));
        }
    }

    public void removeRow()
    {
        Collection collection = dataTabelView.collection;
        if (collection != null)
        {
            if (CollectionManager.Instance.removeCollection(collection) > 0)
            {
                Transform toDelete = container.Find(collection.Id.ToString());
                if (toDelete != null)
                {
                    Destroy(toDelete.gameObject);
                }
                dataTabelView.gameObject.SetActive(false);
            }

        }

    }

    private void renderRow(Collection collection)
    {
        Transform clone = Instantiate(rowTemplate, container);
        clone.gameObject.SetActive(true);
        clone.Find("text_container").Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = collection.Name;
        clone.Find("text_container").Find("entries").GetComponent<TMPro.TextMeshProUGUI>().text = collection.Entries.ToString();
        clone.Find("text_container").Find("mod").GetComponent<TMPro.TextMeshProUGUI>().text = collection.LastMod.ToString().Split(' ')[0];
        clone.name = collection.Id.ToString();

        Button b = clone.GetComponent<Button>();
        Collection captured = collection;
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => dataTabelView.populate(captured));
    }

    //use OnEnable() for detecting set active event
}






