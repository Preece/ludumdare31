using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public PlayState playState = new PlayState();
	public PauseState pauseState = new PauseState();

	public LayerMask groundOnly;

	public bool paused = true;

	// Use this for initialization
	void Start () {
		playState.Start ();
		pauseState.Start ();

		SpawnRawMaterials();
	}
	
	// Update is called once per frame
	void Update () {
		if (paused) {
			pauseState.Update();
		} else {
			playState.Update();
		}

		if(Input.GetKeyDown(KeyCode.Space)) {
			if(!paused) {
				playState.Pause();
			} else {
				pauseState.Play();
			}

			paused = !paused;
		}

		if(Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit; 
			Physics.Raycast (ray, out hit, 100, groundOnly);

			if(paused) {
				pauseState.LeftClick(hit.point);
			} else {
				playState.LeftClick(hit.point);
			}
		}

		if(Input.GetMouseButtonDown (1)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit; 
			Physics.Raycast (ray, out hit, 100, groundOnly);

			if(paused) {
				pauseState.RightClick(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			} else {
				GameObject.Find("GameController").GetComponent<UnitManager>().MoveUnitsTo(hit.point);
			}
		}

		if(Input.GetMouseButtonUp(0)) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit; 
			if (Physics.Raycast(ray, out hit, 100, groundOnly)) {

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

	public void SpawnRawMaterials() {

	}
}
