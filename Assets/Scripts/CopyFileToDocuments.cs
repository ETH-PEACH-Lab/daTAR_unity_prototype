using UnityEngine;
using System.IO;
using System.Collections;

public class CopyFileToDocuments : MonoBehaviour //attach script to any game object to copy a file into a build folder, needed for allowing file import on iOS devises
{
    void Start()
    {
        string fileName = "snacks.csv"; // Change this to your file's name
        string sourcePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string destinationPath = Path.Combine(Application.persistentDataPath, fileName);

        // Ensure the destination directory exists
        Directory.CreateDirectory(Application.persistentDataPath);

        if (File.Exists(destinationPath))
        {
            Debug.Log("File already exists in Documents directory.");
            return;
        }

        StartCoroutine(CopyFile(sourcePath, destinationPath));
    }

    private IEnumerator CopyFile(string sourcePath, string destinationPath)
    {
        if (sourcePath.Contains("://") || sourcePath.Contains(":///"))
        {
            // For Android or WebGL where streamingAssetsPath is a URL
            using (WWW www = new WWW(sourcePath))
            {
                yield return www;

                if (string.IsNullOrEmpty(www.error))
                {
                    File.WriteAllBytes(destinationPath, www.bytes);
                }
                else
                {
                    Debug.LogError("Error copying file: " + www.error);
                }
            }
        }
        else
        {
            // For other platforms
            File.Copy(sourcePath, destinationPath);
        }

        Debug.Log("File copied to Documents directory: " + destinationPath);
    }
}