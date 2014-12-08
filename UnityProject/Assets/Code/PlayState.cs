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
		//if there is a unit under the click, select it
		//if shift is down, add it to the selection.
		//otherwise clear it and add just this guy

		//otherwise,begin a marquee selection
	
	}

	public void RightClick(Vector3 pos){
		//tell all dudes to go to the point
	}

	public void LeftRelease(Vector3 pos) {
		
	}
}
