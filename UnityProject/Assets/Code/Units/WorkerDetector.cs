using UnityEngine;
using System.Collections;

public class WorkerDetector : MonoBehaviour {

	Worker _worker; 
	bool _lookingToRepair = false; 

	public void LookForToRepair(){
		_lookingToRepair = true; 
	}
	public void NoLongerLooking(){
		_lookingToRepair = false; 
	}

	void OnTriggerEnter(Collider _other){
		if(_other.gameObject.layer == 11){ //Enemy found
			if(_other.gameObject.GetComponent<Enemy>() != null){
				_worker.FoundEnemy(_other.gameObject.GetComponent<Enemy>()); 
			}
		}
	}

	void OnTriggerStay(Collider _other){
		if (_other.gameObject.layer == 8 && _lookingToRepair) {
			if(_other.gameObject.GetComponent<ResourceNode>() != null){
				_worker.FoundThingToRepair(_other.gameObject.GetComponent<ResourceNode>()); 
			}
		}
	}
	void Start(){
		_worker = GetComponentInParent<Worker> (); 
	}


}
