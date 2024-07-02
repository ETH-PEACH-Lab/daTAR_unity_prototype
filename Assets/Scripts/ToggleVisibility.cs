using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    // Start is called before the first frame update
    public void toggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        Debug.Log("added2");
    }
}
