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
//using UnityEditor.MemoryProfiler;
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
        //creation new table for all user defined table descriptions
        instance = this;
        string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Collection (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT,
                Entries INTEGER,
                LastMod DATETIME,
                Attributes TEXT
            )";

        var res = dbManeger.Execute(createTableQuery);
        Debug.Log("col manager start " + res);
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

    public Collection addCollection(string name)
    {
        //to do collection should have unique name
        var newCollection = new Collection
        {
            Name = name,
            Entries = 0,
            LastMod = DateTime.Now,
            Attributes = "",
        };

        var res = dbManeger.Insert(newCollection);
        Debug.Log("inserted " + res);
        if (res > 0)
        {
            return newCollection;
        }
        return null;
    }
    public int removeCollection(Collection collection)
    {
        //string deletCol = "DELETE FROM Collections WHERE id = " + collection.Id;
        string dropTable = "DROP TABLE IF EXISTS " + collection.Name + collection.Id;

        var res = dbManeger.Delete(collection);
        Debug.Log("del collection " + res);

        var res2 = dbManeger.Execute(dropTable);
        Debug.Log("drop table " + res2);

        return res;
    }

    public int updateCollection(Collection collection, string oldName)
    {
        var res = dbManeger.UpdateTable(collection);
        Debug.Log("updated col " + res);
        //upadate table name
        try
        {
            string newName = collection.Name + collection.Id;
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
    public List<Collection> getAllCollections()
    {
        List<Collection> list = dbManeger.Query<Collection>("SELECT * FROM Collection");

        return list;
    }

    public Collection getCollection(string name)
    {
        List<Collection> result = dbManeger.Query<Collection>("SELECT * FROM Collection WHERE name = '" + name + "' ");

        if (result.Count > 0)
        {
            return result[0];
        }
        return null;
    }

    public int createDataTable(string tableName, string[] attributes)
    {
        string createTableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} (id INTEGER PRIMARY KEY AUTOINCREMENT";
        foreach (string attr in attributes)
        {
            createTableQuery += ", " + attr + " TEXT";
        }
        createTableQuery += ")";

        Debug.Log(createTableQuery);

        var res = dbManeger.Execute(createTableQuery);
        Debug.Log("created table " + res);

        return res;
    }

    public int addData(string tableName, string[] fields, string[] attributes)
    {
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
    
    public List<Dictionary<string,string>> getDataTable(string tableName)
    {
        string query = $"SELECT * FROM {tableName}";

        // Execute the query and get the result

        try
        {
            List<Dictionary<string,string>> table = new List<Dictionary<string,string>>();

            System.Data.DataTable dt = dbManeger.Query(query);
            foreach (System.Data.DataRow row in dt.Rows)
            {
                Dictionary<string,string> newRow = new Dictionary<string,string>();
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    //Debug.Log(dt.Columns[c].ColumnName + "=" + row[c].ToString() +" c = "+c);
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

    public List<Dictionary<string, string>> executeQuery(string query)
    {
        //string query = $"SELECT * FROM {tableName}";

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
                    //Debug.Log(dt.Columns[c].ColumnName + "=" + row[c].ToString() +" c = "+c);
                    newRow[dt.Columns[c].ColumnName] = row[c].ToString();

                }

                table.Add(newRow);
            }

            //Debug.Log("row count "+rowCount);
            return table;
        }
        catch (Exception e)
        {
            Debug.Log("from try catch " + e);
            return null;
        }

    }



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
    public List<string> getColumnNames(string tableName)
    {
        List<string> columnNames = new List<string>();


        return columnNames;
    }

    private class UserDefinedTable
    {

        public int Id { get; set; }


        public string Data { get; set; }
    }

    private class ColumnInfo
    {
        public int cid { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int notnull { get; set; }
        public string dflt_value { get; set; }
        public int pk { get; set; }
    }
}



public class Collection
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }

    public int Entries { get; set; }

    public DateTime LastMod { get; set; }

    public string Attributes { get; set; }
}
