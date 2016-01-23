using UnityEngine;
using System.Collections;

public class AnalyticsStartedScene : MonoBehaviour {

	void Start()
    {
        Application.ExternalCall("StartedScene", Application.loadedLevelName);
	}
	
}
