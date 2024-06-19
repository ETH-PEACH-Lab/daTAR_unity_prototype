using System.Collections;
using System.Collections.Generic;

public interface IChart
{
    public string collectionName { get; set; }
    public void populateChart(List<string> data);
}
