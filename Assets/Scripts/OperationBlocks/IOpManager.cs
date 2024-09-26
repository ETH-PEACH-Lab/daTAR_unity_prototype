using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOpManager //implemented by OrderbyManager and WhereManager 
{
    //node the operation block is connected to, that is a node corresponding to a data attribute in the AR data table block
    public ColumnNode connectedNode { get; set; }
}
