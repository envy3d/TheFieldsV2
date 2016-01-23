using UnityEngine;
using System;

[Serializable]
public class AnimationPath
{
    public MovementPath movementPath;
    public RotationPath rotationPath;

    public AnimationPath()
    {
        movementPath = new MovementPath();
        rotationPath = new RotationPath();
    }

    public void Setup(Vector3 startPosition, Vector3 endPosition, Quaternion startRotation, Quaternion endRotation)
    {
        movementPath.Setup(startPosition, endPosition);
        rotationPath.Setup(startRotation, endRotation);
    }

    public Vector3 UpdatePosition()
    {
        return movementPath.UpdatePosition();
    }

    public Quaternion UpdateRotation()
    {
        return rotationPath.UpdateRotation();
    }

    public bool IsFinishedUpdating()
    {
        return movementPath.IsFinishedUpdating() && rotationPath.IsFinishedUpdating();
    }
}
