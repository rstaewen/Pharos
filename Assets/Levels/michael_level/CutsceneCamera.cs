using UnityEngine;
using System.Collections;

public class CutsceneCamera : MonoBehaviour {

	void Start () {
		Debug.Log ("Calling Destroy Camera");
		Invoke ("DestroyCamera", 11f);
	}
	
	void DestroyCamera(){
		Debug.Log ("Destroy Camera");
	}
}
