using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TableManager;

public class GroupByCollision : MonoBehaviour
{
    private string[] labelNames = new string[3] { "attr1", "attr2", "attr3" };
    public string selectedLabel = "";
    public List<string> columnData = new List<string>();
    //[SerializeField] private TableManager tableManager;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("colgb " + other.name);
        if (Array.Exists(labelNames, element => element == other.name))
        {
            selectedLabel = other.name;
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.material.color = Color.blue;

            columnData = TableManager.getColumn(selectedLabel);
            columnData = columnData.Distinct().ToList();
            //Debug.Log("colgb " + string.Join(", ", columnData));
     
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == selectedLabel)
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.material.color = Color.gray;
        }
        
    }
}
