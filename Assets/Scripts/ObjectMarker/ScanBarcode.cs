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
        //ProductList productList = JsonUtility.FromJson<ProductList>(jsonFile.text);
        productList = JsonConvert.DeserializeObject<ProductList>(jsonFile.text);
        Debug.Log("load json " + productList.products[0].code);
        Debug.Log("load json 2 " + productList.products[0].nutriments);
    }
    public void activateScanner()
    {
        BarcodeCam.Instance.activate(this);
    }

    public void sendResult(string bc)
    {
        barcode = bc;
        Debug.Log("barcode res code " +  barcode);
        Product foundProduct = productList.products.Find(product => product.code == barcode);
        Debug.Log("barcode res 22");

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

    public void setAttributes(Collection collection)
    {
        attributes = collection.Attributes.Split(", ");
    }
}

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
