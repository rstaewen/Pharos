using UnityEngine;
using System.Collections;

public class Rotating_Lord : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(15,30,45) * Time.deltaTime);
	}

//	void OnTriggerEnter (Collider other) {
//		
//		if(other.gameObject.tag == "Player" )
//			gameObject.SetActive(false);
//	}
}
