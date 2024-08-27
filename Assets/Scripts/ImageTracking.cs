using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracking : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject objectMarker;
    public GameObject visBlock;
    public GameObject tableBlock;
    public GameObject orderByBlock;
    public GameObject whereBlock;
    public GameObject knnBlock;
    public GameObject customVisBlock;

    private string[] nameVisBlocks = new string[2]{"scatter_plot","bar_chart"}; //array with block names for every type of visualization
    private string nameTableBlock = "tableBlock01";
    private string orderBy = "ORDERBY";
    private string where = "WHERE";
    private string kNN = "kNN";
    private string customVis = "customVis";

    private int cacheTrash = 50;

    Dictionary<GameObject,int> ARObjects = new Dictionary<GameObject, int>();


    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }


    // Event Handler
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //Create object based on image tracked
        foreach (var trackedImage in eventArgs.added)
        {
            string imgName = trackedImage.referenceImage.name;
            //Debug.Log("tracking: " + imgName);
            
                if (nameVisBlocks.Contains(imgName))
                {
                    var newPrefab = Instantiate(visBlock, trackedImage.transform.parent);
                    newPrefab.name = imgName;
                    newPrefab.GetComponent<VisBlockManager>().chartType = imgName;

                    newPrefab.SetActive(true);

                    ARObjects.Add(newPrefab, 0);

                } else if(imgName == nameTableBlock)
                {
                    var newPrefab = Instantiate(tableBlock, trackedImage.transform.parent);
                    newPrefab.name = imgName;
                    newPrefab.SetActive(true);
                    ARObjects.Add(newPrefab, 0);
            } else if (trackedImage.referenceImage.name == orderBy)
                {
                var newPrefab = Instantiate(orderByBlock, trackedImage.transform.parent);
                newPrefab.name = trackedImage.referenceImage.name;
                newPrefab.SetActive(true);
                ARObjects.Add(newPrefab, 0);
                }else if (trackedImage.referenceImage.name == where)
                {
                var newPrefab = Instantiate(whereBlock, trackedImage.transform.parent);
                newPrefab.name = trackedImage.referenceImage.name;
                newPrefab.SetActive(true);
                ARObjects.Add(newPrefab, 0);
                }
                else if (trackedImage.referenceImage.name == kNN)
                {
                var newPrefab = Instantiate(knnBlock, trackedImage.transform.parent);
                newPrefab.name = trackedImage.referenceImage.name;
                //set tracked img id for later back end calls
                newPrefab.GetComponent<CustomBlockManager>().imgId = kNN;
                newPrefab.GetComponent<CustomBlockManager>().initConstructorView();
                newPrefab.SetActive(true);
                ARObjects.Add(newPrefab, 0);
                } else if (trackedImage.referenceImage.name == customVis)
                {
                var newPrefab = Instantiate(customVisBlock, trackedImage.transform.parent);
                newPrefab.name = trackedImage.referenceImage.name;
                //set tracked img id for later back end calls
                newPrefab.SetActive(true);
                ARObjects.Add(newPrefab, 0);
                }
                else
                {
                    var newPrefab = Instantiate(objectMarker, trackedImage.transform.parent);
                //Debug.Log(gameObject.transform.position + " ppp " + trackedImage.transform.parent.name);

                    newPrefab.SetActive(true);
                    newPrefab.name = trackedImage.referenceImage.name;
                //newPrefab.transform.position = trackedImage.transform.position;
                ARObjects.Add(newPrefab, 0);
            }
            
        }

        //Update tracking position
        List<string> trackedNames = new List<string>();
        foreach (var trackedImage in eventArgs.updated)
        {
            trackedNames.Add(trackedImage.referenceImage.name);

            foreach (GameObject obj in ARObjects.Keys.ToList())
            {
                Debug.Log("cache " +  obj.name + " " + ARObjects[obj]);
                if (obj.name == trackedImage.referenceImage.name && trackedImage.trackingState == TrackingState.Tracking)
                {
                    //Debug.Log("marker " + trackedImage.referenceImage.name);
                    obj.SetActive(true);
                    ARObjects[obj] = 0;
                    obj.transform.localPosition = trackedImage.transform.localPosition;
                    //y rotation for table surfaces
                    float rotationImage = trackedImage.transform.localEulerAngles.y;
                    float rotationObject = obj.transform.localEulerAngles.y;
                    //Debug.Log(rotationImage + ",rot " + rotationObject);
                    //Debug.Log(Mathf.Asin(rotationImage) + ",rot2 " + Mathf.Asin(rotationObject));
                    if (Mathf.Abs(rotationImage - rotationObject) > 8)
                    {
                        //gameObject.transform.rotation = Quaternion.Euler(0, rotationImage, 0);
                        obj.transform.localRotation = trackedImage.transform.localRotation;
                    }

                }
                else if(obj.name == trackedImage.referenceImage.name)
                {
                    //Destroy(gameObject);
                    //Debug.Log("marker limited " + gameObject.name);
                    //blend out obj only after some time out after AR not visible by camera any more
                    if (ARObjects[obj] > cacheTrash)
                    {
                        obj.SetActive(false);
                    }
                    else
                    {
                        ARObjects[obj] += 1;
                    }
                    
                }
            }
        }

        foreach(KeyValuePair<GameObject, int> gO in ARObjects)
        {
            if (!trackedNames.Contains(gO.Key.name))
            {
                //Destroy(gO);
                gO.Key.SetActive(false);
                //Debug.Log("marker not contained " + gO.name);
            }
        }

    }
}
