﻿using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour {

	int _remainingWorkers;
	public int WorkersLeft {get{ return _remainingWorkers;}set{_remainingWorkers = value;}}

	int _enemiesKiled; 


	public void KilledAnEnemy(){
		_enemiesKiled += 1;
	}

	public void MakeExtractor(){
	
	}
	public void MakePipes(){
		
	}
	public void MakeRefinery(){
		
	}
	public void MakeFabricator(){
	
	}
	public void SelectBuilging(ResourceNode _bulding){

	}
	public void Play(){
	
	}
	public void Pause(){
		
	}
}