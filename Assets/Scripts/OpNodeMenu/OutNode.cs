using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
   private LineRenderer lineRenderer;
    private Vector3 offset;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
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
        if(LineManager.Instance.checkLine(lineRenderer) == 1)
        {
            Debug.Log("hit endpoint");
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
