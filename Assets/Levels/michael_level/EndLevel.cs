using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {

	void Start () {
		Invoke ("LevelTransition", 6f);
	}
	
	void LevelTransition() {
		Debug.Log ("Level transition script here.");
		Application.LoadLevel (Application.loadedLevel);
	}
}
