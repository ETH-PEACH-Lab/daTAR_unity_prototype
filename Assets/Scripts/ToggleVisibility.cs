using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    public List<GameObject> toHide = new List<GameObject>();
    public void toggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void hideOthers()
    {
        foreach (GameObject go in toHide)
        {
            go.SetActive(!gameObject.activeSelf);
        }
    }
}
