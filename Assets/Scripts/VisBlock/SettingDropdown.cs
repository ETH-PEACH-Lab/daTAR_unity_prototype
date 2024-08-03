using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingDropdown : MonoBehaviour
{
    public string settingName = "";
    public TMP_Dropdown dropdown;
    public VisBlockManager visBlockManager;

    private List<string> options = new List<string>();
    public void setOptions(List<string> op)
    {
        options = op;
        dropdown.ClearOptions();
        Debug.Log("options " + settingName);
        foreach (string option in options)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = option });
        }
    }

    public void selectOption(int index)
    {
        Debug.Log("selected " + options[index]);
        
        visBlockManager.applySetting(settingName, options[index]);
    }

}
