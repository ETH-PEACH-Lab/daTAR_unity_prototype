namespace SimpleSQL.Demos
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections.Generic;
	using System;
	using System.Linq;

	/// <summary>
	/// This script shows how you can use encryption in your database.
    /// Encryption is handled behind the scenes as you read and write data to
    /// your class objects.
	/// </summary>
	public class Encryption : MonoBehaviour
	{
		// reference to our db manager object
		public SimpleSQL.SimpleSQLManager dbManager;

		public UpdateExample updateExample;

		// input fields
		public InputField toEncryptField;

		// reference to our output text object
		public Text outputText;

		void Start()
		{
			// update the example to the newer version of the database if necessary
			updateExample.UpdateDatabase(dbManager);

			// reload the data in the database
			ReadAndDecrypt();
		}

		private void ResetGUI()
		{
			// Reset the temporary GUI variables
			toEncryptField.text = "";

			outputText.text = "";
		}

		/// <summary>
		/// Encrypts and stores the value of the input string
		/// </summary>
		public void Encrypt()
		{
			// set up a new record and store the clear text versions of the data
			var model = new EncryptedData()
			{
				TextA = toEncryptField.text,
				TextB = DateTime.Now.ToString()
			};

            // insert into the database
			dbManager.Insert(model);

            // show the result
			ResetGUI();

			outputText.text = $"'{model.TextA}' inserted and encrypted to '{model.EncryptedTextA}'";
		}

        /// <summary>
        /// Reads all records in database and shows the decrypted values.
        /// The TextA and TextB fields are automatically populated when the
        /// database is read.
        /// </summary>
        public void ReadAndDecrypt()
        {
			ResetGUI();

			var results = dbManager.Query<EncryptedData>("SELECT * FROM EncryptedData");

            foreach (var result in results)
            {
				// note that we reference the TextA and TextB fields instead of the EncryptedTextA and EncryptedTextB database fields.
				outputText.text += $"{result.ID}: {result.TextA} @ {result.TextB}\n";
            }
        }
    }
}
