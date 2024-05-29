using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ExtractCollision : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool trigger = false;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("col11 " + other.name);
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material.color = Color.blue;
        //transform.Find("cube1").gameObject.SetActive(true);
        //transform.Find("cube2").gameObject.SetActive(true);
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1, other.transform.position);
        trigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //MeshRenderer mr = GetComponent<MeshRenderer>();
        //mr.material.color = Color.gray;
    }

    private void Update()
    {
        if (trigger)
        {
            lineRenderer.SetPosition(0,transform.position);
        }
    }
}
