using UnityEngine;
using System.Collections;

public class JumpScript : MonoBehaviour
{
    public float hopTime = 0.2f;
    public float rotationTime = 0.15f;
    public AnimationCurve hopDisplacement = AnimationCurve.Linear(0,0,1,1);
    public GameObject digEffect;

    private Animator animator;
    private Quaternion prevRotation;
    private Quaternion targetRotation;
    private Vector3 prevPosition;
    private Vector3 targetPosition;
    private float time;
    //private float distance;

	void Start()
    {
        animator = GetComponent<Animator>();
        time = Mathf.Max(hopTime * 2, rotationTime * 2);
	}
	
	void Update()
    {
        time += Time.deltaTime;

        if (time <= rotationTime)
            transform.rotation = Quaternion.Slerp(prevRotation, targetRotation, time / rotationTime);
        else
            transform.rotation = targetRotation;

        if (time <= hopTime)
            transform.position = Vector3.Lerp(prevPosition, targetPosition, hopDisplacement.Evaluate(time / hopTime));
        //transform.position = Vector3.Lerp(prevPosition, targetPosition, time / hopTime);
        else
            transform.position = targetPosition;
	}

    public void Move(Vector2 moveDirection)
    {
        Vector3 rotVector = new Vector3(moveDirection.x, 0, moveDirection.y);
        time = 0;
        prevRotation = transform.rotation;
        targetRotation = Quaternion.LookRotation(rotVector.normalized);
        prevPosition = transform.position;
        targetPosition = transform.position + rotVector;
        //distance = moveDirection.magnitude;
        animator.SetTrigger("Jumping");
        ///
    }

    public void Dig()
    {
        animator.SetTrigger("Digging");
        ///
    }

    public void SpawnDigEffect()
    {
        GameObject.Instantiate(digEffect, transform.position, Quaternion.LookRotation(Vector3.up));
    }

    public void ForcePlacement(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        prevPosition = position;
        targetPosition = position;
        transform.rotation = rotation;
        prevRotation = rotation;
        targetRotation = rotation;
    }

}
