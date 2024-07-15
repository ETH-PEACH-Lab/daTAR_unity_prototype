using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LineManager : MonoBehaviour
{
    public Transform lineTemplate;
    public Transform container;

    private static LineManager instance = null;
    private static readonly object padlock = new object();

    private List<INode> inNodes;
    public static LineManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    Debug.Log("fail");
                }
                return instance;
            }
        }
    }

    private void Start()
    {
        lineTemplate.gameObject.SetActive(false);
        inNodes = new List<INode>();
        instance = this;
    }

    public LineRenderer startNewLine()
    {
        //maybe not needed 
        Debug.Log("line0");
        Transform clone = Instantiate(lineTemplate, container);
        Debug.Log("line1");
        clone.gameObject.SetActive(true);
        Debug.Log("line2");
        return clone.GetComponent<LineRenderer>();

    }

    public int checkLine(LineRenderer line)
    {
        //return 1 if a potential end node is hit

        return 0;
    }

    public void registerInNode(INode node)
    {
        inNodes.Add(node);
    }
}
