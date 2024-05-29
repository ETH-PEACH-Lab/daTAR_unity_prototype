using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class multiple_tracking_instances : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;
    public GameObject virtualContentPrefab;
    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            // When a new image is detected
            Instantiate(virtualContentPrefab, newImage.transform.position, newImage.transform.rotation, newImage.transform);
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            // Update the position of the virtual content
            var virtualContent = updatedImage.transform.GetChild(0).gameObject;
            virtualContent.transform.position = updatedImage.transform.position;
            virtualContent.transform.rotation = updatedImage.transform.rotation;
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Remove the virtual content when the image is no longer tracked
            Destroy(removedImage.transform.GetChild(0).gameObject);
        }
    }
}



