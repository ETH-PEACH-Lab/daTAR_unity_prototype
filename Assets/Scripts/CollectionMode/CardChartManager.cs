using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChartManager : MonoBehaviour
{
    private Transform textTemplate;
    // Start is called before the first frame update
    void Start()
    {
        textTemplate = transform.Find("text_template");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void addData(List<string> data)
    {
        textTemplate.gameObject.SetActive(false);

    }
}
