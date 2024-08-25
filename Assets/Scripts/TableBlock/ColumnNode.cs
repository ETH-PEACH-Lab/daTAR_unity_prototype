using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColumnNode : MonoBehaviour, INode, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private LineRenderer lineRenderer;

    public ARTableManager tableManager;
    public Transform parent;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        NodeManger.Instance.registerNode("ColumnNode", this);
    }

    public UnityEngine.Vector3 getPosition()
    {
        //Debug.Log("colNode 0" + transform);
        return transform.position;
    }

    //will be called from an OpNode object to send their data operation string
    public void setOperation(string operation, string condition)
    {
        
        gameObject.GetComponent<Image>().color = new Color32(24, 164, 245, 255);
        tableManager.executeOperation(operation, condition, transform.parent.name);
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
            clearSelectedNodes();
            connectedNode.clearConnection();
            gameObject.GetComponent<Image>().color = new Color32(24, 164, 245, 255);
            lineRenderer.enabled = true;
            tableManager.executeOperation(connectedNode.operation, connectedNode.condition, transform.parent.name);
            connectedNode.setActive();
        }
            lineRenderer.enabled = false;
        
    }

    private void UpdateLinePositions(Vector3 screenPoint)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(screenPoint.x, screenPoint.y, screenPoint.z));
        //Debug.Log("drag3 " + lineRenderer.GetPosition(0) + " " + lineRenderer.GetPosition(1));
    }

    public void clearSelectedNodes()
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            Image node = parent.GetChild(i).GetChild(1).GetComponent<Image>();
            LineRenderer line = parent.GetChild(i).GetChild(1).GetComponent<LineRenderer>();
            node.color = new Color32(226,226,226,255);
            line.enabled = false;
        }
    }
}
