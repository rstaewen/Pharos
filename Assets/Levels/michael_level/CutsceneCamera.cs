using UnityEngine;
using System.Collections;

public class CutsceneCamera : MonoBehaviour {
	
	public float TimeToDestroy = 11f;
	void Start () {
		Invoke ("DestroyCamera", TimeToDestroy);
	}
	
	void DestroyCamera(){
		Destroy (this.gameObject);
		
	}
}
