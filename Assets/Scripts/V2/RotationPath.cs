using UnityEngine;
using System;

[Serializable]
public class RotationPath
{
    public float rotationTime = 1.0f;
    public AnimationCurve rotationOverTime = AnimationCurve.Linear(0, 0, 1, 1);

    private Quaternion startRotation;
    private Quaternion endRotation;
    private float elapsedTime = 0;

    public RotationPath()
    {
        startRotation = new Quaternion();
        endRotation = new Quaternion();
        elapsedTime = 0;
    }

    public void Setup(Quaternion startRotation, Quaternion endRotation)
    {
        this.startRotation = startRotation;
        this.endRotation = endRotation;
        elapsedTime = 0;
    }

    public Quaternion UpdateRotation()
    {
        elapsedTime += Time.deltaTime;
        float normalizedTime = elapsedTime / rotationTime;                  // The animation curves are evaluated from 0 to 1 so the elapsed time must be normalized.
        float rotationAmount = rotationOverTime.Evaluate(normalizedTime);   // Get the percentage of rotation change since elapsedTime = 0.

        // Use the percentage of change to get the actual rotation.
        return Quaternion.Slerp(startRotation, endRotation, rotationAmount);
    }

    public bool IsFinishedUpdating()
    {
        if (elapsedTime >= rotationTime)
            return true;

        return false;
    }
}
