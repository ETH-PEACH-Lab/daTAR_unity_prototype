using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeButton : MonoBehaviour
{
    public ToggleVisibility toggleVisibility;
    private void OnMouseUpAsButton()
    {
        Debug.Log("node clicked");
        toggleVisibility.toggleActive();

    }
}
