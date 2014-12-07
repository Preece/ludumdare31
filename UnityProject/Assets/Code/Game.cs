using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	private PlayState playState = new PlayState();
	private PauseState pauseState = new PauseState();

	public LayerMask groundOnly;

	bool paused = true;

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

		if(Input.GetMouseButtonDown (0)) {
			if(paused) {
				pauseState.LeftClick(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			} else {
				playState.LeftClick(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
		}

		if(Input.GetMouseButtonDown (1)) {
			if(paused) {
				pauseState.RightClick(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			} else {
				
			}
		}

		if(Input.GetMouseButtonUp(0)) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit; 
			if (Physics.Raycast (ray, out hit, 100, groundOnly)) {

				if(paused) {
					pauseState.LeftRelease(hit.point);
				} else {
					playState.LeftRelease(hit.point);
				}
			}
		}
	}

	public bool Paused() {
		return paused;
	}
}
