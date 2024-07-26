using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChartSetting : MonoBehaviour
{
    public VisBlockManager visBlockManager;
    public Transform settingTemplate;
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
