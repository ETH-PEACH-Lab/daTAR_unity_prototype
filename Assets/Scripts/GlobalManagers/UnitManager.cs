using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UnitManager
{
    private static readonly object padlock = new object();
    private static UnitManager instance = null;

    private Dictionary<string, List<int>> activeUnits;

    UnitManager() { 
        activeUnits = new Dictionary<string, List<int>>();
    }
    public static UnitManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new UnitManager();
                }
                return instance;
            }
        }
    }


    public void addUnit(string collectionName, int rwoId)
    {
        if(!activeUnits.ContainsKey(collectionName))
        {
            activeUnits[collectionName] = new List<int>();
        }

        activeUnits[collectionName].Add(rwoId);
        
    }

    public List<int> getUnits(string collectionName)
    {
        if (!activeUnits.ContainsKey(collectionName))
        {
            return null;
        }

        return activeUnits[collectionName];
    }
}
