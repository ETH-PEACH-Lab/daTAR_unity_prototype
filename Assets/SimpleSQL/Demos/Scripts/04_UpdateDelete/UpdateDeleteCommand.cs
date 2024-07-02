namespace SimpleSQL.Demos
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using System;
    using System.Linq;

    /// <summary>
    /// This script shows how to use the Update command with a class definition and also
    /// through SQL statements.
    ///
    /// In this example we will not overwrite the working database since we are updating the data. If
    /// we were to overwrite, then changes would be lost each time the scene is run again.
    /// </summary>
    public class UpdateDeleteCommand : MonoBehaviour
    {

        // The list of player stats from the database
        private List<PlayerStats> _playerStatsList;
        private List<GameObject> recordObjectList;

        // These variables will be used to store data from the GUI interface
        private string _newPlayerName;
        private string _newPlayerTotalKills;
        private string _newPlayerPoints;

        // Player ID key field pulled from the first record in the table
        private int _playerId;

        // reference to our db manager object
        public SimpleSQL.SimpleSQLManager dbManager;

        // label showing the currently edited record
        public Text playerIdLabel;

        // input fields
        public InputField playerNameInput;
        public InputField totalKillsInput;
        public InputField pointsInput;

        // the panel with all the edit fields
        public GameObject editPanel;

        // reference to our container object
        public Transform container;

        // record object to instantiate in the scroll view
        public GameObject updateRecordPrefab;
        public float recordObjectHeight;

        void Start()
        {
            recordObjectList = new List<GameObject>();

            // reset the GUI and reload
            ResetGUI();
        }

        public void ResetGUI()
        {
            // clear out the previous list of record objects
            foreach (var recordObject in recordObjectList)
            {
                GameObject.DestroyImmediate(recordObject);
            }

            // hide the edit panel
            editPanel.SetActive(false);

            _playerId = -1;

            // Reset the temporary GUI variables
            playerIdLabel.text = "";
            playerNameInput.text = "";
            totalKillsInput.text = "";
            pointsInput.text = "";

            // Loads the player stats from the database using Linq
            _playerStatsList = new List<PlayerStats>(from ps in dbManager.Table<PlayerStats>() select ps);

            var y = -recordObjectHeight;

            // loop through each stat record
            foreach (var playerStatRecord in _playerStatsList)
            {
                // instantiate the record and add it to our list of objects
                var recordObject = GameObject.Instantiate(updateRecordPrefab);
                recordObjectList.Add(recordObject);

                // set the record inside the scroll view container
                recordObject.transform.SetParent(container);
                var rectTransform = recordObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(10, y);

                // populate the record
                var record = recordObject.GetComponent<UpdateRecord>();
                record.SetRecord(playerStatRecord.PlayerID,
                                    "<color=#1abc9c>Id:</color> " + playerStatRecord.PlayerID.ToString() + " " +
                                    "<color=#1abc9c>Name:</color> " + playerStatRecord.PlayerName + " " +
                                    "<color=#1abc9c>Total Kills:</color> " + playerStatRecord.TotalKills.ToString() + " " +
                                    "<color=#1abc9c>Points:</color> " + playerStatRecord.Points.ToString() + "\n");

                // set the record's edit button callback to the method here
                record.editButtonClicked = Edit;

                // increment the y position for the next record
                y -= recordObjectHeight;
            }
        }

        /// <summary>
        /// Sets up the UI inputs based on the player stat record selected
        /// </summary>
        /// <param name="playerId"></param>
        public void Edit(int playerId)
        {
            ResetGUI();

            _playerId = playerId;

            // determine if the record exists
            bool recordExists;
            var firstRecord = dbManager.QueryFirstRecord<PlayerStats>(out recordExists, "SELECT * FROM PlayerStats WHERE PlayerID = ?", _playerId);

            if (recordExists)
            {
                // show the edit panel;
                editPanel.SetActive(true);

                // set the player id label
                playerIdLabel.text = "PlayerId: " + _playerId.ToString();

                // populate the edit fields with the record if it exists
                playerNameInput.text = firstRecord.PlayerName;
                totalKillsInput.text = firstRecord.TotalKills.ToString();
                pointsInput.text = firstRecord.Points.ToString();
            }
            else
            {
                // hide the edit panel
                editPanel.SetActive(false);
            }
        }

        /// <summary>
        /// Chooses the update method to use based on the button clicked
        /// </summary>
        /// <param name="method"></param>
        public void UpdateDeleteViewCommand(string method)
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
                case "Update":

                    UpdatePlayerStats_Simple(_playerId, playerName, totalKills, points);

                    break;

                case "UpdateQuery":

                    UpdatePlayerStats_Query(_playerId, playerName, totalKills, points);

                    break;

                case "Delete":

                    DeletePlayerStats_Simple(_playerId);

                    break;

                case "DeleteQuery":

                    DeletePlayerStats_Query(_playerId);

                    break;
            }

            ResetGUI();
        }

        /// <summary>
        /// Updates the player stats table using the class definition. No need for SQL here.
        /// </summary>
        /// <param name='playerID'>
        /// The ID of the player to update
        /// </param>
        /// <param name='playerName'>
        /// Player name.
        /// </param>
        /// <param name='totalKills'>
        /// Total kills.
        /// </param>
        /// <param name='points'>
        /// Points.
        /// </param>
        private void UpdatePlayerStats_Simple(int playerId, string playerName, int totalKills, int points)
        {
            // Set up a player stats class, setting all values including the playerID
            PlayerStats playerStats = new PlayerStats { PlayerID = playerId, PlayerName = playerName, TotalKills = totalKills, Points = points };

            // the database manager will update all the fields except the primary key which it uses to look up the data
            dbManager.UpdateTable(playerStats);
        }

        /// <summary>
        /// Updates the player stats by executing a SQL statement. Note that no data is returned, this only modifies the table
        /// </summary>
        /// <param name='playerID'>
        /// The ID of the player to update
        /// </param>
        /// <param name='playerName'>
        /// Player name.
        /// </param>
        /// <param name='totalKills'>
        /// Total kills.
        /// </param>
        /// <param name='points'>
        /// Points.
        /// </param>
        private void UpdatePlayerStats_Query(int playerId, string playerName, int totalKills, int points)
        {
            // Call our SQL statement using ? to bind our variables
            dbManager.Execute("UPDATE PlayerStats SET PlayerName = ?, TotalKills = ?, Points = ? WHERE PlayerID = ?", playerName, totalKills, points, playerId);
        }

        /// <summary>
        /// Deletes the first player stat in the table using the class definition. No need for SQL here.
        /// </summary>
        /// <param name='playerID'>
        /// The ID of the player to update
        /// </param>
        private void DeletePlayerStats_Simple(int playerID)
        {
            // Set up a player stats class, setting the key field
            PlayerStats playerStats = new PlayerStats { PlayerID = playerID };

            dbManager.Delete<PlayerStats>(playerStats);
        }

        /// <summary>
        /// Deletes the first player stat by executing a SQL statement. Note that no data is returned, this only modifies the table
        /// </summary>
        /// <param name='playerID'>
        /// The ID of the player to update
        /// </param>
        private void DeletePlayerStats_Query(int playerID)
        {
            // Call our SQL statement using ? to bind our variables
            dbManager.Execute("DELETE FROM PlayerStats WHERE PlayerID = ?", playerID);
        }
    }
}
