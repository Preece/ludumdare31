using UnityEngine;
using System.Collections;

public class Worker : Unit {

	[SerializeField]
	float _health;
	public float Health { get { return _health; } set { _health = value; } }
	public float repairDistance = 5;

	bool _repairingStructure = false; 
	bool _attacking = false; 
	Enemy _targetEnemy; 
	ResourceNode _targetStructure; 


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
	}

	public void FoundEnemy(Enemy _theEnemy){
		if(!_attacking){
			_targetEnemy = _theEnemy; 
			_attacking = true;
			_repairingStructure = false;
			_targetStructure = null; 
		}
	}
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
	void StartRepairing(){ //if you have arrived at the structure, but haven't started repairing it yet
		if(!_attacking && !_repairingStructure && _targetStructure != null && !_agent.pathPending)
		{
			if (_agent.remainingDistance <= _agent.stoppingDistance)
			{
				if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
				{
					_repairingStructure = true; 
				}
			}
		}
	}

	void RepairStructure(){
		
	}
	void Update(){
		if (_play) {
			CheckToRepair();
			StartRepairing();
			RepairStructure(); 
		}
	}

}
