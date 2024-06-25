using UnityEngine;
using System.Data;
using SQLite;
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
//using UnityEditor.MemoryProfiler;
public sealed class CollectionManager
{
    private static CollectionManager instance = null;
    private static readonly object padlock = new object();

    private SQLiteConnection dbConnection;
    //private string tabelName = "user_collections";
    CollectionManager()
    {
        dbConnection= new SQLiteConnection($"{Application.persistentDataPath}sqliteDB.db");
        
        //creation of table causes problems when running on android tablet. When creating the table for second time in same db
        //dbConnection.CreateTable<Collection>();
        string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Collections (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                entries INTEGER,
                last_mod DATETIME,
                attributes TEXT
            )";
        dbConnection.Execute(createTableQuery);
        //addCollection("example8");
    }

    public static CollectionManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new CollectionManager();
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

        var res = dbConnection.Insert(newCollection);
        if (res != 0)
        {
            return newCollection;
        }
        return null;
    }
    public int removeCollection(Collection collection)
    {
        var res = dbConnection.Delete(collection);
        string dropTable = @"DROP TABLE IF EXISTS "+collection.Name+collection.Id;
        var res2 = dbConnection.Execute(dropTable);
        Debug.Log(res2 + " res2");
        return res;
    }

    public int updateCollection(Collection collection, string oldName)
    {
        var res = dbConnection.Update(collection);
        //upadate table name
        try
        {
            string newName = collection.Name + collection.Id;
            string query = $"ALTER TABLE {oldName} RENAME TO {newName};";
            dbConnection.Execute(query);

        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
        
        return res;
    }
    public List<Collection> getAllCollections() 
    {
        List<Collection> list = dbConnection.Query<Collection>("SELECT * FROM Collections");
        
        return list; 
    }

    public Collection getCollection(string name)
    {
        List<Collection> result = dbConnection.Query<Collection>("SELECT * FROM Collections WHERE name = '"+name+"' ");
       
        if(result.Count > 0)
        {
            return result[0];
        }
        return null;
    }

    public int createDataTable(string tableName)
    {
        string createTableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} (id INTEGER PRIMARY KEY AUTOINCREMENT, data TEXT)"; 
        
        var res = dbConnection.Execute(createTableQuery);
     
        return res;
    }

    public int addData(string tableName, string[] fields)
    {
        string newData = string.Join(", ", fields);
        //string columns = string.Join(", ", attributes);
        //string values = string.Join(", ", fields.Select(f => $"'{f.Replace("'", "''")}'")); // Escape single quotes
        string insertQuery = $"INSERT INTO {tableName} (data) VALUES ('{newData}')";
        //Debug.Log(insertQuery);
        // Execute the query
        var res = dbConnection.Execute(insertQuery);
        //Debug.Log(res);
        return res;
    }

    public string[] getDataTable(string tableName)
    {
        string query = $"SELECT data FROM {tableName}";

        // Execute the query and get the result
        var command = dbConnection.CreateCommand(query);
        try
        {
            List<UserDefinedTable> result = command.ExecuteQuery<UserDefinedTable>();
            List<string> data = new List<string>();
            foreach (UserDefinedTable table in result)
            {
                string[] s = table.Data.Split(", ");
                foreach (string s2 in s)
                {
                    data.Add(s2);
                    //Debug.Log(s2);
                }
            }


            return data.ToArray();
        }
        catch(Exception e)
        {
            Debug.Log("from try catch "+ e);
            return null;
        }
        
    }
    public List<string> getColumnNames(string tableName)
    {
        List<string> columnNames = new List<string>();

        string query = $"PRAGMA table_info({tableName})";
        var result = dbConnection.Query<ColumnInfo>(query);

        foreach (var column in result)
        {
            columnNames.Add(column.name);
        }

        return columnNames;
    }

    private class UserDefinedTable
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("data")]
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



[Table("Collections")]
public class Collection
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("entries")]
    public int Entries { get; set; }

    [Column("last_mod")]
    public DateTime LastMod { get; set; }

    [Column("attributes")]
    public string Attributes { get; set; }
}
