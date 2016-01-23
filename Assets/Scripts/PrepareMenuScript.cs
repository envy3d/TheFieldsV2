using UnityEngine;
using System.Collections;

public class PrepareMenuScript : MonoBehaviour {

	void Start () {
        GameObject o = GameObject.FindGameObjectWithTag("Score");
        if (o != null)
            Destroy(o);
	}
}
