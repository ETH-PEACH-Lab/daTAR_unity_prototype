using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InNode : MonoBehaviour, INode
{
    

    void Start()
    {
        LineManager.Instance.registerInNode(this);
    }

    public void setInputData(string data)
    {
        Debug.Log("hit endpoint 2");
    }

   

    public Vector3 getPosition()
    {
        return transform.position;
    }
}
