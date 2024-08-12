using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MethodView : MonoBehaviour
{
    public Transform executeBtn;
    public CustomBlockManager blockManager;

    public Transform settingsContainer;
    public Transform settingTemplate;

    public Transform userInputContainer;
    public Transform dataContainer;
    public Transform dataTemplate;
    public Dictionary<string, string[]> settings { get; set; }

    private Dictionary<string, string> selectedSettings = new Dictionary<string, string>();
    public string executeName { get; set; }
    public void populate()
    {

        executeBtn.Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = executeName;
        executeBtn.GetComponent<Button>().onClick.AddListener(() => blockManager.executeMethod(selectedSettings));

        settingTemplate.gameObject.SetActive(false);

        foreach (KeyValuePair<string, string[]> setting in settings)
        {
            if (setting.Value.Length > 0)
            {
                if (setting.Value[0] == "user input")
                {
                    userInputContainer.Find("placeholder").GetComponent<TMPro.TextMeshProUGUI>().text = setting.Key;
                    selectedSettings[setting.Key] = setting.Value[0];
                }
                else
                {
                    Transform s = Instantiate(settingTemplate, settingsContainer);
                    s.name = setting.Key;
                    s.Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = setting.Key;
                    TMP_Dropdown dropdown = s.Find("dropdown").GetComponent<TMP_Dropdown>();
                    dropdown.ClearOptions();
                    foreach (string option in setting.Value)
                    {
                        dropdown.options.Add(new TMP_Dropdown.OptionData() { text = option });
                    }
                    dropdown.onValueChanged.AddListener(delegate {
                        onSelectOption(dropdown, setting.Key);
                    });
                    s.gameObject.SetActive(true);
                }
            }
        }
    }

    public void displayError(string error)
    {
        userInputContainer.Find("placeholder").GetComponent<TMPro.TextMeshProUGUI>().text = error;
    }

    public void displayData(Dictionary<string, string> data)
    {
        //asume data has exactly to features
        //to do: also add key as header text
        foreach(KeyValuePair<string, string> d in data)
        {
            Transform cell = Instantiate(dataTemplate, dataContainer);
            cell.Find("text").GetComponent<TMPro.TextMeshProUGUI>().text = d.Value;
        }

        userInputContainer.Find("placeholder").gameObject.SetActive(false);
        dataContainer.gameObject.SetActive(true);
    }

    public void displayResults(List<Dictionary<string, string>> results)
    {
        clear();
        foreach(Dictionary<string, string> d in results) { displayData(d); }
    }
    private void onSelectOption(TMP_Dropdown dropdown, string settingName)
    {
        selectedSettings[settingName] = settings[settingName][dropdown.value];
    }

    private void clear()
    {
        for (int i = 0; i < dataContainer.childCount; i++)
        {
            if( i > 1 )
            {
                Destroy( dataContainer.GetChild(i).gameObject );
            }
        }
    }
}