using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System;

public class CargaURLMapa : MonoBehaviour
{
    public ImageFormat format = ImageFormat.png;
    public RawImage image;
    //public GoogleMapLocation centerLocation;
    public bool isUpdating;
    public int zoom;
    //public string size;
    public string color;
    public string key;
    private float w;
    private float h;


    public void CargaMapa()
    {
        if (!isUpdating)
        {
            StartCoroutine(GetLocation());
            isUpdating = !isUpdating;
        }

    }


    IEnumerator GetLocation()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield return new WaitForSeconds(3);

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 3;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            //gpsOut.text = "Timed out";
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            //gpsOut.text = "Unable to determine device location";
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            //gpsOut.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + 100f + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

            try
            {
                w = image.rectTransform.rect.width;
                h = image.rectTransform.rect.height;
                var querryString =
                    "https://maps.googleapis.com/maps/api/staticmap?" +
                    "center=" + Input.location.lastData.latitude + "," + Input.location.lastData.longitude +
                    "&zoom=" + zoom.ToString() +
                    "&size=" + w + "x" + h +
                    "&maptype=roadmap" +
                    "&markers=color:" + color + "%7Clabel:A%7C"+ Input.location.lastData.latitude + "," + Input.location.lastData.longitude +
                    "&key="+key;
                Debug.Log(querryString);
                var request = new WWW(querryString);
                Debug.Log(request.url.ToString());
                //GetComponent<Renderer>().material.mainTexture = request.texture;
                image.texture = request.texture;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        // Stop service if there is no need to query location updates continuously
        isUpdating = !isUpdating;
        Input.location.Stop();
    }
}
