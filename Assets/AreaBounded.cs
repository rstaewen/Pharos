using UnityEngine;
using System.Collections;

public class AreaBounded : MonoBehaviour {
	
	private SphereCollider _sphereCollider;
	private Collider playerCollider;
	private PlayerPhysics playerPhysics;
	// Use this for initialization
	void Start ()
	{
		_sphereCollider = GetComponent<SphereCollider>();
	}
	
	void OnTriggerStay(Collider _collider)
	{
		playerCollider = _collider;
		PlayerPhysics testPlayer = playerCollider.gameObject.GetComponent<PlayerPhysics>();
		if (testPlayer)
		{
			playerPhysics = testPlayer;
			playerPhysics.pushPosition = Vector3.zero;
			CancelInvoke("pushIn");
			Invoke("pushIn", 0.2f);
		}
		else
			Physics.IgnoreCollision(_sphereCollider, _collider);
	}
	void pushIn()
	{
		if(playerPhysics)
			playerPhysics.pushPosition = transform.position;
	}
	void OnDisable()
	{
		if(playerPhysics)
			playerPhysics.pushPosition = Vector3.zero;
	}
}
