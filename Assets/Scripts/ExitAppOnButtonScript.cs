using UnityEngine;
using System.Collections;

public class ExitAppOnButtonScript : MonoBehaviour {

    public string buttonName = "Escape";
	
	void Update() 
    {
        if (Input.GetButtonDown(buttonName))
            Application.Quit();
	}
}
