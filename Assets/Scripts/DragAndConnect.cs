using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndConnect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject panel1;
    private LineRenderer lineRenderer;
    private Vector3 offset;
    private Vector3 start = new Vector3(1,0,5);

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        //lineRenderer.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lineRenderer.enabled = true;
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        offset = screenPoint - start;
        Debug.Log("drag1 " + screenPoint);
        UpdateLinePositions(screenPoint);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        //Debug.Log("drag2 " + screenPoint);
        UpdateLinePositions(screenPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //lineRenderer.enabled = false;

        if (RectTransformUtility.RectangleContainsScreenPoint(panel1.GetComponent<RectTransform>(), eventData.position))
        {
            Debug.Log("Connected to Panel 1");
            // Implement connection logic
        }
    }

    private void UpdateLinePositions(Vector3 screenPoint)
    {
        //lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, screenPoint * 100);
        Debug.Log("drag3 " + lineRenderer.GetPosition(0) + " " + lineRenderer.GetPosition(1));
    }
}
