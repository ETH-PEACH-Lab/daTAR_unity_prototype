using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColumnNode : MonoBehaviour, INode, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private LineRenderer lineRenderer;
    public ARTableManager tableManager;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        NodeManger.Instance.registerNode("ColumnNode", this);
    }

    public UnityEngine.Vector3 getPosition()
    {
        return transform.position;
    }

    //will be called from an OpNode object to send their data operation string
    public void setOperation(string data)
    {
        
        gameObject.GetComponent<Image>().color = new Color32(179, 225, 251, 255);
        tableManager.executeOperation(data, transform.parent.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lineRenderer.enabled = true;
        UpdateLinePositions(eventData.pointerCurrentRaycast.worldPosition);
        //Debug.Log("drag1 " + lineRenderer.GetPosition(0) + " " + lineRenderer.GetPosition(1));
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateLinePositions(eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OpNode connectedNode = NodeManger.Instance.checkNodeHit("OpNode", lineRenderer.GetPosition(1)) as OpNode;

        if (connectedNode != null)
        {
            Debug.Log("hit endpoint " + gameObject.name);
            gameObject.GetComponent<Image>().color = new Color32(179, 225, 251, 255);
            tableManager.executeOperation(connectedNode.operation, transform.parent.name);
            connectedNode.setActive();
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
