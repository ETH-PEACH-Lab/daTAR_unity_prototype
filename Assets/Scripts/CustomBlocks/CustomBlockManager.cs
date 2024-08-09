using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBlockManager : MonoBehaviour
{
   public ConstructorView constructorView;
    public string imgId { get; set; }
    public void initConstructorView()
    {
        Debug.Log("imgId " + imgId);
        Dictionary<string, string> settings  = new Dictionary<string, string>() { //map storing the settings specs. key = setting name , value = setting type
        {"Training Data","collection" },
        {"Class Label", "column" } };

        string comitName = "Train"; //name of the btn user sees to submit the settings

        constructorView.comitName = comitName;
        constructorView.settings = settings;
        constructorView.populate();
    }
}
