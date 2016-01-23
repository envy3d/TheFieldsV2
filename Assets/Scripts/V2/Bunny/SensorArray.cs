using UnityEngine;
using System;

namespace Bunny
{
    public class SensorArray : MonoBehaviour
    {
        [Range(0,10)]
        public float sensorDistanceFromCenter = 1;
        public float sensorRadius = 0.1f;

        [NonSerialized]
        public Sensor centerSensor;
        [NonSerialized]
        public Sensor northSensor;
        [NonSerialized]
        public Sensor north2Sensor;
        [NonSerialized]
        public Sensor eastSensor;
        [NonSerialized]
        public Sensor east2Sensor;
        [NonSerialized]
        public Sensor southSensor;
        [NonSerialized]
        public Sensor south2Sensor;
        [NonSerialized]
        public Sensor westSensor;
        [NonSerialized]
        public Sensor west2Sensor;

        private Vector3 prevGOPosition;
        private Vector3 prevGOScale;

        void Awake()
        {
            prevGOPosition = transform.position;
            prevGOScale = transform.localScale;
            InitializeSensors();
        }

        void Update()
        {
            if (prevGOScale != transform.localScale)
            {
                UpdateSensorPositions();
                UpdateSensorRadii();
            }
            prevGOScale = transform.localScale;
            if (prevGOPosition.y != transform.position.y)
            {
                UpdateSensorPositions();
            }
            prevGOPosition = transform.position;
        }

        public void InitializeSensors()
        {
            centerSensor = new GameObject("Center Sensor").AddComponent<Sensor>();
            northSensor = new GameObject("North Sensor").AddComponent<Sensor>();
            north2Sensor = new GameObject("North2 Sensor").AddComponent<Sensor>();
            eastSensor = new GameObject("East Sensor").AddComponent<Sensor>();
            east2Sensor = new GameObject("East2 Sensor").AddComponent<Sensor>();
            southSensor = new GameObject("South Sensor").AddComponent<Sensor>();
            south2Sensor = new GameObject("South2 Sensor").AddComponent<Sensor>();
            westSensor = new GameObject("West Sensor").AddComponent<Sensor>();
            west2Sensor = new GameObject("West2 Sensor").AddComponent<Sensor>();

            centerSensor.transform.parent = transform;
            northSensor.transform.parent = transform;
            north2Sensor.transform.parent = transform;
            eastSensor.transform.parent = transform;
            east2Sensor.transform.parent = transform;
            southSensor.transform.parent = transform;
            south2Sensor.transform.parent = transform;
            westSensor.transform.parent = transform;
            west2Sensor.transform.parent = transform;

            UpdateSensorPositions();
            UpdateSensorRadii();
        }

        public void UpdateSensors()
        {
            UpdateSensorPositions();
            UpdateSensorRadii();
        }

        public void UpdateSensorPositions()
        {
            // Scaling the game object will affect the position of the sensors.
            // The scale corrections allow the game object to be scaled without
            // having to tweak sensor positions.
            float xScaleCorrection = 1 / transform.localScale.x;
            float yScaleCorrection = 1 / transform.localScale.y;
            float zScaleCorrection = 1 / transform.localScale.z;

            // yPosCorrection is the distance the sensors need to be moved in along 
            // the y-axis so that the sensor are at y = 0 in world space.
            float yPosCorrection = -transform.position.y * yScaleCorrection;

            centerSensor.transform.localPosition = new Vector3(0, yPosCorrection, 0);
            northSensor.transform.localPosition = new Vector3(0, yPosCorrection, sensorDistanceFromCenter * zScaleCorrection);
            north2Sensor.transform.localPosition = new Vector3(0, yPosCorrection, sensorDistanceFromCenter * 2 * zScaleCorrection);
            eastSensor.transform.localPosition = new Vector3(sensorDistanceFromCenter * xScaleCorrection, yPosCorrection, 0);
            east2Sensor.transform.localPosition = new Vector3(sensorDistanceFromCenter * 2 * xScaleCorrection, yPosCorrection, 0);
            southSensor.transform.localPosition = new Vector3(0, yPosCorrection, -sensorDistanceFromCenter * zScaleCorrection);
            south2Sensor.transform.localPosition = new Vector3(0, yPosCorrection, -sensorDistanceFromCenter * 2 * zScaleCorrection);
            westSensor.transform.localPosition = new Vector3(-sensorDistanceFromCenter * xScaleCorrection, yPosCorrection, 0);
            west2Sensor.transform.localPosition = new Vector3(-sensorDistanceFromCenter * 2 * xScaleCorrection, yPosCorrection, 0);
        }

        public void UpdateSensorRadii()
        {
            // Sphere collider radius is affected by the scale axis with the highest absolute value.
            // Find that value then use that correction to calculate the final scale.
            float radius = sensorRadius / Mathf.Max(Math.Abs(transform.localScale.x),
                                                    Math.Abs(transform.localScale.y),
                                                    Math.Abs(transform.localScale.z));

            centerSensor.transform.localScale = new Vector3(radius, radius, radius);
            northSensor.transform.localScale = new Vector3(radius, radius, radius);
            north2Sensor.transform.localScale = new Vector3(radius, radius, radius);
            eastSensor.transform.localScale = new Vector3(radius, radius, radius);
            east2Sensor.transform.localScale = new Vector3(radius, radius, radius);
            southSensor.transform.localScale = new Vector3(radius, radius, radius);
            south2Sensor.transform.localScale = new Vector3(radius, radius, radius);
            westSensor.transform.localScale = new Vector3(radius, radius, radius);
            west2Sensor.transform.localScale = new Vector3(radius, radius, radius);
        }

    }
}

