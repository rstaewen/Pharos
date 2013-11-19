using UnityEngine;
using System.Collections;

public class CutsceneCamera : MonoBehaviour {
	
	public float TimeToDestroy = 11f;
	void Start () {
		Invoke ("DestroyCamera", TimeToDestroy);
	}
	
	void DestroyCamera(){
		Destroy (this.gameObject);
<<<<<<< HEAD
=======
		
>>>>>>> f8d901c82c1d3a8891ac536dc05c823179eb5414
	}
}
