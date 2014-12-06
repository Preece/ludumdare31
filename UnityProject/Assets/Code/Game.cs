using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	PlayState playState = new PlayState();
	PauseState pauseState = new PauseState();

	bool paused = false;

	// Use this for initialization
	void Start () {
		playState.Start ();
		pauseState.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		if (paused) {
			pauseState.Update();
		} else {
			playState.Update();
		}

		if(Input.GetKeyDown(KeyCode.Space)) {
			paused = !paused;
		}
	}

	public bool Paused() {
		return paused;
	}
}
