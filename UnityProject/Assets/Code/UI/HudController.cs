using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudController : MonoBehaviour {

	int _remainingWorkers;
	public int WorkersLeft {get{ return _remainingWorkers;}set{_remainingWorkers = value;}}
	public Text enemiesKilledText;
	public Text remainingWorkersText;
	public Text theTime; 

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
	public void Timer(string _currentTime){
		theTime.text = _currentTime; 
	}
	void OnGUI(){
		remainingWorkersText.text = _remainingWorkers.ToString();
		enemiesKilledText.text = _enemiesKiled.ToString(); 
	}
}
