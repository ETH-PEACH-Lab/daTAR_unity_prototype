using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowAnimation : MonoBehaviour
{
    public GameObject dataTable;

    private UnityEngine.UI.Image image;
    
    public void toggleAnimation()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        if (dataTable.activeSelf)
        {
            image.color = new Color32(89,89,89,255);
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            image.color = new Color32(182, 181, 181,255);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
