using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutputDataNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, INode
{
    public CustomBlockManager blockManager;

    private LineRenderer lineRenderer;
    private Vector3 offset;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        NodeManger.Instance.registerNode("CustomNode", this);
    }
    public UnityEngine.Vector3 getPosition()
    {
        return transform.position;
    }
    public List<Dictionary<string, string>> getDataTable()
    {
        gameObject.GetComponent<Image>().color = new Color32(24, 164, 245, 255);
        return blockManager.getOutData();
    }

    public Collection GetCollection()
    {
        return blockManager.getCollection();
    }

    public void setVisNode(VisNode visNode)
    {
        blockManager.connectedVisNode = visNode;
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
            blockManager.updateVisBlock();
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
        lineRenderer.SetPosition(1, new Vector3(screenPoint.x, screenPoint.y, screenPoint.z));
        Debug.Log("drag3 " + lineRenderer.GetPosition(0) + " " + lineRenderer.GetPosition(1));
    }
}
