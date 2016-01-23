using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class SpawnAfterDelayScript : MonoBehaviour {

    public GameObject thingToSpawn;
    public float delay;

    private Timer delayTimer;

	void Start () {
        delayTimer = new Timer(delay, () => SpawnThing());
        delayTimer.Restart();
	}
	
	void Update () {
        delayTimer.Update(Time.deltaTime);
	}

    public void SpawnThing()
    {
        GameObject.Instantiate(thingToSpawn,transform.position, transform.rotation);
    }
}
