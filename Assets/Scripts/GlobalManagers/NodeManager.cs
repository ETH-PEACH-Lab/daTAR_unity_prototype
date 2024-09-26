using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NodeManger
{
    

    private static NodeManger instance = null;
    private static readonly object padlock = new object();
    //data structure holding all nodes subscribed to the event of collison (connecting per drag and drop)
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

    /// <summary>
    /// adds node to the dictionary data structure with node type as key
    /// </summary>
    /// <param name="type">node type should be "CustomNode","UserInput","DataPoint","OpNode","ColumnNode","TableNode" or "VisNode"</param>
    /// <param name="node">node to add to the data structure</param>
    public void registerNode(string type, INode node)
    {
        if (!registeredNodes.ContainsKey(type))
        {
            registeredNodes[type] = new List<INode>();

        }
        registeredNodes[type].Add(node);
    }

    /// <summary>
    /// checks if the given positon collides with any of the registered nodes in the data structure matching the given type
    /// </summary>
    /// <param name="type">type of node that should be checked for collision</param>
    /// <param name="pos">positon in 3d space to check agains any node postion with matching type</param>
    /// <returns>first node with given type and within a collison range of the given postion, null if no node found</returns>
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

