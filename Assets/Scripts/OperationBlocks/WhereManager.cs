using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhereManager : MonoBehaviour, IOpManager
{
    //node the operation block is connected to, that is a node corresponding to a data attribute in the AR data table block
    public ColumnNode connectedNode { get; set; }

    public string conditon = "";

    /// <summary>
    /// sets sql where condition based on user input, modified through UI elements
    /// </summary>
    public void updateCondition()
    {
        if (connectedNode != null && conditon != "")
        {
            connectedNode.setOperation("WHERE",conditon);
        }
    }
}
