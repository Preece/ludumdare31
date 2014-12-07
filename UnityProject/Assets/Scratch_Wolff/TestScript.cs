using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	public GameObject theStart;
	public GameObject end; 
	public Object _theObject; 

	public GameObject MakePipes(GameObject _startObject, GameObject endPositionObj, Object _pipe){
		Vector3 _direction = (endPositionObj.transform.position - _startObject.transform.position).normalized; //get the direction of the pieps
		Debug.Log (_startObject.transform.position + " | " + endPositionObj.transform.position); 
		float _distance = Vector3.Distance (_startObject.transform.position, endPositionObj.transform.position);  //how far are they?
		Debug.Log (_distance); 
		GameObject pipeParent = new GameObject ();  //make the parent object
		pipeParent.transform.position = _startObject.transform.position + _distance * .5f * _direction; // put it in the middle
		pipeParent.transform.position = new Vector3(0,0,0) + _distance * .5f * _direction; // put it in the middle
		
		float _laidPipeDistance = 0; 
		float _pipeLength = 0; 
		Debug.Log (_distance); 
		while (_laidPipeDistance < _distance) { //this will keep making pipe, till you've reached your destination
			GameObject _thePipe = Instantiate(_pipe) as GameObject; 
			Pipe _pipeComp = _thePipe.GetComponent<Pipe>(); 
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

	void Start(){
		MakePipes (theStart, end, _theObject); 
	}
}
