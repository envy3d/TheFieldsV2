using UnityEngine;

public class RandomizedTransform : MonoBehaviour
{
	
	public float xRotationRange = 0;
	public float yRotationRange = 0;
	public float zRotationRange = 0;
	
	public float xPositionRange = 0;
	public float yPositionRange = 0;
	public float zPositionRange = 0;
	
	void Start() 
	{		
		gameObject.transform.eulerAngles += new Vector3((Random.Range (-xRotationRange,xRotationRange)),(Random.Range (-yRotationRange,yRotationRange)),(Random.Range (-zRotationRange,zRotationRange)));
		gameObject.transform.position += new Vector3((Random.Range (-xPositionRange,xPositionRange)),(Random.Range (-yPositionRange,yPositionRange)),(Random.Range (-zPositionRange,zPositionRange)));
		
	}
}
