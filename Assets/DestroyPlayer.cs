using UnityEngine;
using System.Collections;

public class DestroyPlayer : MonoBehaviour {

	public Transform RespawnPosition;
	// Use this for initialization
	void Start () {
		//transform.position = RespawnPosition.position;
		print ("transform position set to " + RespawnPosition.position);
	}
	
	// Update is called once per frame
	void Update () {
		//respawnTransform.position = Vector3(372.8109, 23.58064, 195.3531);
	}
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
						try {
				print ("Got the player-fire..");
								other.transform.position = RespawnPosition.position;
						} catch (UnityException e) {
								print ("exception is->" + e);
						}
				}
		}
}
