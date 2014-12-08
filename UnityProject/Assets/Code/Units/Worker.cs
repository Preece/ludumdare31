using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class Worker : Unit {

	public int weaponDamage = 30;
	public float repairDistance = 5;

	bool _repairingStructure = false; 
	bool _attacking = false; 
	public Enemy _targetEnemy; 
	ResourceNode _targetStructure; 
	public LayerMask buildingMask; 
	public WorkerDetector detector; 
	public List<Transform> gunPoints = new List<Transform> (); 
	public Object muzzleFX; 
	public Object explosionFX;

	bool _spawnFireFX = true; 
	float _animTime; 


	protected override void Died (){
		Instantiate (explosionFX, transform.position, new Quaternion()); 
		_manager.Died (this);
		Destroy (_moveTarget.gameObject); 
		Destroy (this.gameObject); 
	}
	protected override void AddToList (){
		_manager.AddToLists (this); 
	}

	public override void GoHere (Vector3 _destination){
		base.GoHere (_destination);
		_repairingStructure = false; 
		_targetStructure = null; 
		_anim.SetBool("Building", false); 
		detector.NoLongerLooking (); 

		foreach (SkinnedMeshRenderer _theMesh in theMeshes) {
			_theMesh.material.color = healthTint.Evaluate (100-_health); 
		}
	}

	public void FoundEnemy(Enemy _theEnemy){
		if(!_attacking){
			Debug.Log("Cry havoc"); 
			_targetEnemy = _theEnemy; 
			_attacking = true;
			_repairingStructure = false;
			_targetStructure = null; 
			_anim.SetBool("Building", false); 
			_anim.SetBool("Attacking",true); 
			detector.NoLongerLooking(); 
		}
	}
	void FaceTheEnemy(){
		transform.forward = Vector3.Lerp (transform.forward, _targetEnemy.transform.position - transform.position,Time.deltaTime);
	}
	void CryHavoc(){ //The attacking bit of code
		if (_targetEnemy != null) {
			FaceTheEnemy(); 
		}
		if (_attacking && _targetEnemy == null) { //lost the target
			Debug.Log("Target eliminated"); 
			if(LookForAnotherEnemy() == false){
				_attacking = false; 	
				_anim.SetBool("Attacking",false); 
			}

		}
		if (_attacking && _targetEnemy != null) {
			if(Vector3.Distance(transform.position, _targetEnemy.transform.position) > detector.GetComponent<SphereCollider>().radius +2 ){
				Debug.Log("We are done with war");  //target too far away
				Debug.Log(Vector3.Distance(transform.position, _targetEnemy.transform.position)); 
				_attacking = false;
				_targetEnemy = null; 
				_anim.SetBool("Attacking",false); 
			}
		}
	}
	bool LookForAnotherEnemy(){
		Collider[] _potentialEnemies = Physics.OverlapSphere (transform.position, detector.GetComponent<SphereCollider> ().radius);
		foreach (Collider _enemy in _potentialEnemies) {
			if( _enemy.gameObject.GetComponent<Enemy>() != null){
				_attacking = true; 
				_targetEnemy = _enemy.gameObject.GetComponent<Enemy>();
				return true; 
			}
		}
		return false; 
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

	void CheckToRepair(){
		if(!_attacking && !_repairingStructure && _targetStructure == null)
		{
			if (_agent.remainingDistance <= _agent.stoppingDistance)
			{
				if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
				{
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
					_anim.SetBool("Building", true); 
					_repairingStructure = true; 
				}
			}
		}
	}

	void RepairStructure(){ //the actual process of repairing
		
	}


	public void FireWeaponFX(){

		if(_anim.GetCurrentAnimatorStateInfo (0).IsName("Attack") && _spawnFireFX && _attacking && _targetEnemy != null){ //just started playing
			for(int i = 0; i < gunPoints.Count;i++){
				GameObject _theFX = Instantiate(muzzleFX) as GameObject;
				_theFX.transform.parent = gunPoints[i].transform; 
				_theFX.transform.position = gunPoints[i].transform.position;
				_theFX.transform.rotation = gunPoints[i].transform.rotation;
			}
			_spawnFireFX = false;
			DoDamage (); 

		}
		//the start of a loop
		if (_animTime >  _anim.GetCurrentAnimatorStateInfo (0).normalizedTime - Mathf.Floor (_anim.GetCurrentAnimatorStateInfo (0).normalizedTime) && !_spawnFireFX) {
			_spawnFireFX = true; 	
		}
		_animTime = _anim.GetCurrentAnimatorStateInfo (0).normalizedTime - Mathf.Floor (_anim.GetCurrentAnimatorStateInfo (0).normalizedTime);
	}
	void DoDamage(){
		_targetEnemy.GotShot (this, weaponDamage); 
	}
	public void GotHit(Enemy _attacker, int _damage){
		_health -= _damage;
		Debug.Log ("A worker got hit and has " + _health + " left"); 
		if (_health < 0) {
			Died(); 		
		}
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
	void Start(){
		foreach (SkinnedMeshRenderer _theMesh in theMeshes) {
			_theMesh.material.color = healthTint.Evaluate (100-_health); 
		}
	}

	void Update(){
		if(_play) {
			CheckToRepair();
			StartRepairing();
			RepairStructure(); 
			CryHavoc (); 
			AnimControl (); 
			//FollowTest (); 
			FireWeaponFX (); 
		}
	}
	void CheckState(){
		if (_manager.isPaused ) {
			Pause();		
		}
		else{
			Play(); 
			Debug.Log ("Starting in play mode"); 
		}
	}


}
