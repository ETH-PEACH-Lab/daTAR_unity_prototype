using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpNodeManager : MonoBehaviour
{
    public FromViewOp fromView;
    public GameObject tableTemplate;
    public Transform addBtn;
    public Transform menu;

    private Collection collection = null;
    private GameObject arTable = null;
    void Start()
    {
       init();
    }

    public void setCollection(Collection collection)
    {
        this.collection = collection;
        if(arTable == null)
        {
            arTable = Instantiate(tableTemplate, transform);
            arTable.GetComponent<ARTableManager>().opNodeManager = this;
            arTable.GetComponent<ARTableManager>().populate(collection);

            addBtn.gameObject.SetActive(false);
            menu.gameObject.SetActive(false);
        }
        else
        {
            arTable.GetComponent<ARTableManager>().populate(collection);
            addBtn.gameObject.SetActive(false);
            menu.gameObject.SetActive(false);
            arTable.SetActive(true);
        }
        Debug.Log("selected " + this.collection.Name);
    }

    public void edit()
    {
        fromView.resetSelection();
        menu.gameObject.SetActive(true);
        addBtn.gameObject .SetActive(false);
    }
    private void init()
    {
        fromView.resetSelection();
        addBtn.gameObject.SetActive(true);
        menu.gameObject.SetActive(false);
    }
}
