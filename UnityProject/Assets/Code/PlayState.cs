using UnityEngine;
using System.Collections;

public class PlayState {

	UnitManager _unitManger; 

	// Use this for initialization
	public void Start () {
		_unitManger = GameObject.Find ("GameController").GetComponent<UnitManager> (); 
	}
	
	// Update is called once per frame
	public void Update () {

	}

	public void Pause() {

	}

	public void LeftClick(Vector3 pos) {
	
	}

	public void RightClick(Vector3 pos){

	}

	public void LeftRelease(Vector3 pos) {
		
	}
}
