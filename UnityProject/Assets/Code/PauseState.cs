using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseState {

	ResourceNode selectedStructure = null;
	ResourceNode placementStructure = null;

	CommandCenter comGhost;
	Extractor extGhost;
	Refinery refGhost;
	Fabricator fabGhost;
	Pipe pipGhost;

	LayerMask obstructions;

	Dictionary<string, ResourceNode> prefabs = new Dictionary<string, ResourceNode>();

	enum MouseFunc { moving, dragging, prePiping, piping, placing };
	MouseFunc mouseFunc = MouseFunc.moving;


	// Use this for initialization
	public void Start () {
	
		prefabs.Add("command", GameObject.Find ("Structures").GetComponent<StructureController> ().commandCenter);
		prefabs.Add("extractor", GameObject.Find ("Structures").GetComponent<StructureController> ().extractor);
		prefabs.Add("refinery", GameObject.Find ("Structures").GetComponent<StructureController> ().refinery);
		prefabs.Add("fabricator", GameObject.Find ("Structures").GetComponent<StructureController> ().fabricator);
		prefabs.Add("pipe", GameObject.Find ("Structures").GetComponent<StructureController> ().pipe);

		obstructions = GameObject.Find("Structures").GetComponent<StructureController>().obstructions;
	
	}
	
	// Update is called once per frame
	public void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit; 
		Physics.Raycast (ray, out hit, 100);

		if(placementStructure != null) {
			placementStructure.transform.position = hit.point;
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			DestroyPlacementStructure();
			placementStructure = GameObject.Instantiate(prefabs["pipe"], Vector3.zero, Quaternion.identity) as ResourceNode;
			mouseFunc = MouseFunc.piping;
		}

		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			DestroyPlacementStructure();
			placementStructure = GameObject.Instantiate(prefabs["extractor"], Vector3.zero, Quaternion.identity) as ResourceNode;
			mouseFunc = MouseFunc.placing;
		}

		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			DestroyPlacementStructure();
			placementStructure = GameObject.Instantiate(prefabs["fabricator"], Vector3.zero, Quaternion.identity) as ResourceNode;
			mouseFunc = MouseFunc.placing;
		}

		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			DestroyPlacementStructure();
			placementStructure = GameObject.Instantiate(prefabs["refinery"], Vector3.zero, Quaternion.identity) as ResourceNode;
			mouseFunc = MouseFunc.placing;
		}
	}

	public void Play() {

	}

	public void LeftClick(Vector3 pos) {
		//when they left click, it could have a number of meanings
		//1. they are placing a structure that was selected
		//2. they are selecting a building that was already placed

		//if there is no placement structure, they are trying to select something
		if(mouseFunc == MouseFunc.moving) {

		} 
		//if they have the pipe selected and hooked to something, they are piping
		else if(mouseFunc == MouseFunc.piping) {

		}
		
	}

	public void RightClick(Vector3 pos) {
		DestroyPlacementStructure();
		
	}

	public void LeftRelease(Vector3 pos) {
		//instantiate a structure
		Collider[] collides = Physics.OverlapSphere (pos, 3.0f, obstructions);

		if(placementStructure != null && collides.Length == 0) {
			GameObject.Instantiate (placementStructure, pos, Quaternion.identity);
			DestroyPlacementStructure();
		}
	}

	private void DestroyPlacementStructure() {
		if(placementStructure != null) {
			Object.Destroy(placementStructure.gameObject);
			placementStructure = null;
			mouseFunc = MouseFunc.moving;
		}
	}
}
