using UnityEngine;
using System.Collections;

public class TurbulentPosition : MonoBehaviour 
{
	public float 	xAmp		= 1f;
	public float 	yAmp		= 1f;
	public float 	zAmp		= 1f;
	public float 	globalAmp 	= 1f;
	public int 		octaves 	= 1;
	public float 	frequency 	= 1f;
	public bool 	randomSeed	= true;
	public float 	timeOffset	= 0f;
	public float 	xseed		= 100f;
	public float 	yseed		= 20f;
	public float 	zseed		= -100f;
	public bool		posSeed		= false;
	public float	posFreq		= 0.1f;
	public bool		currentPos	= true;
	
	public Transform positionSample;
	
	private Vector3 lastPos;
	private float 	noiseYValue;
	private Vector3 basePos = Vector3.zero;
		

	void Start () 
	{
		
		if(randomSeed)
		{
			xseed = Random.Range(-65535f,65535f);
			yseed = Random.Range(-65535f,65535f);
			zseed = Random.Range(-65535f,65535f);
		}
		
		if(octaves>8){octaves=8;}
		if(positionSample == null){positionSample=gameObject.transform.parent.transform;}
		//Debug.Log("positionSample=" + positionSample);
		if(currentPos) {basePos = transform.localPosition;}
	}
	

	void FixedUpdate () 
	{
		float posDelta = 0f;
		
		if(posSeed)
		{
			posDelta = Vector3.Distance(lastPos,positionSample.position) * posFreq;
		}
		
		noiseYValue += Time.deltaTime*frequency+posDelta;
		
		float xnoise = 0.5f - Mathf.PerlinNoise (xseed+posDelta,noiseYValue+timeOffset);
		float ynoise = 0.5f - Mathf.PerlinNoise (yseed+posDelta,noiseYValue+timeOffset);
		float znoise = 0.5f - Mathf.PerlinNoise (zseed+posDelta,noiseYValue+timeOffset);
		
		if(octaves > 1) //add in extra octaves
		{
			int octave = 1;
			
			while(octave < octaves)
			{
				xnoise += (0.5f - Mathf.PerlinNoise (xseed,noiseYValue*(Mathf.Pow(2,octave*1f)+timeOffset*octave))) * (Mathf.Pow(0.5f,octave*1f));
				ynoise += (0.5f - Mathf.PerlinNoise (yseed,noiseYValue*(Mathf.Pow(2,octave*1f)+timeOffset*octave))) * (Mathf.Pow(0.5f,octave*1f));
				znoise += (0.5f - Mathf.PerlinNoise (zseed,noiseYValue*(Mathf.Pow(2,octave*1f)+timeOffset*octave))) * (Mathf.Pow(0.5f,octave*1f));
				
				octave ++;
			}
		}
		
		transform.localPosition = basePos + new Vector3( xnoise*xAmp*globalAmp , ynoise*yAmp*globalAmp , znoise*zAmp*globalAmp );
		
		lastPos = positionSample.position;
		
	}
}