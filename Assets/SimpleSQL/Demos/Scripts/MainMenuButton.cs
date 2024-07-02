namespace SimpleSQL.Demos
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using SimpleSQL;

    /// <summary>
    /// Handles the main menu button to return to the main menu
    /// </summary>
    public class MainMenuButton : MonoBehaviour
    {
        public SimpleSQLManager dbManager;

        public void MainMenu()
        {
            dbManager.Close();
            dbManager.Dispose();

            SceneManager.LoadScene(0);
        }
    }
}