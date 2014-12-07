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

	private GameObject pipeStarter;


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
			mouseFunc = MouseFunc.prePiping;
			Debug.Log("pre pipin");
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
		//if they have selected the pipe, they are trying to start the pipe
		else if(mouseFunc == MouseFunc.prePiping) {
			
			Collider[] collides = Physics.OverlapSphere (pos, 0.2f, obstructions);

			if(collides.Length > 0) {
				Debug.Log("layin pipez start");
				pipeStarter = collides[0].gameObject;
				mouseFunc = MouseFunc.piping;
			}
		}
		//if they have the pipe selected and hooked to something, they are piping
		else if(mouseFunc == MouseFunc.piping) {
			Debug.Log("layin pipez");
			Collider[] collides = Physics.OverlapSphere (pos, 0.2f, obstructions);

			if(collides.Length > 0) {
				MakePipes(pipeStarter, collides[0].gameObject, prefabs["pipe"]);
				DestroyPlacementStructure();
				mouseFunc = MouseFunc.moving;
			}
		}
		
	}

	public void RightClick(Vector3 pos) {
		DestroyPlacementStructure();
		
	}

	public void LeftRelease(Vector3 pos) {
		//instantiate a structure
		if(mouseFunc == MouseFunc.placing) {
			Collider[] collides = Physics.OverlapSphere (pos, 3.0f, obstructions);

			if(placementStructure != null && collides.Length == 0) {
				GameObject.Instantiate (placementStructure, pos, Quaternion.identity);
				DestroyPlacementStructure();
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

	public GameObject MakePipes(GameObject _startObject, GameObject endPositionObj, ResourceNode _pipe){
		Vector3 _direction = (endPositionObj.transform.position - _startObject.transform.position).normalized; //get the direction of the pieps
		Debug.Log (_startObject.transform.position + " | " + endPositionObj.transform.position); 
		float _distance = Vector3.Distance (_startObject.transform.position, endPositionObj.transform.position);  //how far are they?
		Debug.Log (_distance); 
		GameObject pipeParent = new GameObject ();  //make the parent object
		pipeParent.transform.position = _startObject.transform.position + _distance * .5f * _direction; // put it in the middle
		pipeParent.transform.position = new Vector3(0,0,0) + _distance * .5f * _direction; // put it in the middle
		pipeParent.name = "PipesFrom " + _startObject.name; 
		float _laidPipeDistance = 0; 
		float _pipeLength = 0; 
		Debug.Log (_distance); 
		while (_laidPipeDistance < _distance) { //this will keep making pipe, till you've reached your destination
			ResourceNode _thePipe = GameObject.Instantiate(_pipe) as ResourceNode; 
			if(_thePipe == null){
				Debug.Log("no pipe object"); 
			}
			Pipe _pipeComp  = _thePipe.GetComponent<Pipe>(); 
			if(_pipeLength == 0){
				_pipeLength = _pipeComp.mesh.mesh.bounds.size.z; //this may be the wrong axis, will need checking
			}
			_thePipe.transform.forward = _direction; //have it look towards the end position
			_thePipe.transform.position = _startObject.transform.position + _direction * _laidPipeDistance; //tells it where to be
			_thePipe.transform.parent = pipeParent.transform; 
			_laidPipeDistance += _pipeLength; 
		}
		return pipeParent; 
	}
}
