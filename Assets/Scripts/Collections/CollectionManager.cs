using UnityEngine;
using System.Data;
using SQLite;
using System.IO;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Collections;
using Unity.VisualScripting;
public sealed class CollectionManager
{
    private static CollectionManager instance = null;
    private static readonly object padlock = new object();

    private SQLiteConnection dbConnection;
    private string tabelName = "user_collections";
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
                last_mod DATETIME
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
        return res;
    }

    public int updateCollection(Collection collection)
    {
        //to do collection should have unique name
        var res = dbConnection.Update(collection);
        return res;
    }
    public List<Collection> getAllCollections() 
    {
        List<Collection> list = dbConnection.Query<Collection>("SELECT * FROM Collections");
        
        return list; 
    }

    public int createDataTable(string tableName, string[] attributes)
    {
        var res = 0;

        return res;
    }

    public int addData(string tableName, string[] attributes, string[] fields)
    {
        var res = 0;

        return res;
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
}
