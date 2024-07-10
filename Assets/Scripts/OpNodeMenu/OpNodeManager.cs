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
            arTable.GetComponent<ARTableManager>().populate(collection);

            addBtn.gameObject.SetActive(false);
            menu.gameObject.SetActive(false);
        }
        Debug.Log("selected " + this.collection.Name);
    }
    private void init()
    {
        fromView.resetSelection();
    }
}
