using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NodeManger
{
    

    private static NodeManger instance = null;
    private static readonly object padlock = new object();

    private Dictionary<string, List<INode>> registeredNodes;

    NodeManger() {
        registeredNodes = new Dictionary<string, List<INode>>();
    }
    public static NodeManger Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new NodeManger();
                }
                return instance;
            }
        }
    }

    public void registerNode(string type, INode node)
    {
        if (!registeredNodes.ContainsKey(type))
        {
            registeredNodes[type] = new List<INode>();

        }
        registeredNodes[type].Add(node);
    }

    public INode checkNodeHit(string type, Vector3 pos)
    {
        if(registeredNodes.ContainsKey(type))
        {
            foreach (INode node in registeredNodes[type])
            {
                float distance = Vector3.Distance(pos, node.getPosition());
                Debug.Log("hit dist " + distance);
                if (distance < 0.01f)
                {
                    Debug.Log("hit node");
                    return node;
                }
            }
        }
        return null;
    }

    
}

