using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("col2 "+ other.name);
        Debug.Log("col3 " + other.gameObject.name);
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material.color = Color.blue;
        //transform.Find("cube1").gameObject.SetActive(true);
        //transform.Find("cube2").gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material.color = Color.gray;
    }

}

