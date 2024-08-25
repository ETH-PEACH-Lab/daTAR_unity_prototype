using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TableNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, INode
{
    public ARTableManager tableManager;

   private LineRenderer lineRenderer;
    private Vector3 offset;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        NodeManger.Instance.registerNode("TableNode", this);
    }
    public UnityEngine.Vector3 getPosition()
    {
        return transform.position;
    }
    public List<Dictionary<string, string>> getDataTable()
    {
        gameObject.GetComponent<Image>().color = new Color32(24, 164, 245, 255);
        return tableManager.table;
    }

    public Collection GetCollection()
    {
        return tableManager.collection;
    }

    public void setVisNode(VisNode visNode)
    {
        tableManager.connectedVisNode = visNode;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        lineRenderer.enabled = true;
        //lineRenderer = LineManager.Instance.startNewLine();


        //Vector3 screenPoint = Camera.main.ScreenToWorldPoint(eventData.position);

        UpdateLinePositions(eventData.pointerCurrentRaycast.worldPosition);
        //Debug.Log("drag1 " + lineRenderer.GetPosition(0) + " " + lineRenderer.GetPosition(1));
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        Debug.Log("event pos " + eventData.pressPosition);
        Vector3 pos = Input.mousePosition;
        //Debug.Log("drag2 " + screenPoint); 
        UpdateLinePositions(eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        VisNode connectedNode = NodeManger.Instance.checkNodeHit("VisNode", lineRenderer.GetPosition(1)) as VisNode;

        if (connectedNode != null)
        {
            Debug.Log("hit endpoint");
            gameObject.GetComponent<Image>().color = new Color32(24, 164, 245, 255);
            setVisNode(connectedNode);
            tableManager.initVisBlock();
            //connectedNode.setDataTable(tableManager.table, tableManager.collection);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void UpdateLinePositions(Vector3 screenPoint)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(screenPoint.x , screenPoint.y , screenPoint.z));
        Debug.Log("drag3 " + lineRenderer.GetPosition(0) + " " + lineRenderer.GetPosition(1));
    }
}
