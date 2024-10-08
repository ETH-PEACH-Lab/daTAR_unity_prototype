using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ConstructorView : MonoBehaviour
{
    public Transform container;
    public Transform settingTemplate;
    public Transform comitBtn;
    public CustomBlockManager blockManager;

    public Dictionary<string, string> settings {  get; set; }
    private Dictionary<string, string> selectedSettings = new Dictionary<string, string>();
    public string comitName { get; set; }

    private List<TMP_Dropdown> columnSettings = new List<TMP_Dropdown>();
    private List<Collection> collectionOptions = new List<Collection>();
    private Collection selectedCollection = null;


    public void populate()
    {
        clear();

        foreach (KeyValuePair<string,string> setting in settings)
        {
            Transform s = Instantiate(settingTemplate, container);
            s.name = setting.Key;
            s.Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = setting.Key;
            s.gameObject.SetActive(true);

            s.Find("Dropdown").Find("placeholder").GetComponent<TMPro.TextMeshProUGUI>().text = "choose " + setting.Value;

            TMP_Dropdown dropdown = s.Find("Dropdown").GetComponent <TMP_Dropdown>();
            dropdown.ClearOptions();

            switch(setting.Value)
            {
                case "collection":
                    List<Collection> collections = CollectionManager.Instance.getAllCollections();
                    collectionOptions = collections;
                    dropdown.options.Add(new TMP_Dropdown.OptionData() { text = " " });
                    foreach (Collection collection in collections)
                    {
                        dropdown.options.Add(new TMP_Dropdown.OptionData() { text = collection.Name });                                               
                    }
                    dropdown.onValueChanged.AddListener(delegate {
                        onSelectCollection(dropdown, setting.Key);
                    });
                   
                    break;
                case "column":
                    dropdown.name = setting.Key;
                    columnSettings.Add(dropdown);
                    break;
            }
        }

        comitBtn.Find("name").GetComponent<TMPro.TextMeshProUGUI>().text = comitName;
        comitBtn.GetComponent<Button>().onClick.AddListener(() => blockManager.executeConstructor(selectedSettings));
    }

    private void onSelectCollection(TMP_Dropdown changedDropdown, string settingName)
    {
        if(changedDropdown.value < 1) { return; }

        changedDropdown.transform.Find("placeholder").gameObject.SetActive(false);

        selectedCollection = collectionOptions[changedDropdown.value - 1];

        selectedSettings[settingName] = selectedCollection.Name;

        foreach (TMP_Dropdown dropdown in columnSettings)
        {
            string[] columnNames = selectedCollection.Attributes.Split(", ");

            dropdown.ClearOptions();
            foreach (string c in columnNames)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData() { text = c});
                dropdown.onValueChanged.AddListener(delegate {
                    onSelectedColumn(dropdown, dropdown.name);
                });
            }
            
        }
    }

    private void onSelectedColumn(TMP_Dropdown changedDropdown, string settingName)
    {
        string[] options = selectedCollection.Attributes.Split(", ");
        selectedSettings[settingName] = options[changedDropdown.value];
    }
    private void clear()
    {
        settingTemplate.gameObject.SetActive(false);

        for (int i = 0; i < container.childCount; i++)
        {
            if ( i > 0)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }
}
