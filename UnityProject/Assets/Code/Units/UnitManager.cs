using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class UnitManager : MonoBehaviour {
	
	public List<Worker> _workers = new List<Worker>(); 
	List<Enemy> _enemies = new List<Enemy>(); 
	List<Worker> _selectedUnits = new List<Worker> (); 	

	public void Pause(){
		foreach (Unit _worker in _workers) {
			_worker.Pause();		
		}
		foreach (Unit _enemy in _enemies) {
			_enemy.Pause(); 		
		}
	}
	public void Play(){
		foreach (Unit _worker in _workers) {
			_worker.Play();		
		}
		foreach (Unit _enemy in _enemies) {
			_enemy.Play(); 		
		}
	}
	public void AddToLists(Worker _unit){
		_workers.Add (_unit); 
	}
	public void AddToLists(Enemy _unit){
		_enemies.Add (_unit); 
	}
	public void AddToLists(Unit _unit){

	}
	public void Died(Unit _unit){

	}
	public void Died(Worker _unit){
		_workers.Remove (_unit);
	}
	public void Died(Enemy _unit){
		_enemies.Remove (_unit);
	}
	public void DeselectWorkers(){
		_selectedUnits.Clear (); 
	}
	public void SelectWorkers(List<Worker> _selection){
		foreach (Worker _selWorker in _selection) {
			_selectedUnits.Add(_selWorker); 		
		}
	}
	public void MoveUnitsTo(Vector3 _here){
		foreach (Worker _selWorker in _selectedUnits) {
			_selWorker.GoHere(_here); 	
		}
	}



}
