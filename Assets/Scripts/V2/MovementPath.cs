using UnityEngine;
using System;

[Serializable]
public class MovementPath
{
    public float movementTime = 1.0f;
    public AnimationCurve heightOverTime = AnimationCurve.Linear(0, 0, 1, 0);
    public AnimationCurve distanceOverTime = AnimationCurve.Linear(0, 0, 1, 1);

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float elapsedTime = 0;

    public MovementPath()
    {
        startPosition = new Vector3();
        endPosition = new Vector3();
        elapsedTime = 0;
    }

    public void Setup(Vector3 startPosition, Vector3 endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        elapsedTime = 0;
    }

    public Vector3 UpdatePosition()
    {
        elapsedTime += Time.deltaTime;
        float normalizedTime = elapsedTime / movementTime;                          // The animation curves are evaluated from 0 to 1 so the elapsed time must be normalized.
        float displacementAmount = distanceOverTime.Evaluate(normalizedTime);       // Get the percentage of change along the x or z axis since elapsedTime = 0.
        float x = Mathf.Lerp(startPosition.x, endPosition.x, displacementAmount);   // Use the percentage of change to get the actual position.
        float z = Mathf.Lerp(startPosition.z, endPosition.z, displacementAmount);

        // Get the current height (changes linearly over time) and add current heightOverTime (changes based on curve e.g. parabola).
        float y = Mathf.Lerp(startPosition.y, endPosition.y, normalizedTime) + heightOverTime.Evaluate(normalizedTime);

        return new Vector3(x, y, z);
    }

    public bool IsFinishedUpdating()
    {
        if (elapsedTime >= movementTime)
            return true;

        return false;
    }
}
