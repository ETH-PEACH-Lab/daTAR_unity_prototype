using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showCollections : MonoBehaviour
{
    public GameObject scrollView;
    public bool isEnabled = true;
    public void ButtonClicked()
    {
     
        isEnabled = !isEnabled;
        scrollView.SetActive(isEnabled);
    }
}
