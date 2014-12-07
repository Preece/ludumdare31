using UnityEngine;
using System.Collections;

public class Enemy : Unit {

	Worker _presentThreat;

	public void GotShot(Worker _shooter, int damage){
		_health -= damage; 
		if (_health <= 0) {
			Died();	
		}
		if (_presentThreat == null) {
			_presentThreat = _shooter; 	
		}
	}

	protected override void Died (){
		_manager.Died (this);
	}
	protected override void AddToList (){
		_manager.AddToLists (this); 
	}


}
