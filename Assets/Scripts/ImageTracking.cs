using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracking : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject chartViewCollection;
    public GameObject outputNode;

    private string nameVisNode = "qr_code_100";

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
            
                if (trackedImage.referenceImage.name == nameVisNode || trackedImage.referenceImage.name == "marker05")
                {
                    var newPrefab = Instantiate(outputNode, trackedImage.transform.parent);
                    newPrefab.name = trackedImage.name;
                    newPrefab.SetActive(true);
                    Debug.Log("output node");
                    ARObjects.Add(newPrefab);

                }
                else
                {
                    var newPrefab = Instantiate(chartViewCollection, trackedImage.transform.parent);
                //Debug.Log(gameObject.transform.position + " ppp " + trackedImage.transform.parent.name);

                    newPrefab.SetActive(true);
                    newPrefab.name = trackedImage.name;
                //newPrefab.transform.position = trackedImage.transform.position;

                ARObjects.Add(newPrefab);
            }
            
        }

        //Update tracking position
        foreach (var trackedImage in eventArgs.updated)
        {

            foreach (var gameObject in ARObjects)
            {
                if (gameObject.name == trackedImage.name && trackedImage.trackingState == TrackingState.Tracking)
                {
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
            }
        }

    }
}
