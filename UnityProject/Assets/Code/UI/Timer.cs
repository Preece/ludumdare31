using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public float runningTime = 0;
	public Text timerText;
	public double timeInSecs;

	Game game;

	// Use this for initialization
	void Start () {
		game = GameObject.Find("GameController").GetComponent<Game>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI () {
		if(!game.Paused()) {
	    	runningTime += Time.deltaTime; 
		}

		float minutes = runningTime / 60; 
		float seconds = runningTime % 60;
		float fraction = (runningTime * 100) % 100;

		timeInSecs = runningTime;

		timerText.text = string.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);

	}
}
