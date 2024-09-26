using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockManager //implemented by CustomVisManager and CustomBlockManager
{
    //needed when connecting to a data point or data table , registers data points and associated collection
    public void addUserInput(List<Dictionary<string, string>> userInput, Collection fromCollection);
}
