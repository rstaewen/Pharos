using UnityEngine;
using System.Collections;

public class mirrorEnd : MonoBehaviour {

	public GameObject warppoint;

	// Use this for initialization
	void Start () {
		GameObject Mirror = GameObject.Find ("warp");

		if (Mirror != null && warppoint !=null) {
			this.transform.position=warppoint.transform.position;
		}
	
	}
	
	// Update is called once per frame

	void Update () {
	
	}
}
