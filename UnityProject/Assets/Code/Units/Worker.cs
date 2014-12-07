using UnityEngine;
using System.Collections;

public class Worker : Unit {


	public float repairDistance = 5;

	bool _repairingStructure = false; 
	bool _attacking = false; 
	public Enemy _targetEnemy; 
	ResourceNode _targetStructure; 
	public LayerMask buildingMask; 


	protected override void Died (){
		_manager.Died (this);
	}
	protected override void AddToList (){
		_manager.AddToLists (this); 
	}

	public override void GoHere (Vector3 _destination){
		base.GoHere (_destination);
		_repairingStructure = false; 
		_targetStructure = null; 
		detector.NoLongerLooking (); 
	}

	public void FoundEnemy(Enemy _theEnemy){
		if(!_attacking){
			Debug.Log("Cry havoc"); 
			_targetEnemy = _theEnemy; 
			_attacking = true;
			_repairingStructure = false;
			_targetStructure = null; 
			detector.NoLongerLooking(); 
		}
	}
	void CryHavoc(){
		if (_targetEnemy != null) {
			Debug.Log("Let loose the dogs of war"); 		
		}
		if (_attacking && _targetEnemy == null) { //lost the target
			Debug.Log("Target eliminated"); 
			_attacking = false; 		
		}
		if (_attacking && _targetEnemy != null) {
			if(Vector3.Distance(transform.position, _targetEnemy.transform.position) > detector.GetComponent<SphereCollider>().radius +2 ){
				Debug.Log("We are done with war");  //target too far away
				Debug.Log(Vector3.Distance(transform.position, _targetEnemy.transform.position)); 
				_attacking = false;
				_targetEnemy = null; 
			}
		}
	}
	/*
	void CheckToRepair(){
		if(!_attacking && !_repairingStructure && _targetStructure == null && !_agent.pathPending)
		   {
			if (_agent.remainingDistance <= _agent.stoppingDistance)
			{
				if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
				{
					Collider[] _nearby = Physics.OverlapSphere(this.transform.position, repairDistance);
					for(int i = 0; i < _nearby.Length; i++){
						if(_nearby[i].gameObject.layer == 8){
							if(_nearby[i].gameObject.GetComponent<ResourceNode>() != null){
								_targetStructure = _nearby[i].gameObject.GetComponent<ResourceNode>(); 
								return;
							}
						}
					}
				}
			}
		}
	}
	*/
	public WorkerDetector detector; 
	void CheckToRepair(){
		if(!_attacking && !_repairingStructure && _targetStructure == null)
		{
			if (_agent.remainingDistance <= _agent.stoppingDistance)
			{
				if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
				{
					Debug.Log("look for stuff"); 
					detector.LookForToRepair(); 
				}
			}
		}
	}
	public void FoundThingToRepair(ResourceNode _node){
		Debug.Log ("Starting raycast"); 
		_targetStructure = _node; 
		Ray _ray = new Ray (transform.position, ( _node.transform.position - transform.position).normalized);
		RaycastHit _hit; 
		if(Physics.Raycast(_ray, out _hit,100,buildingMask)){
			_moveTarget.transform.position = _hit.point; 
			Debug.Log("Do the thing"); 
			detector.NoLongerLooking(); 
		}
	}
	void StartRepairing(){ //if you have arrived at the structure, but haven't started repairing it yet
		if(!_attacking && !_repairingStructure && _targetStructure != null && !_agent.pathPending)
		{
			if (_agent.remainingDistance <= _agent.stoppingDistance)
			{
				if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
				{
					Debug.Log("Repairing"); 
					_repairingStructure = true; 
				}
			}
		}
	}

	void RepairStructure(){ //the actual process of repairing
		
	}

	public Transform testTarget;
	void FollowTest(){
		if(_targetStructure == null){
			_moveTarget.position  = testTarget.position; 
			_agent.destination = _moveTarget.position;
		}
		else{
			_agent.destination = _moveTarget.position; 
		}
	}

	void Update(){
			CheckToRepair();
			StartRepairing();
			RepairStructure(); 
		CryHavoc (); 
		FollowTest (); 
	}


}
