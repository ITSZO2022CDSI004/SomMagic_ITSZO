using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//ARFoundation y ARCoreExtensions
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

    public class GeoApiScript : MonoBehaviour
    {
        //Informacion de rastreo usando Geospatial API
        public AREarthManager EarthManager;
        //Inicializacion del Geospatial API y ARCore
        public GeoInit Initializer;
        //UI para la pantalla
        public Text OutputText;

        //Precisión admisible del acimut
        public double HeadingThreshold = 25;
        //Precisión admisible de la posición horizontal
        public double HorizontalThreshold = 20;

    //public double Latitude;
    //public double Longitude;
    //public GameObject ContentPrefab;
    public double Altitude;
    public double Heading;
    GameObject displayObject;

        public ARAnchorManager AnchorManager;

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


    // Start is called before the first frame update
    void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {
            string status = "";
            //Si la inicializacion falla o no se quiere rastrar, no hace nada y regresa
            if (!Initializer.IsReady || EarthManager.EarthTrackingState != TrackingState.Tracking)
            {
                return;
            }
            //Se obtienen resultados del tracking
            GeospatialPose pose = EarthManager.CameraGeospatialPose;
        //Aqui se describe el comportamiento de acuerdo a la exactitud de rastreo
        //La precisión de seguimiento es peor que el umbral (valor grande)    
        if (pose.HeadingAccuracy > HeadingThreshold ||
                 pose.HorizontalAccuracy > HorizontalThreshold)
            {
                status = "Low Tracking Accuracy";
            }
            else 
            {
                status = "High Tracking Accuracy";
                if (displayObject == null)
                {

                    Altitude = pose.Altitude - 1.5f;

                    Quaternion quaternion = Quaternion.AngleAxis(180f - (float)Heading, Vector3.up);

                    foreach (var obj in geospatialObjects)
                    {
                        var eartPosition = obj.EarthPosition;
                        ARGeospatialAnchor anchor = AnchorManager.AddAnchor(eartPosition.Latitude, eartPosition.Longitude, Altitude, quaternion);
                        if (anchor != null)
                        {
                            displayObject = Instantiate(obj.ObjectPrefab, anchor.transform);
                        }
                    }
                }
            }

            //Muestra los resultados del tracking en pantalla
            ShowTrackingInfo(status, pose);
        }

        void ShowTrackingInfo(string status, GeospatialPose pose)
        {
            OutputText.text = string.Format(
    "Latitude/Longitude: {0}°, {1}°\n" +
    "Horizontal Accuracy: {2}m\n" +
    "Altitude: {3}m\n" +
    "Vertical Accuracy: {4}m\n" +
    "Heading: {5}°\n" +
    "Heading Accuracy: {6}°\n" +
    "{7} \n"
    ,
    pose.Latitude.ToString("F6"),  //{0}
    pose.Longitude.ToString("F6"), //{1}
    pose.HorizontalAccuracy.ToString("F6"), //{2}
    pose.Altitude.ToString("F2"),  //{3}
    pose.VerticalAccuracy.ToString("F2"),  //{4}
    pose.Heading.ToString("F1"),   //{5}
    pose.HeadingAccuracy.ToString("F1"),   //{6}
    status //{7}
);
        }
    }


