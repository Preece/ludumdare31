using UnityEngine;
using System.Collections;

public class Enemy : Unit {

	protected override void Died (){
		_manager.Died (this);
	}
	protected override void AddToList (){
		_manager.AddToLists (this); 
	}


}
