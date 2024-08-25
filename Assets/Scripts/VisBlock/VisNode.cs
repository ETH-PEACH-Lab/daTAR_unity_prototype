using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VisNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, INode
{
    public VisBlockManager blockManager;

    private LineRenderer lineRenderer;
    void Start()
    {
        NodeManger.Instance.registerNode("VisNode", this);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    public void setDataTable(List<Dictionary<string,string>> table, Collection collection)
    {
        gameObject.GetComponent<Image>().color = new Color32(24, 164, 245, 255);

        blockManager.setDynamicData(table, collection);
    }

    public void updateDataTable(List<Dictionary<string, string>> table, Collection collection)
    {
        blockManager.updateChart(table, collection);
    }

    public Vector3 getPosition()
    {
        return transform.position;
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
        TableNode connectedNode = NodeManger.Instance.checkNodeHit("TableNode", lineRenderer.GetPosition(1)) as TableNode;
        OutputDataNode connectedNode2 = NodeManger.Instance.checkNodeHit("CustomNode", lineRenderer.GetPosition(1)) as OutputDataNode;

        if (connectedNode != null)
        {
            Debug.Log("hit endpoint");
            gameObject.GetComponent<Image>().color = new Color32(24, 164, 245, 255);
            connectedNode.setVisNode(this);

            blockManager.setDynamicData(connectedNode.getDataTable(), connectedNode.GetCollection());
        }else if (connectedNode2 != null)
        {
            Debug.Log("hit endpoint");
            gameObject.GetComponent<Image>().color = new Color32(24, 164, 245, 255);
            connectedNode2.setVisNode(this);

            blockManager.setDynamicData(connectedNode2.getDataTable(), connectedNode2.GetCollection());
        }
            lineRenderer.enabled = false;
        
    }

    private void UpdateLinePositions(Vector3 screenPoint)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(screenPoint.x, screenPoint.y, screenPoint.z));
        Debug.Log("drag3 " + lineRenderer.GetPosition(0) + " " + lineRenderer.GetPosition(1));
    }
}
