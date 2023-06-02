using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions;
using System;
using UnityEngine.UI;

public class GeoManager : MonoBehaviour
{
    [SerializeField]
    private AREarthManager earthManager;

    public Text texto;
    public Text tLat;
    public Text tLon;
    public Text tAlt;

    [Serializable]
    public struct GeospatialObject
    {
        public GameObject ObjectPrefab;
        public EarthPosition EarthPosition;
    }

    [Serializable]
    public struct EarthPosition
    {
        public double Latitude;
        public double Longitude;
        public double Altitude;
    }

    [SerializeField]
    private List<GeospatialObject> geospatialObjects = new List<GeospatialObject>();

    [SerializeField]
    private ARAnchorManager arAnchorManager;

    private void Start()
    {
        VerifyGeospatialSupport();
    }

    private void VerifyGeospatialSupport(){
        var result = earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
        switch (result)
        {
            case FeatureSupported.Supported:
                Debug.Log("Listo para usar VPS");
                texto.text = "Listo para usar VPS";
                PlaceObjects();
                break;
            case FeatureSupported.Unknown:
                Debug.Log("Desconocido");
                texto.text = "Desconocido";
                PlaceObjects();
                break;
            case FeatureSupported.Unsupported:
                Debug.Log("VPS no soportado");
                texto.text = "VPS no soportado";
                break;
            default:
                break;
        }

    }

    private void PlaceObjects()
    {
        if (earthManager.EarthTrackingState == TrackingState.Tracking)
        {
            var geospatialPose = earthManager.CameraGeospatialPose;
            tLat.text = geospatialPose.Latitude.ToString();
            tLon.text = geospatialPose.Longitude.ToString();
            tAlt.text = geospatialPose.Altitude.ToString();

            //texto.text = "Coord:" + geospatialPose.Latitude.ToString() + ", " + geospatialPose.Longitude.ToString() + ", " + geospatialPose.Altitude.ToString();
            foreach (var obj in geospatialObjects)
            {
                var eartPosition = obj.EarthPosition;
                var objAnchor = ARAnchorManagerExtensions.AddAnchor(arAnchorManager,
                    eartPosition.Latitude, eartPosition.Longitude, eartPosition.Altitude, Quaternion.identity);
                Instantiate(obj.ObjectPrefab, objAnchor.transform);
                //texto.text = "Objeto Instanciado en: " + eartPosition.Latitude + ", " + eartPosition.Longitude + ", " + eartPosition.Altitude;
            }

        }
        else if (earthManager.EarthTrackingState == TrackingState.None)
        {
            Invoke("PlaceObjects", 5.0f);
            texto.text += " Tracking None\n";
        }
    }
}
