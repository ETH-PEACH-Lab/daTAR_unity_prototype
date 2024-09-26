using UnityEngine;
using System.Data;
//using SQLite;
using System.IO;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;
using static UnityEngine.Rendering.DebugUI.Table;
using System.Xml;
using System.Collections.ObjectModel;
using SimpleSQL;
using static UnityEngine.XR.ARSubsystems.XRFaceMesh;
using static UnityEngine.Rendering.DebugUI;
using System.Text.RegularExpressions;

public sealed class CollectionManager : MonoBehaviour
{
    private static CollectionManager instance = null;
    private static readonly object padlock = new object();

    public SimpleSQLManager_WithSystemData dbManeger;

    //private SQLiteConnection dbConnection;
    CollectionManager()
    {

    }
    private void Start()
    {
        instance = this;
        //create single table holding a summary for every user defined collection
        string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Collection (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT,
                Entries INTEGER,
                LastMod DATETIME,
                Attributes TEXT
            )";

        var res = dbManeger.Execute(createTableQuery);
        // TODO: api call for retriving any shared collection and adding summary to the table Collection (adding entry calling addCollection() )
        // TODO: api call for creating a separate table for every added shared collection summary (create tables calling createDataTable() )
    }
    public static CollectionManager Instance
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
    /// <summary>
    /// adds summary of user defined collection to Collection table
    /// </summary>
    /// <param name="name">user definded name of collection or "untitled" if new collection</param>
    /// <returns>
    /// the newly added collection summary or if insertion failed null
    /// </returns>
    public Collection addCollection(string name)
    {
        var newCollection = new Collection
        {
            Name = name,
            Entries = 0,
            LastMod = DateTime.Now,
            Attributes = "",
        };

        var res = dbManeger.Insert(newCollection);
        
        if (res > 0)
        {
            return newCollection;
        }
        return null;
    }

    /// <summary>
    /// removes collection summary entry from the Collection table and drops corresponding table holding the collection data
    /// </summary>
    /// <param name="collection">collection summary to remove</param>
    /// <returns>
    /// 1 if collection summary entry was found and removed from the Collection table 0 otherwise
    /// </returns>
    public int removeCollection(Collection collection)
    {
        //string deletCol = "DELETE FROM Collections WHERE id = " + collection.Id;
        string dropTable = "DROP TABLE IF EXISTS " + collection.Name + collection.Id;

        var res = dbManeger.Delete(collection);

        var res2 = dbManeger.Execute(dropTable);

        return res;
    }

    /// <summary>
    /// updates summary data for a specified collection and if exists renames table holding collection data
    /// </summary>
    /// <param name="collection">collection summary to update</param>
    /// <param name="oldName">old name of the collection to update</param>
    /// <returns>
    /// 1 if collection summary was found in Collection table and updated 0 otherwise
    /// </returns>
    public int updateCollection(Collection collection, string oldName)
    {
        var res = dbManeger.UpdateTable(collection);
        Debug.Log("updated col " + res);
        //upadate table name
        try
        {
            string newName = collection.Name + collection.Id; //following naming convention for table names associated to a single collection summary
            string alterTable = "ALTER TABLE " + oldName + " RENAME TO " + newName;

            dbManeger.Execute(alterTable);
            Debug.Log("alter table name");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }

        return res;
    }

    /// <summary>
    /// searches for all the collection summary entries in Collection table
    /// </summary>
    /// <returns>
    /// list of all the entries in Collection table
    /// </returns>
    public List<Collection> getAllCollections()
    {
        List<Collection> list = dbManeger.Query<Collection>("SELECT * FROM Collection");

        return list;
    }

    /// <summary>
    /// searches for collection summary in Collection table matching a specified name
    /// </summary>
    /// <param name="name">collection summary name to search for</param>
    /// <returns>
    /// first collection summary with matching name null otherwise
    /// </returns>
    public Collection getCollection(string name)
    {
        List<Collection> result = dbManeger.Query<Collection>("SELECT * FROM Collection WHERE name = '" + name + "' ");

        if (result.Count > 0)
        {
            return result[0];
        }
        return null;
    }

    /// <summary>
    /// creates a new table in local db holding all the data for a single collection
    /// </summary>
    /// <param name="tableName">name of the table to create should be collection.Name + collection.Id </param>
    /// <param name="attributes">array of attribute names for new table</param>
    /// <param name="types">array of attribute types for new table order sould match with attribute array, type "REAL" for number, type "TEXT" for strings</param>
    /// <returns>
    /// 1 if table created 0 otherwise
    /// </returns>
    public int createDataTable(string tableName, string[] attributes, string[] types)
    {
        string createTableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} (id INTEGER PRIMARY KEY AUTOINCREMENT";
        for(int i = 0; i < attributes.Length; i++)
        {
            createTableQuery += ", " + attributes[i] + " " + types[i];
        }
        createTableQuery += ")";

        Debug.Log(createTableQuery);

        var res = dbManeger.Execute(createTableQuery);

        return res;
    }

    /// <summary>
    /// adds a new row in specified table
    /// </summary>
    /// <param name="tableName">name of the table to add rows should be collection.Name + collection.Id </param>
    /// <param name="attributes">array of attribute names for the table</param>
    /// <param name="fields">array of new data fields to add each entry should correspond to a attribute</param>
    /// <returns>
    /// row id of row added to table -1 otherwise
    /// </returns>
    public int addData(string tableName, string[] fields, string[] attributes)
    {
        //clean up data first
        string pattern = @"[^\x00-\x7F]+";
        for (int i = 0;i< fields.Length; i++)
        {
            fields[i] = Regex.Replace(fields[i], pattern, "_");
        }
        string columns = string.Join(", ", attributes);
        string values = string.Join(", ", fields.Select(f => $"'{f.Replace("'", "''")}'")); // Escape single quotes

        string query = "INSERT INTO " + tableName + " (" + columns + ") VALUES (" + values + ")";
        
        Debug.Log(query);

        var res = dbManeger.Execute(query);

        
        Debug.Log("insert data " + res);
        query = "SELECT last_insert_rowid()";
        System.Data.DataTable dt = dbManeger.Query(query);
        int lastId = -1;
        foreach (System.Data.DataRow row in dt.Rows)
        {
            for (int c = 0; c < dt.Columns.Count; c++)
            {
                Debug.Log(dt.Columns[c].ColumnName + "=" + row[c].ToString() +" c = "+c);
                
                lastId = int.Parse(row[c].ToString());
                Debug.Log(lastId +" "+lastId.GetType());
            }

        }



        return lastId;
    }

    /// <summary>
    /// searches for table matching a given name
    /// </summary>
    /// <param name="tableName">name of the table to return should be collection.Name + collection.Id </param>
    /// <returns>
    /// data table with matching name in list dictionary format null otherwise
    /// </returns>
    public List<Dictionary<string,string>> getDataTable(string tableName)
    {
        string query = $"SELECT * FROM {tableName}";

        // Execute the query and get the result

        try
        {
            // convert returned table into conventional data structure
            List<Dictionary<string,string>> table = new List<Dictionary<string,string>>();

            System.Data.DataTable dt = dbManeger.Query(query);
            foreach (System.Data.DataRow row in dt.Rows)
            {
                Dictionary<string,string> newRow = new Dictionary<string,string>();
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    newRow[dt.Columns[c].ColumnName] = row[c].ToString();

                }

                table.Add(newRow);
            }


            return table;
        }
        catch (Exception e)
        {
            Debug.Log("from try catch " + e);
            return null;
        }

    }

    /// <summary>
    /// executes a given sql query on local db
    /// </summary>
    /// <param name="query">user defined sql query to execute on local db </param>
    /// <returns>
    /// result from query in form of a modified data table following conventional data structure for storing data table null otherwise
    /// </returns>
    public List<Dictionary<string, string>> executeQuery(string query)
    {
        // Execute the query and get the result

        try
        {
            List<Dictionary<string, string>> table = new List<Dictionary<string, string>>();

            System.Data.DataTable dt = dbManeger.Query(query);
            
            foreach (System.Data.DataRow row in dt.Rows)
            {
                
                Dictionary<string, string> newRow = new Dictionary<string, string>();
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    newRow[dt.Columns[c].ColumnName] = row[c].ToString();
                }

                table.Add(newRow);
            }

            return table;
        }
        catch (Exception e)
        {
            Debug.Log("from try catch " + e);
            return null;
        }

    }

    /// <summary>
    /// adds new table column to specified data table
    /// </summary>
    /// <param name="tableName">name of the table to add to should be collection.Name + collection.Id </param>
    /// <param name="columnName">name of column to add to data table </param>
    /// <param name="dataType">type for the new column, type "REAL" for number, type "TEXT" for strings  </param>
    /// <returns>
    /// 1 if coulmn added -1 else
    /// </returns>

    public int addColumn(string tableName, string columnName, string dataType)
    {
        string query = "ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " " + dataType;
        try
        {
            dbManeger.Execute(query);
            return 1;
        }
        catch (Exception e)
        {
            Debug.Log("from try catch " + e);
            return -1;
        }
        
    }

    /// <summary>
    /// updates data table based on user defined sql query
    /// </summary>
    /// <param name="tableName">name of the table to add to should be collection.Name + collection.Id </param>
    /// <param name="updateQuery">user defined sql query </param>
    /// <returns>
    /// 1 if query executed -1 else
    /// </returns>
    public int updateDataTable(string tableName, string updateQuery)
    {
        string query = "UPDATE " + tableName + " SET " + updateQuery + " ;";
        Debug.Log(query);

        try
        {
            Debug.Log("updating "+ query);
            dbManeger.Execute(query);
            return 1;
        }catch(Exception e)
        {
            Debug.LogError(e);
            return -1;
        }
    }


    /// <summary>
    /// searches for table row with matching id
    /// </summary>
    /// <param name="tableName">name of the table to add to should be collection.Name + collection.Id </param>
    /// <param name="id">row id to look up </param>
    /// <returns>
    /// table row in a dictionary with matching row id null otherwise
    /// </returns>
    public Dictionary<string,string> getDataTableRow(string tableName, string id)
    {
        string query = "SELECT * FROM "+tableName+" WHERE id = "+id;
        try
        {
            List<Dictionary<string, string>> table = new List<Dictionary<string, string>>();

            System.Data.DataTable dt = dbManeger.Query(query);
            foreach (System.Data.DataRow row in dt.Rows)
            {
                Dictionary<string, string> newRow = new Dictionary<string, string>();
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    //Debug.Log(dt.Columns[c].ColumnName + "=" + row[c].ToString() +" c = "+c);
                    newRow[dt.Columns[c].ColumnName] = row[c].ToString();

                }

                table.Add(newRow);
            }


            return table[0];
        }
        catch (Exception e)
        {
            Debug.Log("from try catch " + e);
            return null;
        }
    }

}


//helper class holding a collection summary
public class Collection
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }

    public int Entries { get; set; }

    public DateTime LastMod { get; set; }

    public string Attributes { get; set; }
}
