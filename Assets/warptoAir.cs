using UnityEngine;
using System.Collections;

public class warptoAir : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		GameObject warp = GameObject.Find ("warp");
		if (warp != null) {
			DontDestroyOnLoad(warp);
		}
		Application.LoadLevel ("new_michael_level 1");
	}

	void OnGUI() {
		if(Application.isLoadingLevel)
		{
			GUI.Button(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2), "Loading...");
		}
	}
}
