using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class ScanBarcode : MonoBehaviour
{
    //offline data from open api https://world.openfoodfacts.org/api/v0/products/your_barcode.json
    public TextAsset jsonFile;
    private string barcode = "";
    private ProductList productList =  new ProductList();
    private string[] attributes;

    public Dictionary<string, string> dataExtraction = new Dictionary<string, string>();
    public ObjectManager objectManager;
    private void Start()
    {
        // Parse the JSON data into a list of Product objects
        productList = JsonConvert.DeserializeObject<ProductList>(jsonFile.text);
    }

    /// <summary>
    /// activated the bar code scanner for a limited amount of frames 
    /// </summary>
    public void activateScanner()
    {
        BarcodeCam.Instance.activate(this);
    }

    /// <summary>
    /// look up scanned bar code in the local json file
    /// </summary>
    /// <param name="bc">barcode scan result</param>
    public void sendResult(string bc)
    {
        barcode = bc;
        Debug.Log("barcode res code " +  barcode);
        //TODO: make api call based on barcode instead of retriving from local downloaded json file
        Product foundProduct = productList.products.Find(product => product.code == barcode);

        dataExtraction = new Dictionary<string, string>();

        if (foundProduct != null)
        {
            Debug.Log("barcode found");
            dataExtraction.Add("Name",foundProduct.product_name);
            dataExtraction.Add("NutriScore", foundProduct.nutriscore_grade);
            foreach (string attr in attributes)
            {
                if (foundProduct.nutriments.TryGetValue(attr, out string nutrientValue))
                {
                    dataExtraction.Add(attr, nutrientValue);
                }
            }
            
        }

        objectManager.dataExtraction(dataExtraction);
    }

    /// <summary>
    /// specify attributes to look for in the extracted product data (nutriments to look for)
    /// </summary>
    /// <param name="collection">collection to get attributes from</param>
    public void setAttributes(Collection collection)
    {
        attributes = collection.Attributes.Split(", ");
    }
}

//helper class to extract product data from the provided data structure of the api
public class Product
{
    public string code { get; set; }

    public string product_name { get; set; }
    public string nutriscore_grade { get; set; }
    public Dictionary<string, string> nutriments { get; set; }
}

public class ProductList
{
    public List<Product> products { get; set; }
}
