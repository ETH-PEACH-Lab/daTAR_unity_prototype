using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomBlockManager : MonoBehaviour
{
    public ConstructorView constructorView;
    public MethodView methodView;
    public GameObject outDataNode;
    public string imgId { get; set; }
    //custom variables would be on remote server
    private int k = 2;
    private List<string> featureNames = new List<string>();
    private List<Tuple<float[], string>> trainingData;
    private List<Dictionary<string, string>> data_out = new List<Dictionary<string, string>>();

    private List<Dictionary<string, string>> data_userInput = new List<Dictionary<string, string>>();

    public void initConstructorView()
    {
        Debug.Log("imgId " + imgId);
        Dictionary<string, string> paramsConstructor = new Dictionary<string, string>() { //map storing the settings specs. key = setting name , value = setting type (or maybe better setting options as string[]) later get from backend
        {"Training Data","collection" },
        {"Class Label", "column" } };

        string comitName = "Train"; //name of the btn user sees to submit the settings 

        constructorView.comitName = comitName;
        constructorView.settings = paramsConstructor;
        constructorView.populate();
    }

    public void executeConstructor(Dictionary<string, string> selectedParams)
    {
        //execute custom knn code later on remote server
        if (selectedParams != null && selectedParams.ContainsKey("Training Data") && selectedParams.ContainsKey("Class Label"))
        {
            Collection selectedCollection = CollectionManager.Instance.getCollection(selectedParams["Training Data"]);
            string tableName = selectedCollection.Name + selectedCollection.Id;
            List<Dictionary<string, string>> dataTable = CollectionManager.Instance.getDataTable(tableName);
            data_out = dataTable;

            trainingData = new List<Tuple<float[], string>>();

            foreach (Dictionary<string, string> row in dataTable)
            {
                List<float> features = new List<float>();
                
                foreach (KeyValuePair<string, string> pair in row)
                {

                    if (pair.Key != selectedParams["Class Label"] && pair.Key != "id")
                    {
                        float feature = 0;
                        if(float.TryParse(pair.Value, out feature))
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
                trainingData.Add(new Tuple<float[], string>(features.ToArray(), row[selectedParams["Class Label"]]));
            }

            initMethodView();
        }
    }

    public void initMethodView()
    {
        Dictionary<string, string[]> paramsMethod = new Dictionary<string, string[]>() //map storing the parameters for the custom method. key = parameter name, value = options || user defined data input(single point or table)
        {
            {"k parameter",new string[3] {"2","3","4" } },
            {"choose data point to classify", new string[1] {"user input"} }
        };

        methodView.settings = paramsMethod;
        methodView.executeName = "Classify"; //name of the button to execute the method

        methodView.populate();

        methodView.transform.gameObject.SetActive(true);
        outDataNode.SetActive(true);
    }

    public void executeMethod(Dictionary<string, string> selectedParams)
    {
        //custom code for knn algorithm later executed on backend
       if(selectedParams.ContainsKey("k parameter"))
        {
            int.TryParse(selectedParams["k parameter"], out k);
        }
       List<Dictionary<string, string>> resultData = new List<Dictionary<string, string>>();
       foreach (Dictionary<string, string> dataPoint in data_userInput)
        {
            List<float> features = new List<float>();
            foreach (string f in featureNames)
            {
                features.Add(float.Parse(dataPoint[f])); //convert user input into suitabel data format for algo
            }

            var distances = trainingData.Select(t =>
            new
            {
                Distance = CalculateDistance(features.ToArray(), t.Item1),
                Label = t.Item2
            })
            .OrderBy(t => t.Distance)
            .Take(k);

            string classLabel = distances
                .GroupBy(t => t.Label)
                .OrderByDescending(g => g.Count())
                .First().Key;
            Debug.Log("classified as " + classLabel);
            Dictionary<string, string> classifiedPoint = new Dictionary<string, string>() //summary view of the data output can be customized
            {
                {"data point", dataPoint[featureNames[0]]},
                {"class label", classLabel } 
            };
            resultData.Add(classifiedPoint);
        }

       methodView.displayResults(resultData);

       
    }

    private double CalculateDistance(float[] features1, float[] features2)
    {
        double sum = 0;
        Debug.Log("classified array l " +  features1.Length + " " +  features2.Length);
        for (int i = 0; i < features1.Length && i < features2.Length; i++)
        {
            sum += Math.Pow(features1[i] - features2[i], 2);
            Debug.Log("classified feature " + features1[i] + " " + features2[i]);
        }
        return Math.Sqrt(sum);
    }
    public void addUserInput(List<Dictionary<string, string>> userInput)
    {
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
                {"data point", row[featureNames[0]]},
                {"class label", "-" } //leave empty first then calcualte when executing custom method i.e. classify
            };
            methodView.displayData(dataPoint);
        }
    }

    public List<Dictionary<string, string>> getOutData() //also needs collection name for getting unit highlights
    {
        return data_out;
    }
}
