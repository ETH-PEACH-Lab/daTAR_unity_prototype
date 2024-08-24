using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpNode : MonoBehaviour, INode, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string operation;
    public string condition = "";
    public Transform manager;

    private LineRenderer lineRenderer;
    private IOpManager opManager;
    void Start()
    {
        NodeManger.Instance.registerNode("OpNode",this);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        opManager = manager.GetComponent<IOpManager>();
    }

    public UnityEngine.Vector3 getPosition()
    {
        return transform.position;
    }

    //will be called from a ColumnNode Object to indicate that the nodes are connected
    public void setActive()
    {
        gameObject.GetComponent<Image>().color = new Color32(24, 164, 245, 255);
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
        ColumnNode connectedNode = NodeManger.Instance.checkNodeHit("ColumnNode", lineRenderer.GetPosition(1)) as ColumnNode;

        if (connectedNode != null)
        {
            gameObject.GetComponent<Image>().color = new Color32(24, 164, 245, 255);
            connectedNode.clearSelectedNodes();
            connectedNode.setOperation(operation, condition);
            opManager.connectedNode = connectedNode;
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

    public void clearConnection()
    {
        lineRenderer.enabled = false;
    }
}
