using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	private float startTime = 0;
	public Text timerText;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI () {
	    float guiTime = Time.time - startTime; 

		float minutes = guiTime / 60; 
		float seconds = guiTime % 60;
		float fraction = (guiTime * 100) % 100;

		timerText.text = string.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);

		//gameObject.guiText.text = "asss";
	}
}
