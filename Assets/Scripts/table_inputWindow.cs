using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class table_inputWindow : MonoBehaviour
{
    private TMP_InputField atr1Field;
    private TMP_InputField atr2Field;
    private TMP_InputField atr3Field;
    public Transform trackable;

    [SerializeField] private TableManager tableManager;
    private void Awake()
    {
        Hide();
        atr1Field = transform.Find("atr1_field").GetComponent<TMP_InputField>();
        atr2Field = transform.Find("atr2_field").GetComponent <TMP_InputField>();
        atr3Field = transform.Find("atr3_field").GetComponent<TMP_InputField>();

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void okBehaviour()
    {
        string atr1 = atr1Field.text;
        string atr2 = atr2Field.text;
        string atr3 = atr3Field.text;
        atr1Field.text = null;
        atr2Field.text = null;
        if(atr1 == null) { atr1 = "-"; }
        if(atr2 == null) { atr2 = "-"; }
        if (atr3 == null) { atr3 = "-"; }
        tableManager.addEntry(atr1, atr2, atr3);
        //barchart.loadBarchart();
        //add single bar not relad all
        //barchart.initBar(1f,1f,1f);
        //spawnBarchart chart = trackable.Find("Trackables").GetChild(0).GetComponent<spawnBarchart>();
        //Debug.Log(chart);
        //chart.addEntry();
        Hide();
    }
}
