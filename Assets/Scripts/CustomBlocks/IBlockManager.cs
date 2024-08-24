using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockManager 
{
    //register datatable associated to a collection 
    public void addUserInput(List<Dictionary<string, string>> userInput, Collection fromCollection);
}
