using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserView : MonoBehaviour
{
    public GameObject name;
    public GameObject role;

    //[SerializeField] private UserManager userManager;
    void Start()
    {
        User user = UserManager.getUser();
        if (user != null)
        {
            name.GetComponent<TMPro.TextMeshProUGUI>().text += user.name;
            role.GetComponent<TMPro.TextMeshProUGUI>().text += user.role.ToString();
        }
        
    }

}
