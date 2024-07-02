using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackingCollectionMode : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject arPrefab;

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
                
             var newPrefab = Instantiate(arPrefab, trackedImage.transform);
                    Debug.Log("new tracking "+ trackedImage.referenceImage.name);

             newPrefab.SetActive(true);
                    //newPrefab.transform.position = trackedImage.transform.position;

             ARObjects.Add(newPrefab);
                
            
        }

        //Update tracking position
        foreach (var trackedImage in eventArgs.updated)
        {

        }

    }
}
