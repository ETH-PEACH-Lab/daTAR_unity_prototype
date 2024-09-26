using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderbyManager : MonoBehaviour, IOpManager
{
    //node the operation block is connected to, that is a node corresponding to a data attribute in the AR data table block
    public ColumnNode connectedNode {  get; set; }
}
