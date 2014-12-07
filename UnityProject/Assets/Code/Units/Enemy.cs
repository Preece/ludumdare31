using UnityEngine;
using System.Collections;

public class Enemy : Unit {

	public int damage = 10; 
	Worker _presentThreat;

	bool _startingAttack = true; 
	float _animTimer = 0; 

	public void GotShot(Worker _shooter, int _damageTaken){

		_health -= _damageTaken; 
		Debug.Log ("an enemy gotShot | " + _health); 
		if (_health <= 0) {
			Died();	
		}
		if (_presentThreat == null) {
			_presentThreat = _shooter; 	
		}
	}
	void Attack(){
		if (_agent.remainingDistance <= _agent.stoppingDistance && _presentThreat != null) {
			_anim.SetBool("Attacking",true); 
		}
		else{
			_anim.SetBool("Attacking",false); 
		}

		if(_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && _startingAttack == true && _presentThreat != null){
			_presentThreat.GotHit (this,damage); 
			_startingAttack = false;
		}
		if (_animTimer > _anim.GetCurrentAnimatorStateInfo (0).normalizedTime - Mathf.Floor (_anim.GetCurrentAnimatorStateInfo (0).normalizedTime)) {
			// a new animation happened
			_startingAttack = true;		
		}
		_animTimer = _anim.GetCurrentAnimatorStateInfo (0).normalizedTime - Mathf.Floor (_anim.GetCurrentAnimatorStateInfo (0).normalizedTime);
	}
	void Pursuit(){
		if (_presentThreat != null) {
			GoHere (_presentThreat.transform.position); 
		}
	}
	protected override void Died (){
		_manager.Died (this);
		Destroy (gameObject); 
	}
	protected override void AddToList (){
		_manager.AddToLists (this); 
	}

	void RelentlessHunt(){
		if(_presentThreat == null){
			Debug.Log("LOoking for another worker to savage"); 
			Collider[] _workers = Physics.OverlapSphere (transform.position, 80f);
			foreach (Collider _possibleTarget in _workers) {
				if(_possibleTarget.GetComponent<Worker>() != null){
					_presentThreat = _possibleTarget.GetComponent<Worker>();
					return; 
				}
			}
		}
	}
	void Update(){
		RelentlessHunt (); 
		Pursuit ();
		Attack ();
	}
}
