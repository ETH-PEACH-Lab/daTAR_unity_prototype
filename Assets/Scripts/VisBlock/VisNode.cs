using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisNode : MonoBehaviour, INode
{
    

    void Start()
    {
        NodeManger.Instance.registerNode("VisNode", this);
    }

    public void sendInputData(string data)
    {
        gameObject.GetComponent<Image>().color = new Color32(179, 225, 251, 255);
        Debug.Log("hit TableNode " + data);
    }

   

    public Vector3 getPosition()
    {
        return transform.position;
    }
}
