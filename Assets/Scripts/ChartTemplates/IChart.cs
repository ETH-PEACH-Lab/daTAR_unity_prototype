using System.Collections;
using System.Collections.Generic;

public interface IChart
{
    public string collectionName { get; set; }

    //public string data {get; set;}
    public void populateChart(string rowId);

    public void populateChart(Collection collection);
}
