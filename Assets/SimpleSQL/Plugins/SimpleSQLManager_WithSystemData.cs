using UnityEngine;
using System;
using System.Data;
using SimpleSQL;

public class SimpleSQLManager_WithSystemData : SimpleSQL.SimpleSQLManager
{
    protected override void CreateConnection(string documentsPath)
    {
        _db = new SimpleSQL.SQLiteConnection_WithSystemData(documentsPath);
    }

    /// <summary>
    /// Creates a SQLiteCommand given the command text (SQL) with arguments. Place a '?'
    /// in the command text for each of the arguments and then executes that command.
    /// It returns a datatable populated with the results. 
    /// </summary>
    /// <param name="query">
    /// The fully escaped SQL.
    /// </param>
    /// <param name="args">
    /// Arguments to substitute for the occurences of '?' in the query.
    /// </param>
    /// <returns>
    /// A datatable populated with the results
    /// </returns>	
    public DataTable Query(string query, params object[] args)
    {
        Initialize(false);

        return ((SimpleSQL.SQLiteConnection_WithSystemData)_db).Query(query, args);
    }
}