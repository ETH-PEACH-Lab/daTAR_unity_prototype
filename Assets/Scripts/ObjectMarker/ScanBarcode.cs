using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanBarcode : MonoBehaviour
{
    public void activateScanner()
    {
        BarcodeCam.Instance.activate();
    }
}
