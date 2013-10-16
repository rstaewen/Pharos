﻿using UnityEngine;
using System.Collections;

public class CameraPhysics : MonoBehaviour {
	
	private MonoBehaviour cameraController;
	
	public void SetCameraController(MonoBehaviour cameraController)
	{
		this.cameraController = cameraController;
	}
	
	//This script prevents the camera's collider/rigidbody pair from
	//causing problems. collider/rigidbody pair will prevent camera from running into terrain or buildings.
	//testing is required to see if further changes are needed to make sure camera doesn't get caught.
	void OnTriggerEnter(Collider _collider){ OnTriggerStay(_collider);}
	void OnTriggerStay(Collider other)
	{
		//detect if object camera collides with has a rigidbody (movable object) or is the player.
		//camera should not collide with player or other objects - causes issues if it collides with player,
		//and if it collides with movable objects it will move the objects! Camera should not have any effect
		//on the world itself.
		if(other.GetComponent<Rigidbody>() || other.GetComponent<CharacterController>())
		{
			rigidbody.velocity = Vector3.zero;
			Physics.IgnoreCollision(collider, other);
		}
		else if (cameraController)
		{
			rigidbody.velocity = Vector3.zero;
			cameraController.Invoke("RetractDistance",0f);
		}
	}
}
