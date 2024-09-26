using System.Collections;
using System.Collections.Generic;

public interface IChart //implemented by PieChartManager, CardChartManager, ScatterPlotManager, BarChartManager
{
    /// <summary>
    /// collection summary associate with data table to visulize
    /// </summary>
    public Collection collection { get; set; }

    /// <summary>
    /// needed if chart visualizes single data point (pie chart, card chart)
    /// </summary>
    public int selectedRowId { get; set; }

    /// <summary>
    /// visualize single data point (pie chart, card chart)
    /// </summary>
    public void populateChart(string rowId);

    /// <summary>
    /// visualize entire data table associated to given collection
    /// </summary>
    public void populateChart(Collection collection);

    /// <summary>
    /// visualize entire data table without specifing associated collection, needed when connected to a AR data table block
    /// </summary>
    public void populateChart(List<Dictionary<string, string>> table);

    /// <summary>
    /// returns settings specific to the type of data visulaisation, key = setting name , value = setting type (tabel_column_none or table_column)
    /// </summary>
    public Dictionary<string, string> getSettings();
    /// <summary>
    /// applies setting on chart and updates the data visualisation
    /// </summary>
    public void applySetting(string settingName, string value);
}
