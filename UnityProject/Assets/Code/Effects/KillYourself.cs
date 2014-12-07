using UnityEngine;
using System.Collections;

public class KillYourself : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("Suicide", 2f); 
	}
	void Suicide(){
		Destroy (gameObject); 
	}

}
