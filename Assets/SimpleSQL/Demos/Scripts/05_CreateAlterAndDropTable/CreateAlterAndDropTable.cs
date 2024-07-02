namespace SimpleSQL.Demos
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections.Generic;

	/// <summary>
	/// This script demonstrates how to create a table programmatically at runtime. You can create
	/// a table directly from a class structure, or by calling a SQL statement.
	///
	/// In this example we will not overwrite the working database since we are updating the data. If
	/// we were to overwrite, then changes would be lost each time the scene is run again.
	/// </summary>
	public class CreateAlterAndDropTable : MonoBehaviour
	{

		// reference to the database manager in the scene
		public SimpleSQL.SimpleSQLManager dbManager;

		// reference to the output text in the scene
		public Text outputText;

		void Start()
		{
			outputText.text = "";
		}

		/// <summary>
		/// Output the results of the operation
		/// </summary>
		/// <param name="results"></param>
		private void Results(string results)
		{
			outputText.text = results;
		}

		/// <summary>
		/// Creates the table using the class definition
		/// </summary>
		public void CreateTable()
		{
			// Check out the StarShip class to see the various attributes
			// and how they can be used to set up your table.
			dbManager.CreateTable<StarShip>();

			Results("Create Table Success!");
		}

		/// <summary>
		/// Creates the table using a SQL statement
		/// </summary>
		public void CreateTable_Query()
		{
			string sql;

			// Start a transaction to batch the commands together
			dbManager.BeginTransaction();

			// Create the table
			sql = "CREATE TABLE \"StarShip\" " +
					"(\"StarShipID\" INTEGER PRIMARY KEY  NOT NULL, " +
					"\"StarShipName\" varchar(60) NOT NULL, " +
					"\"HomePlanet\" varchar(100) DEFAULT Earth, " +
					"\"Range\" FLOAT NOT NULL, " +
					"\"Armor\" FLOAT DEFAULT 120, " +
					"\"Firepower\" FLOAT)";
			dbManager.Execute(sql);

			// Create an index on the starship name
			sql = "CREATE INDEX \"StarShip_StarShipName\" on \"StarShip\"(\"StarShipName\")";
			dbManager.Execute(sql);

			// Commit the transaction and run all the commands
			dbManager.Commit();

			Results("Create Table Query Success!");
		}

		/// <summary>
		/// Adds a column to a data table
		/// </summary>
		public void AddColumn()
		{
			string sql;

			sql = "ALTER TABLE \"StarShip\" ADD COLUMN \"NewField\" INTEGER";
			dbManager.Execute(sql);

			Results("Add Column Success!");
		}

		/// <summary>
		/// Drops a column from a data table. Note that there is no simple way to drop a column
		/// from a table, so we first backup our current table, create a new table with the
		/// new structure, copy the data from the backup into the new table, then drop
		/// the backup.
		///
		/// This method also works for renaming a column or changing its type
		/// </summary>
		public void DropColumn()
		{
			string sql;

			// start a transaction to speed up processing
			dbManager.BeginTransaction();

			// rename our table to a backup name
			sql = "ALTER TABLE \"StarShip\" RENAME TO \"Temp_StarShip\"";
			dbManager.Execute(sql);

			// create a new table with our desired structure, leaving out the dropped column(s)
			sql = "CREATE TABLE \"StarShip\" " +
						"(\"StarShipID\" integer PRIMARY KEY  NOT NULL , " +
						"\"StarShipName\" varchar(60) NOT NULL , " +
						"\"HomePlanet\" varchar(100) DEFAULT Earth , " +
						"\"Range\" float NOT NULL , " +
						"\"Armor\" float DEFAULT 120 , " +
						"\"Firepower\" float) ";
			dbManager.Execute(sql);

			// copy the data from the backup table to our new table
			sql = "INSERT INTO \"StarShip\" " +
						"SELECT " + "" +
						"\"StarShipID\", " +
						"\"StarShipName\", " +
						"\"HomePlanet\", " +
						"\"Range\", " +
						"\"Armor\", " +
						"\"Firepower\" " +
						"FROM \"Temp_StarShip\"";
			dbManager.Execute(sql);

			// drop the backup table
			sql = "DROP TABLE \"Temp_StarShip\"";
			dbManager.Execute(sql);

			// commit the transaction and run all the commands
			dbManager.Commit();

			Results("Drop Column Success!");
		}

		/// <summary>
		/// Removes the table from the database
		/// </summary>
		public void DropTable()
		{
			string sql;

			// Drop the table
			sql = "DROP TABLE \"StarShip\"";
			dbManager.Execute(sql);

			Results("Drop Table Success!");
		}
	}
}
