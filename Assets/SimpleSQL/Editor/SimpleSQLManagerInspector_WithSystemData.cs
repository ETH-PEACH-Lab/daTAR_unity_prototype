using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using SimpleSQL;

[CustomEditor(typeof(SimpleSQLManager_WithSystemData))]
public class SimpleSQLManagerInspector_WithSystemData : SimpleSQLManagerInspector
{
    protected override void SetTarget()
    {
        _manager = (SimpleSQLManager_WithSystemData)target;
    }
}