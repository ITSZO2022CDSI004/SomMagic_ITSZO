using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ImageTracker : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager arTrackedImageManager;

    private VideoPlayer videoPlayer;

    private bool isImageTrackable;

    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs eventData)
    {
        foreach (var trackedImage in eventData.added)
        {
            videoPlayer = trackedImage.GetComponentInChildren<VideoPlayer>();
            videoPlayer.Play();
        }

        foreach (var trackedImage in eventData.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                if (isImageTrackable)
                {
                    isImageTrackable = true;
                    videoPlayer.gameObject.SetActive(true);
                    videoPlayer.Play();
                }
            }
            else if (trackedImage.trackingState == TrackingState.Limited)
            {
                if (isImageTrackable)
                {
                    isImageTrackable = false;
                    videoPlayer.gameObject.SetActive(false);
                    videoPlayer.Pause();
                }
            }
        }


    }

}
