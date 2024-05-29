using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            bool hitFound = Physics.Raycast(ray, out hit);
            if(hitFound)
            {
                Debug.Log("hit");
                MeshRenderer mr = hit.transform.GetComponent<MeshRenderer>();
                if(mr.material.color != Color.red )
                {
                    mr.material.color = Color.red;
                }else
                {
                    mr.material.color = Color.gray;
                }
            }
            
        }
    }
}
