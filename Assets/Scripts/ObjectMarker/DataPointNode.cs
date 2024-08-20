using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataPointNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, INode
{
    public Transform parentManger;

    private IChart chartManager;

    private LineRenderer lineRenderer;
    private Vector3 offset;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        chartManager = parentManger.GetComponent<IChart>();

        NodeManger.Instance.registerNode("DataPoint", this);

    }
    public UnityEngine.Vector3 getPosition()
    {
        return transform.position;
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
        UpdateLinePositions(eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UserInputNode connectedNode = NodeManger.Instance.checkNodeHit("UserInput", lineRenderer.GetPosition(1)) as UserInputNode;

        if (connectedNode != null)
        {
            Debug.Log("hit endpoint");
            List<Dictionary<string, string>> dataPoint = new List<Dictionary<string, string>>();
            Collection selectedCollection = chartManager.collection;
            string tableName = selectedCollection.Name + selectedCollection.Id;
            dataPoint.Add(CollectionManager.Instance.getDataTableRow(tableName, chartManager.selectedRowId.ToString()));

            connectedNode.addUserInput(dataPoint, selectedCollection);
            //gameObject.GetComponent<Image>().color = new Color32(24, 164, 245, 255);

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
