using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickAdd() { 
        gameObject.SetActive(true);
    }
}
