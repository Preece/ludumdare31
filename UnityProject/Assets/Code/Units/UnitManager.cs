using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class UnitManager : MonoBehaviour {

	public HudController hud; 
	public Game gameController;
	public List<Worker> _workers = new List<Worker>(); 
	Timer _timer; 
	public Object enemyPrefab; 
	public float spawnFrequency;
	public float spawnAmount; 
	public float spawnMax; 
	public List<Transform> spawnPoints = new List<Transform> (); 
	List<Enemy> _enemies = new List<Enemy>(); 
	public List<Worker> _selectedUnits = new List<Worker> (); 	


	public void Pause(){
		Debug.Log ("Pause"); 
		foreach (Unit _worker in _workers) {
			_worker.Pause();		
		}
		foreach (Unit _enemy in _enemies) {
			_enemy.Pause(); 		
		}
	}
	public void Play(){
		Debug.Log ("Play"); 
		foreach (Unit _worker in _workers) {
			_worker.Play();		
		}
		foreach (Unit _enemy in _enemies) {
			_enemy.Play(); 		
		}
	}
	public void AddToLists(Worker _unit){
		_workers.Add (_unit); 
		hud.WorkersLeft = _workers.Count; 
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
		hud.WorkersLeft = _workers.Count; 
	}
	public void Died(Enemy _unit){
		_enemies.Remove (_unit);
		hud.KilledAnEnemy (); 
	}
	public void DeselectWorkers(){
		_selectedUnits.Clear (); 
	}

	public void SelectWorker(Worker newW) {
		_selectedUnits.Add(newW);
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

	Vector3 PointToSpawn(){
		return  spawnPoints [Random.Range (0, spawnPoints.Count - 1)].position; 
	}
	int SpawnRate(){
		return (int) Mathf.Clamp((Mathf.Sin(_timer.runningTime*spawnFrequency)*_timer.runningTime/spawnAmount),0,spawnMax);
	}
	public bool GetGameState(){
		return gameController.paused; 
	}
	bool spawnSpurt = false; 
	void SpawnEnemies(){
		if (  ((int)(Mathf.Floor(_timer.runningTime))) % 4 == 0) {
			for(int i = 0; i < SpawnRate(); i++){
				GameObject _theEnemy = Instantiate (enemyPrefab) as GameObject;
				_theEnemy.transform.position = PointToSpawn(); 
			}
			spawnSpurt = false; 
		}
		else{
			spawnSpurt = true; 
		}
	}
	void Start(){
		_timer = GetComponent<Timer> ();
	}
	void Update(){
		SpawnEnemies (); 
	}


}
