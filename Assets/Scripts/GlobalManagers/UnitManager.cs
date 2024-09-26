using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class UnitManager
{
    private static readonly object padlock = new object();
    private static UnitManager instance = null;

    // data structure to store all the data points in the scene that should be highlighted,
    // that is data point that are attached to an object
    //key = collection name , value list of row ids from the collection
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

    /// <summary>
    /// adds a data point to the units to highlight
    /// </summary>
    /// <param name="collectionName">collection name data point belongs to</param>
    /// <param name="rowId">row id of the data point in the collection</param>
    public void addUnit(string collectionName, int rowId)
    {
        if(!activeUnits.ContainsKey(collectionName))
        {
            activeUnits[collectionName] = new List<int>();
        }

        activeUnits[collectionName].Add(rowId);
    }

    /// <summary>
    /// gets all data points that should be highlighted from a given collection
    /// </summary>
    /// <param name="collectionName">collection to search for data points</param>
    /// <returns>list of data points that should be highlighted from given collection, if no data point from the given collection added return null</returns>
    public List<int> getUnits(string collectionName)
    {
        if (collectionName == null || !activeUnits.ContainsKey(collectionName))
        {
            return null;
        }
        return activeUnits[collectionName];
    }

    /// <summary>
    /// removes a data point from the units to highlight
    /// </summary>
    /// <param name="collectionName">collection name data point belongs to</param>
    /// <param name="rowId">row id of the data point in the collection</param>
    public void removeUnit(string collectionName, int rowId)
    {
        if(collectionName != null && activeUnits.ContainsKey(collectionName))
        {
            try
            {
                activeUnits[collectionName].Remove(rowId);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
