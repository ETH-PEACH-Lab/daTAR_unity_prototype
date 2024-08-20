using System.Collections;
using System.Collections.Generic;
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
    public GameObject knnBlock;

    private string nameVisBlock = "visBlock01"; //change to array with block names for every type of visualization
    private string nameTableBlock = "tableBlock01";
    private string orderBy = "ORDERBY";
    private string kNN = "kNN";

    List<GameObject> ARObjects = new List<GameObject>();


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
            Debug.Log("tracking: " + trackedImage.referenceImage.name);
            
                if (trackedImage.referenceImage.name == nameVisBlock)
                {
                    var newPrefab = Instantiate(visBlock, trackedImage.transform.parent);
                    newPrefab.name = trackedImage.referenceImage.name;
                    newPrefab.SetActive(true);
                    Debug.Log("vis node");
                    ARObjects.Add(newPrefab);

                } else if(trackedImage.referenceImage.name == nameTableBlock)
                {
                    var newPrefab = Instantiate(tableBlock, trackedImage.transform.parent);
                    newPrefab.name = trackedImage.referenceImage.name;
                    newPrefab.SetActive(true);
                    Debug.Log("table node");
                    ARObjects.Add(newPrefab);
                } else if (trackedImage.referenceImage.name == orderBy)
                {
                var newPrefab = Instantiate(orderByBlock, trackedImage.transform.parent);
                newPrefab.name = trackedImage.referenceImage.name;
                newPrefab.SetActive(true);
                Debug.Log("order by node");
                ARObjects.Add(newPrefab);
                } else if (trackedImage.referenceImage.name == kNN)
                {
                var newPrefab = Instantiate(knnBlock, trackedImage.transform.parent);
                newPrefab.name = trackedImage.referenceImage.name;
                //set tracked img id for later back end calls
                newPrefab.GetComponent<CustomBlockManager>().imgId = kNN;
                newPrefab.GetComponent<CustomBlockManager>().initConstructorView();
                newPrefab.SetActive(true);
                Debug.Log("knn node");
                ARObjects.Add(newPrefab);
            }
            else
                {
                    var newPrefab = Instantiate(objectMarker, trackedImage.transform.parent);
                //Debug.Log(gameObject.transform.position + " ppp " + trackedImage.transform.parent.name);

                    newPrefab.SetActive(true);
                    newPrefab.name = trackedImage.referenceImage.name;
                //newPrefab.transform.position = trackedImage.transform.position;

                ARObjects.Add(newPrefab);
            }
            
        }

        //Update tracking position
        List<string> trackedNames = new List<string>();
        foreach (var trackedImage in eventArgs.updated)
        {
            trackedNames.Add(trackedImage.referenceImage.name);

            foreach (var gameObject in ARObjects)
            {
                if (gameObject.name == trackedImage.referenceImage.name && trackedImage.trackingState == TrackingState.Tracking)
                {
                    Debug.Log("marker " + trackedImage.referenceImage.name);
                    gameObject.SetActive(true);
                    gameObject.transform.localPosition = trackedImage.transform.localPosition;
                    //y rotation for table surfaces
                    float rotationImage = trackedImage.transform.localEulerAngles.y;
                    float rotationObject = gameObject.transform.localEulerAngles.y;
                    //Debug.Log(rotationImage + ",rot " + rotationObject);
                    //Debug.Log(Mathf.Asin(rotationImage) + ",rot2 " + Mathf.Asin(rotationObject));
                    if (Mathf.Abs(rotationImage - rotationObject) > 10)
                    {
                        //gameObject.transform.rotation = Quaternion.Euler(0, rotationImage, 0);
                        gameObject.transform.localRotation = trackedImage.transform.localRotation;
                    }

                }
                else if(gameObject.name == trackedImage.referenceImage.name && trackedImage.trackingState == TrackingState.Limited)
                {
                    //Destroy(gameObject);
                    Debug.Log("marker limited " + gameObject.name);
                    gameObject.SetActive(false);
                }
            }
        }

        foreach(var gO in ARObjects)
        {
            if (!trackedNames.Contains(gO.name))
            {
                //Destroy(gO);
                gO.SetActive(false);
                Debug.Log("marker not contained " + gO.name);
            }
        }

    }
}
