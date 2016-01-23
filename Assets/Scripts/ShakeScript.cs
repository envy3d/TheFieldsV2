using UnityEngine;
using System.Collections;


[RequireComponent(typeof(TurbulentPosition))]
public class ShakeScript : MonoBehaviour {

    public float shakeTime = 1;
    public AnimationCurve shakeIntensity = new AnimationCurve();

    private float time;
    private TurbulentPosition turbPosition;

	void Start () {
        time = shakeTime;
        turbPosition = GetComponent<TurbulentPosition>();
	}
	
	void Update () {
        time += Time.deltaTime;
        if (time <= shakeTime)
        {
            float value = shakeIntensity.Evaluate(time/shakeTime);
            turbPosition.globalAmp = value;
            turbPosition.frequency = value + 4;
        }
	}

    public void Shake()
    {
        time = 0;
    }


}
