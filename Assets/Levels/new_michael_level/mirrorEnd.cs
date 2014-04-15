using UnityEngine;
using System.Collections;

public class mirrorEnd : MonoBehaviour {

	public GameObject warppoint;

	void Start () {
		GameObject warp = GameObject.Find ("warp");

		if (warp != null && warppoint != null) {
			this.transform.position = warppoint.transform.position;
		}
	
	}
}
