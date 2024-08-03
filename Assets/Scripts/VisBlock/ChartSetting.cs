using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChartSetting : MonoBehaviour
{
    public VisBlockManager visBlockManager;
    public Transform settingTemplate;

    //private Dictionary<string, List<string>> options = new Dictionary<string, List<string>>();
    public void loadSettings()
    {
        clear();
        Dictionary<string,string> settings = visBlockManager.getSettings();
        if (settings != null)
        {
            foreach (KeyValuePair<string,string> setting in settings)
            {
                Transform newSetting = Instantiate(settingTemplate, transform);
                newSetting.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = setting.Key;
                newSetting.gameObject.SetActive(true);
                if (setting.Value == "tabel_column")
                {
                    List<string> options = visBlockManager.getTableColumns();
                    
                    SettingDropdown dropdown = newSetting.GetChild(1).GetComponent<SettingDropdown>();
                    dropdown.settingName = setting.Key;
                    dropdown.setOptions(options);

                    
                }

            }
        }

    }

    private void clear()
    {
        settingTemplate.gameObject.SetActive(false);
        for (int i = 0; i < transform.childCount; i++)
        {
            if(i> 0)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
