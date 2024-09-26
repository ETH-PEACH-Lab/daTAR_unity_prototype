using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserManager : MonoBehaviour
{
    //TODO: replace with correct api url
    private string url = "http://10.5.36.22:8000/users/";
    private int id;

    private void Start()
    {
        //TODO: have a UI interface for user to login
        loginUser(2);
    }

    /// <summary>
    /// start coroutine to fetch user data based on id from remote db
    /// </summary>
    /// <param name="id">user id to look up in remote db</param>
    void loginUser(int id)
    {
        this.id = id;
        Debug.Log("login user: "+id);
        StartCoroutine(FetchData());
    }

    /// <summary>
    /// tries to fetch user data from remote db and saves it in PlayerPrefs
    /// </summary>
    /// <returns>
    /// reslut of webrequest
    /// </returns>
    public IEnumerator FetchData()
    {
        //TODO: set timeout if webrequest takes to long
        using (UnityWebRequest request = UnityWebRequest.Get(url + id.ToString()))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("request error: "+request.error);
            }
            else
            {
                User user = new User();
                user = JsonUtility.FromJson<User>(request.downloadHandler.text);
                Debug.Log("userrrr"+user.name);
                string json = JsonUtility.ToJson(user);
                PlayerPrefs.SetString("user", json);
                PlayerPrefs.Save();
            }
        }
    }

    /// <summary>
    /// searches PlayerPrefs for user data
    /// </summary>
    /// <returns>
    /// user data if already safed during the session, null otherwise
    /// </returns>
    public static User getUser()
    {
        string jsonString = PlayerPrefs.GetString("user");
        if (jsonString != null)
        {
            User user = JsonUtility.FromJson<User>(jsonString);
            return user;
        }
        return null;
    }

}

//helper class to store user data
public class User
{
    public int id;
    public string name;
    public int role;
}