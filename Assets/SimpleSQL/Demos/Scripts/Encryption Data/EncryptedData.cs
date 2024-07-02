namespace SimpleSQL.Demos
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SimpleSQL;

    /// <summary>
    /// One way to handle encryption / decryption of a database model.
    /// Note that you will need a pair of properties and a pair of private members for each field in your table.
    /// In this example, there are two fields, so that means four properties and four private members need to be set
    /// up here:
    /// Field1: EncryptedTextA, TextA, _encryptedTextA, _decryptedTextA
    /// Field2: EncryptedTextB, TextB, _encryptedTextB, _decryptedTextB.
    /// Setting up these properties here makes it super simple to use them in your logic without having to
    /// worry about the details of how the encryption is working.
    /// </summary>
    public class EncryptedData
    {
        /// <summary>
        /// Set this to a strong password that only you know
        /// </summary>
        private const string password = "ABCDEFG1234567890";

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        #region EncryptedTextA / TextA

        /// <summary>
        /// These private members will store the data but are inaccessible to your logic code.
        /// </summary>
        private string _encryptedTextA;
        private string _decryptedTextA;

        /// <summary>
        /// This is the field your database will use.
        /// This property stores the encrypted text.
        /// It also automatically decrypts the value to be used by TextA
        /// </summary>
        public string EncryptedTextA
        {
            get
            {
                return _encryptedTextA;
            }
            set
            {
                _encryptedTextA = value;
                _decryptedTextA = Cipher.Decrypt(_encryptedTextA, password);
            }
        }

        /// <summary>
        /// This is the field that you will reference in your logic.
        /// Encryption is handled behind the scenes for you inside the EncryptedTextA field.
        /// The Ignore attribute allows us to use the field in our logic without
        /// needing it in our database.
        /// </summary>
        [Ignore]
        public string TextA
        {
            get
            {
                return _decryptedTextA;
            }
            set
            {
                EncryptedTextA = Cipher.Encrypt(value, password);
            }
        }

        #endregion

        #region EncryptedTextB / TextB

        /// <summary>
        /// Same structure as the above EncryptedTextA / TextA without the comments for clarity
        /// </summary>

        private string _encryptedTextB;
        private string _decryptedTextB;

        public string EncryptedTextB
        {
            get
            {
                return _encryptedTextB;
            }
            set
            {
                _encryptedTextB = value;
                _decryptedTextB = Cipher.Decrypt(_encryptedTextB, password);
            }
        }

        [Ignore]
        public string TextB
        {
            get
            {
                return _decryptedTextB;
            }
            set
            {
                EncryptedTextB = Cipher.Encrypt(value, password);
            }
        }

        #endregion
    }
}
