using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartTypeSelection : MonoBehaviour
{
    public List<Transform> chartTypes;

    private Transform previouslySelected = null;

    private void Start()
    {
        foreach (Transform t in chartTypes)
        {
            Button b = t.GetComponent<Button>();
            Transform captured = t;
            b.onClick.AddListener(() => onSelect(t));
        }
    }

    private void onSelect(Transform selected)
    {
        if (previouslySelected != null)
        {
            previouslySelected.GetComponent<Image>().color = new Color32(245, 245, 245, 255);
        }
        selected.GetComponent<Image>().color = new Color32(179, 225, 251, 255);
        previouslySelected = selected;
    }
}
