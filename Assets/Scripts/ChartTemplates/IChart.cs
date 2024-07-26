using System.Collections;
using System.Collections.Generic;

public interface IChart
{
    public string collectionName { get; set; }
    public int selectedRowId { get; set; }

    //public string data {get; set;}
    //for singel data point visualization
    public void populateChart(string rowId);

    //for entire collection visulization
    public void populateChart(Collection collection);

    //for dynamic data table visulaization
    public void populateChart(List<Dictionary<string, string>> table);

    //returns a dicationary storing the setting in the format of key: setting name, value: input type
    public Dictionary<string, string> getSettings();
}
