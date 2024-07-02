namespace SimpleSQL.Demos
{
    using UnityEngine;

    /// <summary>
    /// This class is only used to update the demo from a previous version.
    /// </summary>
    public class UpdateExample : MonoBehaviour
    {
        /// <summary>
        /// Class used to get the record count for the update example
        /// </summary>
        public class CountRecord
        {
            public int Count { get; set; }
        }

        /// <summary>
        /// This method is only used to update the example to the newer method of encryption using two fields.
        /// </summary>
        public void UpdateDatabase(SimpleSQLManager dbManager)
        {
            var sql = "SELECT COUNT(*) AS Count FROM pragma_table_info('EncryptedData') WHERE name='EncryptedText'";

            bool recordExists;
            var result = dbManager.QueryFirstRecord<CountRecord>(out recordExists, sql);
            if (recordExists && result.Count > 0)
            {
                Debug.Log("Updating example to use new encrypted database");

                sql = "DROP TABLE EncryptedData";
                dbManager.Execute(sql);

                dbManager.CreateTable<EncryptedData>();
            }
        }
    }
}
