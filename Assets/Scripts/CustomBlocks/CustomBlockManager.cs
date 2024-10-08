using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CustomBlockManager : MonoBehaviour, IBlockManager
{
    // UI elements to render
    public ConstructorView constructorView;
    public MethodView methodView;
    public GameObject outDataNode;

    //node to connect to a data visualisation block
    public VisNode connectedVisNode = null;
    public string imgId { get; set; }
    //custom knn algo related variables would be on remote server
    private int k = 3;
    private List<string> featureNames = new List<string>();
    private string classLabel = "class label";
    private List<Tuple<float[], string>> trainingData;
    private List<Dictionary<string, string>> data_out = new List<Dictionary<string, string>>();

    private List<Dictionary<string, string>> data_userInput = new List<Dictionary<string, string>>();
    private Collection userInputCollection = new Collection();

    /// <summary>
    /// handels initialsation of UI elemts of the constructor view (i.e. for choosing constructor parameters)
    /// </summary>
    public void initConstructorView()
    {
        Debug.Log("imgId " + imgId);
        // TODO: call to api for getting constructor parameter names and types based on the imgId
        Dictionary<string, string> paramsConstructor = new Dictionary<string, string>() { //map storing the settings specs. key = setting name , value = setting type (or maybe better setting options as string[]) later get from backend
        {"Training Data","collection" },
        {"Class Label", "column" } };

        string comitName = "Train"; //name of the btn user sees to submit the settings 

        constructorView.comitName = comitName;
        constructorView.settings = paramsConstructor;
        constructorView.populate();
    }

    /// <summary>
    /// executed when user confirms selected constructor parameter loads method view at the end
    /// </summary>
    /// <param name="selectedParams">constructor parameters selceted by the user, key = param name, value = selected value</param>
    public void executeConstructor(Dictionary<string, string> selectedParams)
    {
        //TODO: send raw values for choosen parameter to back end and execute custom knn on remote server
        if (selectedParams != null && selectedParams.ContainsKey("Training Data") && selectedParams.ContainsKey("Class Label"))
        {
            //fetch data table based on the collection the user choose
            Collection selectedCollection = CollectionManager.Instance.getCollection(selectedParams["Training Data"]);
            classLabel = selectedParams["Class Label"];
            string tableName = selectedCollection.Name + selectedCollection.Id;
            List<Dictionary<string, string>> dataTable = CollectionManager.Instance.getDataTable(tableName);
            data_out.Clear();

            //add fetched data table to the knn training data
            trainingData = new List<Tuple<float[], string>>();

            foreach (Dictionary<string, string> row in dataTable)
            {
                List<float> features = new List<float>();
                //dont add data points with missing class label
                
                if (row[classLabel] != null && row[classLabel] != "")
                {
                    foreach (KeyValuePair<string, string> pair in row)
                    {

                        if (pair.Key != classLabel && pair.Key != "id")
                        {
                            float feature = 0;
                            if (float.TryParse(pair.Value, out feature))
                            {
                                features.Add(feature);
                                if (!featureNames.Contains(pair.Key))
                                {
                                    featureNames.Add(pair.Key);
                                }

                                Debug.Log("features " + pair.Key);
                            }
                        }
                    }
                    trainingData.Add(new Tuple<float[], string>(features.ToArray(), row[classLabel]));
                    data_out.Add(row);
                }
                
            }

            initMethodView();
        }
    }

    /// <summary>
    /// handels initialsation of UI elements of the method view (i.e. for choosing method parameters)
    /// </summary>
    public void initMethodView()
    {
        //TODO: get method parameters through api call based on imgId
        Dictionary<string, string[]> paramsMethod = new Dictionary<string, string[]>() //map storing the parameters for the custom method. key = parameter name, value = options || user defined data input(single point or table)
        {
            {"k Parameter",new string[5] {"3","5","7","9","11" } },
            {"Choose a data point to classify", new string[1] {"user input"} }
        };

        methodView.settings = paramsMethod;
        methodView.executeName = "Classify"; //name of the button to execute the method

        methodView.populate();

        methodView.transform.gameObject.SetActive(true);
        outDataNode.SetActive(true);
    }

    /// <summary>
    /// runs method custom script with the parameters choosen by the user
    /// </summary>
    /// <param name="selectedParams">method parameters selceted by the user, key = param name, value = selected value</param>
    public void executeMethod(Dictionary<string, string> selectedParams)
    {
        //fetch parameters for the methods selected by the user
        if (selectedParams.ContainsKey("k Parameter"))
        {
            int.TryParse(selectedParams["k Parameter"], out k);
        }

        //TODO: send raw values for choosen parameter to back end and execute custom knn on remote server
        List<Dictionary<string, string>> resultData = new List<Dictionary<string, string>>();

        Debug.Log("knn k " + k);
        foreach (Dictionary<string, string> dataPoint in data_userInput)
        {
            List<float> features = new List<float>();
            foreach (string f in featureNames)
            {
                features.Add(float.Parse(dataPoint[f])); //convert user data input into suitable format to execute the knn algo
            }
            //main part of knn algo
            var distances = trainingData.Select(t =>
            new
            {
                Distance = CalculateDistance(features.ToArray(), t.Item1),
                Label = t.Item2
            })
            .OrderBy(t => t.Distance)
            .Take(k);

            string computedLabel = distances
                .GroupBy(t => t.Label)
                .OrderByDescending(g => g.Count())
                .First().Key;
            Debug.Log("classified as " + computedLabel);
            Dictionary<string, string> classifiedPoint = new Dictionary<string, string>() //summary view of the data output can be customized
            {
                {"data point", dataPoint[featureNames[0]]},//default take first data feature to show to the user (instead of displaying all data attributes)
                {"class label", computedLabel } 
            };
            dataPoint[classLabel] = computedLabel;
            
            resultData.Add(classifiedPoint);
        }

       methodView.displayResults(resultData);
        updateVisBlock();

       
    }

    private double CalculateDistance(float[] features1, float[] features2)
    { //helper function for knn algo
        double sum = 0;
        for (int i = 0; i < features1.Length && i < features2.Length; i++)
        {
            sum += Math.Pow(features1[i] - features2[i], 2);
        }
        return Math.Sqrt(sum);
    }

    /// <summary>
    /// for setting method paramet of type data point, where user connects data point to the analyis block
    /// </summary>
    /// <param name="userInput">data table based on connected data point</param>
    /// <param name="fromCollection">associated collection summary for data table</param>
    public void addUserInput(List<Dictionary<string, string>> userInput, Collection fromCollection)
    {
        userInputCollection = fromCollection;
        foreach (Dictionary<string, string> row in userInput)
        {
            //check if user input is valid later run on backend
            foreach (string feature in featureNames)
            {
                if (!row.ContainsKey(feature))
                {
                    //non valid user input
                    methodView.displayError("data point has no feature called " +  feature);
                    return;
                }
            }
            //data point is valid
            data_userInput.Add(row);
            Dictionary<string, string> dataPoint = new Dictionary<string, string>() //summary view of the user input can be customized
            {
                {"data point", row[featureNames[0]]},//default take first data feature to show to the user (instead of displaying all data attributes)
                {"class label", "_" } //leave empty first then calcualte when executing custom method i.e. classify
            };
            methodView.displayData(dataPoint);
        }
        updateVisBlock();
    }

    /// <summary>
    /// collects all the data that the user is able to visualize by connection the analysis block to a visualisation block
    /// </summary>
    /// <returns> union of data to visualize in data table format</returns>
    public List<Dictionary<string, string>> getOutData() 
    {
        //knn block returns union of training data and user input data (points to classify)
        List<Dictionary<string, string>> union = new List<Dictionary<string, string>>();
        foreach (Dictionary<string, string> d in data_out)
        {
            d["id"] = "300";//quick fix for test
            union.Add(d);
        }
        foreach (Dictionary<string,string> d in data_userInput)
        {
            union.Add(d);
        }
        
        return union;
    }

    public Collection getCollection() //also needs collection name for getting unit highlights
    {
        return userInputCollection;
    }

    public void updateVisBlock()
    {
        if (connectedVisNode != null)
        {
            connectedVisNode.updateDataTable(getOutData(), getCollection());
        }
    }

    public void initVisBlock()
    {
        if (connectedVisNode != null)
        {
            connectedVisNode.setDataTable(getOutData(), getCollection());
        }
    }
}
