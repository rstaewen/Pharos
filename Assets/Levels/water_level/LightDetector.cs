using UnityEngine;
using System.Collections;

public class LightDetector : MonoBehaviour {
	public int lightnumber;
	public Transform targetlocation;
	public float speed = 5f;
	private LightController lightController;
	private bool move = true;

	void Start(){
		lightController = GameObject.FindGameObjectWithTag ("LightController").GetComponent<LightController>();

	}
	void OnTriggerEnter(Collider other){
		lightController.Check (lightnumber-1);

	}
	void Update(){
		if (move == true && targetlocation != null) {
			moveLight();		
		}

	}
	void moveLight(){
		Vector3 targetdirection = targetlocation.position - transform.position;
		Vector3 targetarea = new Vector3(targetlocation.position.x, targetlocation.position.y,targetlocation.position.z);
		transform.position = Vector3.MoveTowards (transform.position, targetarea, speed*Time.deltaTime);

	}
}
