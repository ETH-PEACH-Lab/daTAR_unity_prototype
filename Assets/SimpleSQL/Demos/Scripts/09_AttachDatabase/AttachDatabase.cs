namespace SimpleSQL.Demos
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections.Generic;
	using System.IO;

    /// <summary>
    /// This script shows how you can attach a database to your base database so that you
    /// can perform queries between them.
    /// </summary>
	public class AttachDatabase : MonoBehaviour
	{
        /// <summary>
        /// This is the base database that we start with.
        /// In this example, it is the Fantasy database.
        /// </summary>
		public SimpleSQL.SimpleSQLManager dbManager;

        /// <summary>
        /// The file to attach to
        /// </summary>
		public TextAsset attachDatabaseFile;

        /// <summary>
        /// This is the name of the database we will be attaching
        /// to. In this example, it is the SciFi database.
        /// </summary>
		public string attachDatabaseNewFileName;

        /// <summary>
        /// Overwrite the attach database file if it exists at the
        /// working directory already.
        /// WARNING: this will overwrite any
        /// data written at runtime and should only be used for
        /// read-only databases.
        /// </summary>
		public bool overwriteAttachDatabaseIfExists;

		// reference to the gui text object in our scene that will be used for output
		public Text outputText;

		void Start()
		{
			outputText.text = "";

            // get the final path in the working directory by combining the persistentDataPath with the new file name
			var attachDatabaseFilePath = Path.Combine(Application.persistentDataPath, attachDatabaseNewFileName);

            // extract the attach database file from the Unity project into the working directory
            if (!ExtractDatabase(attachDatabaseFilePath))
            {
                // failed, exit
                return;
            }

            // attach to the scifi database	
			dbManager.Execute("ATTACH DATABASE '" + attachDatabaseFilePath + "' AS SciFi");

            // query the players from the newly attached database and output them to the screen
			outputText.text += "Players:\n";
			var players = dbManager.Query<PlayerStats>("SELECT * FROM PlayerStats");
            foreach (var player in players)
            {
				outputText.text += player.PlayerName + "\n";
            }

			outputText.text += "\n\n";

            // query the weapons from the original datbase and output them to the screen
			outputText.text += "Weapons:\n";
			var weapons = dbManager.Query<Weapon>("SELECT * FROM Weapon");
			foreach (var weapon in weapons)
			{ 
				outputText.text += weapon.WeaponName + "\n";
			}
		}

        /// <summary>
        /// This method extracts the database file from the Unity project into
        /// the working directory. If the file does not exists, it will copy it.
        /// If the file already exists, it will only copy if the overwriteAttachDatabaseIfExists
        /// is true.
        /// </summary>
        /// <param name="attachDatabaseFilePath"></param>
        /// <returns></returns>
        private bool ExtractDatabase(string attachDatabaseFilePath)
        {
            // check if the file already exists
            bool fileExists = File.Exists(attachDatabaseFilePath);

            if ((overwriteAttachDatabaseIfExists && fileExists) || !fileExists)
            {
                // file does not exist or the overwrite flag is true

                try
                {
                    if (fileExists)
                    {
                        // delete the file if it already exists
                        File.Delete(attachDatabaseFilePath);
                    }

                    // extract the file data to the working path
                    File.WriteAllBytes(attachDatabaseFilePath, attachDatabaseFile.bytes);
                }
                catch
                {
                    Debug.LogError("Failed to open database at the working path: " + attachDatabaseFilePath);
                    return false;
                }
            }

            return true;
        }
    }
}
