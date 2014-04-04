using UnityEngine;
using System.Collections;

public class warptoAir : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		Debug.Log ("working");
		GameObject warp = GameObject.Find ("warp");
		if (warp != null) {
			DontDestroyOnLoad(warp);}
		Application.LoadLevel ("new_michael_level 1");}
}
