namespace SimpleSQL.Demos
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections.Generic;
	using System;
	using System.Linq;

	/// <summary>
	/// This script shows how to call the Insert command on a database using simplified class-based
	/// methods or by calling the SQL statements directly. Transactions are shown in the functions that
	/// insert three times to show how to speed up your data.
	///
	/// In this example we will not overwrite the working database since we are updating the data. If
	/// we were to overwrite, then changes would be lost each time the scene is run again.
	/// </summary>
	public class InsertCommand : MonoBehaviour
	{

		// The list of player stats from the database
		private List<PlayerStats> _playerStatsList;

		// These variables will be used to store data from the GUI interface
		private string _newPlayerName;
		private string _newPlayerTotalKills;
		private string _newPlayerPoints;

		// reference to our db manager object
		public SimpleSQL.SimpleSQLManager dbManager;

		// input fields
		public InputField playerNameInput;
		public InputField totalKillsInput;
		public InputField pointsInput;

		// reference to our output text object
		public Text outputText;

		/// <summary>
        /// Unity's Start method
        /// </summary>
		void Start()
		{
			// clear out the output text since we are using GUI in this example
			outputText.text = "";

			// reset the GUI and reload
			ResetGUI();
		}

		private void ResetGUI()
		{
			// Reset the temporary GUI variables
			playerNameInput.text = "";
			totalKillsInput.text = "";
			pointsInput.text = "";

			// Loads the player stats from the database using Linq (not required to use Linq, but this is a simple example)
			_playerStatsList = new List<PlayerStats>(from ps in dbManager.Table<PlayerStats>() select ps);

			outputText.text = "";

			foreach (var playerStatRecord in _playerStatsList)
			{
				outputText.text += "<color=#1abc9c>Name:</color> " + playerStatRecord.PlayerName + " " +
									"<color=#1abc9c>Total Kills:</color> " + playerStatRecord.TotalKills.ToString() + " " +
									"<color=#1abc9c>Points:</color> " + playerStatRecord.Points.ToString() + "\n";
			}
		}

		/// <summary>
		/// Calls the appropriate insert method based on the button clicked
		/// </summary>
		/// <param name="method"></param>
		public void Insert(string method)
		{
			var playerName = playerNameInput.text;
			var totalKills = 0;
			var points = 0;

			if (!int.TryParse(totalKillsInput.text, out totalKills))
				totalKills = 0;

			if (!int.TryParse(pointsInput.text, out points))
				points = 0;

			switch (method)
			{
				case "Insert":

					SavePlayerStats_Simple(playerName, totalKills, points);

					break;

				case "Query":

					SavePlayerStats_Query(playerName, totalKills, points);

					break;

				case "Insert3":

					SavePlayerStats_SimpleThreeTimes(playerName, totalKills, points);

					break;

				case "Query3":

					SavePlayerStats_QueryThreeTimes(playerName, totalKills, points);

					break;
			}

			ResetGUI();
		}

		/// <summary>
		/// Saves the player stats by using the PlayerStats class structure. No need for SQL here.
		/// </summary>
		/// <param name='playerName'>
		/// Player name.
		/// </param>
		/// <param name='totalKills'>
		/// Total kills.
		/// </param>
		/// <param name='points'>
		/// Points.
		/// </param>
		private void SavePlayerStats_Simple(string playerName, int totalKills, int points)
		{
			// Initialize our PlayerStats class
			PlayerStats playerStats = new PlayerStats { PlayerName = playerName, TotalKills = totalKills, Points = points };

			// Insert our PlayerStats into the database
			dbManager.Insert(playerStats);
		}

		/// <summary>
		/// Saves the player stats by executing a SQL statement. Note that no data is returned, this only modifies the table
		/// </summary>
		/// <param name='playerName'>
		/// Player name.
		/// </param>
		/// <param name='totalKills'>
		/// Total kills.
		/// </param>
		/// <param name='points'>
		/// Points.
		/// </param>
		private void SavePlayerStats_Query(string playerName, int totalKills, int points)
		{
			// Call our SQL statement using ? to bind our variables
			dbManager.Execute("INSERT INTO PlayerStats (PlayerName, TotalKills, Points) VALUES (?, ?, ?)", playerName, totalKills, points);
		}

		/// <summary>
		/// Saves the player stats three times, using a collection of PlayerStats and inserting all at once. This example uses
		/// transactions which can vastly reduce the amount of time spent updating data when you have a lot of records to insert.
		/// </summary>
		/// <param name='playerName'>
		/// Player name.
		/// </param>
		/// <param name='totalKills'>
		/// Total kills.
		/// </param>
		/// <param name='points'>
		/// Points.
		/// </param>
		private void SavePlayerStats_SimpleThreeTimes(string playerName, int totalKills, int points)
		{
			// set up our playerStats variable to store data
			PlayerStats playerStats;

			// set up our collection of playerStats to pass to the database
			List<PlayerStats> playerStatsCollection = new List<PlayerStats>();

			// add player stats to the collection
			playerStats = new PlayerStats { PlayerName = playerName, TotalKills = totalKills, Points = points };
			playerStatsCollection.Add(playerStats);

			// add player stats to the collection
			playerStats = new PlayerStats { PlayerName = playerName, TotalKills = totalKills, Points = points };
			playerStatsCollection.Add(playerStats);

			// add player stats to the collection
			playerStats = new PlayerStats { PlayerName = playerName, TotalKills = totalKills, Points = points };
			playerStatsCollection.Add(playerStats);

			// insert all the player stats at one time by passing the collection
			dbManager.InsertAll(playerStatsCollection);
		}

		/// <summary>
		/// Saves the player stats three times using a SQL statement. The calls are made inside of a transaction
		/// by using BeginTransaction and Commit. Transactions vastly reduce the amount of time it takes
		/// to save multiple records.
		/// </summary>
		/// <param name='playerName'>
		/// Player name.
		/// </param>
		/// <param name='totalKills'>
		/// Total kills.
		/// </param>
		/// <param name='points'>
		/// Points.
		/// </param>
		private void SavePlayerStats_QueryThreeTimes(string playerName, int totalKills, int points)
		{
			// set up our insert SQL statement with ? parameters
			string sql = "INSERT INTO PlayerStats (PlayerName, TotalKills, Points) VALUES (?, ?, ?)";

			// start a transaction
			dbManager.BeginTransaction();

			// execute the SQL statement three times
			dbManager.Execute(sql, playerName, totalKills, points);
			dbManager.Execute(sql, playerName, totalKills, points);
			dbManager.Execute(sql, playerName, totalKills, points);

			// commit our transaction to the database
			dbManager.Commit();
		}
	}
}
