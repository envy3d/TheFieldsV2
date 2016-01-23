using UnityEngine;
using System.Collections;

public class ConstrainToAnother : MonoBehaviour 
{
	public Transform positionTarget = null;
	public Transform rotationTarget = null;
	public bool useSmooth = false;
	public Vector3 posOffset = Vector3.zero;
	public bool useCurrnentOffset = true;
	public bool useRelativeOffset = false;
	public bool usePredictPosition = false;
	public float posSmooth = 1f;
	public float rotSmooth = 1f;
	public float velocityLead = 1f;
	
	private Vector3 targetPosition;
	private Vector3 velocityref;
	private Vector3 velocity;
	private Vector3 relativeOffset;
	private Vector3 previousPos;
	
	// Update is called once per frame
	void Start ()
	{
		if (useCurrnentOffset == true)
		{
			posOffset = transform.position - positionTarget.position;
		}
	}
	void Update () 
	{
		if (positionTarget==null)positionTarget=transform;
		if (rotationTarget==null)rotationTarget=transform;
		relativeOffset = posOffset;
		targetPosition = positionTarget.position;
		velocity = targetPosition - previousPos;
		
		if (useRelativeOffset) 
		{ 
			relativeOffset = positionTarget.forward*relativeOffset.z+positionTarget.up*relativeOffset.y+positionTarget.right*relativeOffset.x; 
		}
		
		if (usePredictPosition)
		{
			targetPosition = positionTarget.position+velocity*velocityLead;
		}
		
		if (!useSmooth)
		{
			if (positionTarget != null)
			{
				//follow positionTarget
				transform.position = Vector3.Lerp ( transform.position , targetPosition + relativeOffset, Time.deltaTime *10/posSmooth );
			}
			
			if (rotationTarget != null)
			{
				//align rotationTarget
				transform.rotation = Quaternion.Lerp ( transform.rotation , rotationTarget.rotation, Time.deltaTime * 10/rotSmooth );
			}
		}
		else
		{
			if (positionTarget != null)
			{
				//follow positionTarget
				transform.position = Vector3.SmoothDamp ( transform.position , targetPosition + relativeOffset, ref velocityref, Time.deltaTime * 10*posSmooth );
			}
			
			if (rotationTarget != null)
			{
				//align rotationTarget
				transform.rotation = Quaternion.Lerp ( transform.rotation , rotationTarget.rotation, Time.deltaTime * 10/rotSmooth );
			}
		}
		
		previousPos = positionTarget.position;
	}
	
}
