using UnityEngine;
using System.Collections;

public class Enemy : Unit {

	public int damage = 10; 
	Worker _presentThreat;
	bool _startingAttack = false; 
	float _animTimer = 0; 
	public Object bloodSplatter; 
	bool _knowsInitialState = false; 

	public void GotShot(Worker _shooter, int _damageTaken){

		_health -= _damageTaken; 
		Debug.Log ("an enemy gotShot | " + _health); 
		if (_health <= 0) {
			Died();	
		}
		if (_presentThreat == null) {
			_presentThreat = _shooter; 	
		}
		GameObject _splatter =  Instantiate (bloodSplatter) as GameObject; 
		_splatter.transform.position = transform.position;
		_splatter.transform.forward = transform.position - _shooter.transform.position; 
		_splatter.transform.parent = transform; 
	}
	void Attack(){
		if (_agent.remainingDistance <= _agent.stoppingDistance && _presentThreat != null) { //gets the attack animation to play
			_anim.SetBool("Attacking",true); 
		}
		else{
			_anim.SetBool("Attacking",false); 
		} //this bit is about holding them in place
		if(_anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
			_agent.speed = 0; 
			_agent.velocity = Vector3.zero; 
		}
		else{
			_agent.speed = 10; 
		}
		if(_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && _startingAttack == true && _presentThreat != null
		   		&& !_anim.IsInTransition(0)){ //at the begginging of their attack animation, they deal damage
			_presentThreat.GotHit (this,damage); 
			_startingAttack = false;
		}
		if (_animTimer > _anim.GetCurrentAnimatorStateInfo (0).normalizedTime - Mathf.Floor (_anim.GetCurrentAnimatorStateInfo (0).normalizedTime)) {
			// a new animation happened
			_startingAttack = true;		
		}
		_animTimer = _anim.GetCurrentAnimatorStateInfo (0).normalizedTime - Mathf.Floor (_anim.GetCurrentAnimatorStateInfo (0).normalizedTime);
	}
	protected override void Died (){
		_manager.Died (this);
		Destroy (_moveTarget.gameObject); 
		gameObject.layer = 0; 
		_anim.Play ("Death"); 
		Destroy (GetComponent<SphereCollider> ());
		Destroy (_agent);
		Destroy (GetComponent<Rigidbody> ());
		Destroy (this); 
	}
	protected override void AddToList (){
		_manager.AddToLists (this); 
	}
	void Pursuit(){
		if (_presentThreat != null) {
			GoHere (_presentThreat.transform.position); 
		}
	}
	void RelentlessHunt(){
		if(_presentThreat == null){
			Collider[] _workers = Physics.OverlapSphere (transform.position, 80f);
			foreach (Collider _possibleTarget in _workers) {
				if(_possibleTarget.GetComponent<Worker>() != null){
					_presentThreat = _possibleTarget.GetComponent<Worker>();
					return; 
				}
			}
		}
	}
	void GetInitialState(){
		if (!_knowsInitialState) {
			Play (); 
			_knowsInitialState =true; 
		}
	}
	void Update(){
		if(_play) {
			RelentlessHunt (); 
			Pursuit ();
			Attack ();
		}
		GetInitialState (); 
	}
	void Start(){
		_anim.Play ("Start",0, Random.Range (0, .8f)); 
	}
}
