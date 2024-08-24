using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserInputNode : MonoBehaviour, INode
{
    public Transform manager;
    private IBlockManager blockManager;

    void Start()
    {
        blockManager = manager.GetComponent<IBlockManager>();
        NodeManger.Instance.registerNode("UserInput", this);

    }
    public UnityEngine.Vector3 getPosition()
    {
        return transform.position;
    }

    public void addUserInput(List<Dictionary<string, string>> dataPoints, Collection fromCollection)
    {
       blockManager.addUserInput(dataPoints, fromCollection);
    }
    
    
}
