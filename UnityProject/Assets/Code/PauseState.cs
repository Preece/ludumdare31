using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseState {

	UnitManager _unitManger; 

	ResourceNode selectedStructure = null;
	ResourceNode placementStructure = null;

	LayerMask obstructions;
	LayerMask rawResources;
	LayerMask ground;

	Dictionary<string, ResourceNode> prefabs = new Dictionary<string, ResourceNode>();

	enum MouseFunc { moving, dragging, prePiping, piping, placing, placingExtractor };
	MouseFunc mouseFunc = MouseFunc.moving;

	private GameObject pipeStarter;


	// Use this for initialization
	public void Start () {
	
		prefabs.Add("command", GameObject.Find ("Structures").GetComponent<StructureController> ().commandCenter);
		prefabs.Add("extractor", GameObject.Find ("Structures").GetComponent<StructureController> ().extractor);
		prefabs.Add("refinery", GameObject.Find ("Structures").GetComponent<StructureController> ().refinery);
		prefabs.Add("fabricator", GameObject.Find ("Structures").GetComponent<StructureController> ().fabricator);
		prefabs.Add("pipe", GameObject.Find ("Structures").GetComponent<StructureController> ().pipe);

		obstructions = GameObject.Find("Structures").GetComponent<StructureController>().obstructions;
		rawResources = GameObject.Find("Structures").GetComponent<StructureController>().rawResources;
		ground = GameObject.Find("Structures").GetComponent<StructureController>().ground;
	}
	
	// Update is called once per frame
	public void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit; 
		Physics.Raycast (ray, out hit, 100, ground);

		if(placementStructure != null) {
			placementStructure.transform.position = hit.point;
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			DestroyPlacementStructure();
			placementStructure = GameObject.Instantiate(prefabs["pipe"], Vector3.zero, Quaternion.identity) as ResourceNode;
			placementStructure.gameObject.layer = 0;
			mouseFunc = MouseFunc.prePiping;
		}

		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			DestroyPlacementStructure();
			placementStructure = GameObject.Instantiate(prefabs["extractor"], Vector3.zero, Quaternion.identity) as ResourceNode;
			placementStructure.gameObject.layer = 0;
			mouseFunc = MouseFunc.placingExtractor;
		}

		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			DestroyPlacementStructure();
			placementStructure = GameObject.Instantiate(prefabs["fabricator"], Vector3.zero, Quaternion.identity) as ResourceNode;
			placementStructure.gameObject.layer = 0;
			mouseFunc = MouseFunc.placing;
		}

		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			DestroyPlacementStructure();
			placementStructure = GameObject.Instantiate(prefabs["refinery"], Vector3.zero, Quaternion.identity) as ResourceNode;
			placementStructure.gameObject.layer = 0;
			mouseFunc = MouseFunc.placing;
		}

		if(selectedStructure != null) {
			Debug.Log("Raw: " + selectedStructure.GetRaw() + " Fuel: " + selectedStructure.GetFuel() + " Parts: " + selectedStructure.GetParts());
		}
	}

	public void Play() {
		_unitManger = GameObject.Find ("GameController").GetComponent<UnitManager> (); 
	}

	public void LeftClick(Vector3 pos) {
		//when they left click, it could have a number of meanings
		//1. they are placing a structure that was selected
		//2. they are selecting a building that was already placed

		Collider[] buildings = Physics.OverlapSphere (pos, 0.2f, obstructions);

		//if there is no placement structure, they are trying to select something
		if(mouseFunc == MouseFunc.moving) {
			if(buildings.Length > 0) {
				selectedStructure = buildings[0].gameObject.GetComponent<ResourceNode>();
			}
		} 
		//if they have selected the pipe, they are trying to start the pipe
		else if(mouseFunc == MouseFunc.prePiping) {

			if(buildings.Length > 0) {
				pipeStarter = buildings[0].gameObject;
				mouseFunc = MouseFunc.piping;
			}
		}
		//if they have the pipe selected and hooked to something, they are piping
		else if(mouseFunc == MouseFunc.piping) {

			if(buildings.Length > 0) {
				MakePipes(pipeStarter, buildings[0].gameObject, prefabs["pipe"]);
				DestroyPlacementStructure();
				mouseFunc = MouseFunc.moving;
			}
		}
		
	}

	public void RightClick(Vector3 pos) {
		DestroyPlacementStructure();
		selectedStructure = null;
		
	}

	public void LeftRelease(Vector3 pos) {
		//instantiate a structure
		if(mouseFunc == MouseFunc.placing) {
			Collider[] collides = Physics.OverlapSphere (pos, 3.0f, obstructions);

			if(placementStructure != null && collides.Length == 0) {
				ResourceNode nodezz = GameObject.Instantiate (placementStructure, pos, Quaternion.identity) as ResourceNode;
				DestroyPlacementStructure();
				nodezz.gameObject.layer = 8;
			}
		}
		//if they are trying to place an extractor
		else if(mouseFunc == MouseFunc.placingExtractor) {
			Collider[] collides = Physics.OverlapSphere (pos, 3.0f, obstructions);
			Collider[] rezzys = Physics.OverlapSphere(pos, 1, rawResources);

			if(placementStructure != null && collides.Length == 0 && rezzys.Length != 0) {
				ResourceNode newExtractor = GameObject.Instantiate (placementStructure, pos, Quaternion.identity) as ResourceNode;
				DestroyPlacementStructure();
				newExtractor.gameObject.layer = 8;

				//set the extractor to be fed by the resource
				rezzys[0].gameObject.GetComponent<RawMaterials>().AddFeedee(newExtractor.GetComponent<Extractor>());
			}
		}
	}

	private void DestroyPlacementStructure() {
		if(placementStructure != null) {
			Object.Destroy(placementStructure.gameObject);
			placementStructure = null;
			mouseFunc = MouseFunc.moving;
		}
	}

	public GameObject MakePipes(GameObject startObject, GameObject endPositionObj, ResourceNode pipe) {

		Vector3 direction = (endPositionObj.transform.position - startObject.transform.position).normalized; //get the direction of the pieps 
		float distance = Vector3.Distance (startObject.transform.position, endPositionObj.transform.position);  //how far are they?

		GameObject pipeParent = new GameObject ();  //make the parent object
		pipeParent.transform.position = startObject.transform.position + distance * .5f * direction; // put it in the middle
		pipeParent.transform.position = new Vector3(0,0,0) + distance * .5f * direction; // put it in the middle
		pipeParent.name = "PipesFrom " + startObject.name; 
		float laidPipeDistance = 0; 
		float pipeLength = 0; 

		Pipe prevPipe = null;

		while (laidPipeDistance < distance) { //this will keep making pipe, till you've reached your destination
			ResourceNode thePipe = GameObject.Instantiate(pipe) as ResourceNode; 

			Pipe pipeComp  = thePipe.GetComponent<Pipe>(); 

			if(pipeLength == 0){
				pipeLength = pipeComp.mesh.mesh.bounds.size.z; //this may be the wrong axis, will need checking
			} 

			if(prevPipe == null) {
				//set the first section of pipe as a feedee of the start object
				startObject.GetComponent<ResourceNode>().AddFeedee(pipeComp);
				prevPipe = pipeComp;
			} else {
				//otherwise, set the previous pipe section as being fed by this current section
				prevPipe.AddFeedee(pipeComp);
				prevPipe = pipeComp;
			}

			thePipe.transform.forward = direction; //have it look towards the end position
			thePipe.transform.position = startObject.transform.position + direction * laidPipeDistance; //tells it where to be
			thePipe.transform.parent = pipeParent.transform; 
			laidPipeDistance += pipeLength; 
		}

		//connect the end object as a feedee of the last section of pipe
		prevPipe.AddFeedee(endPositionObj.GetComponent<ResourceNode>());

		return pipeParent; 
	}
}
