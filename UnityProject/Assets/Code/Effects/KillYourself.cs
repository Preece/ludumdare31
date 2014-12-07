using UnityEngine;
using System.Collections;

public class KillYourself : MonoBehaviour {

	// Use this for initialization
	public float deathTimer = 2; 
	void Start () {
		Invoke ("Suicide", deathTimer); 
	}
	void Suicide(){
		Destroy (gameObject); 
	}

}
