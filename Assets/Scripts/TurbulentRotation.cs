using UnityEngine;
using System.Collections;

public class TurbulentRotation : MonoBehaviour 
{
	public float 	xAmp		= 60f;
	public float 	yAmp		= 60f;
	public float 	zAmp		= 60f;
	public float 	globalAmp 	= 1f;
	public int 		octaves 	= 1;
	public float 	frequency 	= 1f;
	public bool 	randomSeed	= true;
	public float 	timeOffset	= 0f;
	public float 	xseed		= 100f;
	public float 	yseed		= 20f;
	public float 	zseed		= -100f;
	public bool		posSeed		= false;
	public float	posFreq		= 1f;
	public bool		currentRot	= true;
	
	Vector3 lastPos;
	float noiseYValue;
	Vector3 baseRot = Vector3.zero;
		
	// Use this for initialization
	void Start () 
	{
		
		if(randomSeed)
		{
			xseed = Random.Range(-65535f,65535f);
			yseed = Random.Range(-65535f,65535f);
			zseed = Random.Range(-65535f,65535f);
		}
		
		if(octaves>8){octaves=8;}
		if(currentRot)
		{
			baseRot = transform.rotation.eulerAngles;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		float posOffset = 0f;
		
		if(posSeed)
		{
			posOffset = Vector3.Distance(lastPos,transform.position) * posFreq;
		}
		
		noiseYValue += Time.deltaTime*frequency+posOffset;
		
		float xnoise = 0.5f - Mathf.PerlinNoise (xseed+posOffset,noiseYValue+timeOffset);
		float ynoise = 0.5f - Mathf.PerlinNoise (yseed+posOffset,noiseYValue+timeOffset);
		float znoise = 0.5f - Mathf.PerlinNoise (zseed+posOffset,noiseYValue+timeOffset);
		
		if(octaves > 1)
		{
			int octave = 1;
			
			while(octave < octaves)
			{
				
				xnoise += (0.5f - Mathf.PerlinNoise (xseed,noiseYValue*(Mathf.Pow(2,octave*1f)+timeOffset))) * (Mathf.Pow(0.5f,octave*1f));
				ynoise += (0.5f - Mathf.PerlinNoise (yseed,noiseYValue*(Mathf.Pow(2,octave*1f)+timeOffset))) * (Mathf.Pow(0.5f,octave*1f));
				znoise += (0.5f - Mathf.PerlinNoise (zseed,noiseYValue*(Mathf.Pow(2,octave*1f)+timeOffset))) * (Mathf.Pow(0.5f,octave*1f));
				
				octave ++;
			}
		}
		
		transform.localEulerAngles = baseRot + new Vector3(xnoise*xAmp*globalAmp,ynoise*yAmp*globalAmp,znoise*zAmp*globalAmp);
		
		lastPos = transform.position;
		
	}
}
