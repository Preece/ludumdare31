using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudController : MonoBehaviour {

	int _remainingWorkers;
	public int WorkersLeft {get{ return _remainingWorkers;}set{_remainingWorkers = value;}}
	public Text enemiesKilledText;
	public Text remainingWorkersText;
	public Text theTime; 

	public Text fuelAmount;
	public Text rawAmount; 
	public Text partsAmount; 
	ResourceNode _selectedNode; 

	int _enemiesKiled; 


	public void KilledAnEnemy(){
		_enemiesKiled += 1;
	}

	public void MakeExtractor(){
		GameObject.Find("GameController").GetComponent<Game>().pauseState.makeExtractor = true;
	}

	public void MakePipes(){
		GameObject.Find("GameController").GetComponent<Game>().pauseState.makePipe = true;
	}

	public void MakeRefinery(){
		GameObject.Find("GameController").GetComponent<Game>().pauseState.makeRefinery = true;
	}

	public void MakeFabricator(){
		GameObject.Find("GameController").GetComponent<Game>().pauseState.makeFabricator = true;
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
	public void SelectBuilding(ResourceNode _node ){
		_selectedNode = _node; 
	}
	void DisplayBuildingSpecfics(){
		if(_selectedNode != null){
			rawAmount.text = _selectedNode.GetRaw().ToString();
			fuelAmount.text = _selectedNode.GetFuel().ToString();
			partsAmount.text = _selectedNode.GetParts ().ToString(); 
		}
		else {
			rawAmount.text = " ";
			fuelAmount.text = " ";
			partsAmount.text = " "; 
		}
	}
	void OnGUI(){
		remainingWorkersText.text = _remainingWorkers.ToString();
		enemiesKilledText.text = _enemiesKiled.ToString(); 
		DisplayBuildingSpecfics (); 
	}
}
