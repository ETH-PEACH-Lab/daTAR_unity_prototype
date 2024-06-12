using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserManager : MonoBehaviour
{
    private string url = "http://localhost:8000/users/";
    private int id;

    private void Start()
    {
        loginUser(3);
    }
    void loginUser(int id)
    {
        //get user data from remote db
        //save user data in playerPrefs
        this.id = id;
        Debug.Log("loginnn"+id);
        StartCoroutine(FetchData());
    }
    public IEnumerator FetchData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url + id.ToString()))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("requestttt"+request.error);
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
public class User
{
    public int id;
    public string name;
    public int role;
}