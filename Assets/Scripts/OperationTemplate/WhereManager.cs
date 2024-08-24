using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhereManager : MonoBehaviour, IOpManager
{
    public ColumnNode connectedNode { get; set; }

    public string conditon = "";

    public void updateCondition()
    {
        if (connectedNode != null && conditon != "")
        {
            connectedNode.setOperation("WHERE",conditon);
        }
    }
}
