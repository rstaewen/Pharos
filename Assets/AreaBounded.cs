using UnityEngine;
using System.Collections;

public class AreaBounded : MonoBehaviour {
	
	private SphereCollider _sphereCollider;
	private Collider playerCollider;
	private PlayerPhysics playerInteraction;
	// Use this for initialization
	void Start ()
	{
		_sphereCollider = GetComponent<SphereCollider>();
	}
	
	void OnTriggerStay(Collider _collider)
	{
		playerCollider = _collider;
		PlayerPhysics testPlayer = playerCollider.gameObject.GetComponent<PlayerPhysics>();
			Debug.Log("testTriggerStay");
		if (testPlayer)
		{
			playerInteraction = testPlayer;
			playerInteraction.pushPosition = Vector3.zero;
			Debug.Log("test");
			CancelInvoke("pushIn");
			Invoke("pushIn", 0.2f);
		}
		else
			Physics.IgnoreCollision(_sphereCollider, _collider);
	}
	void pushIn()
	{
			Debug.Log("test2");
		playerInteraction.pushPosition = transform.position;
	}
	void OnDisable()
	{
		playerInteraction.pushPosition = Vector3.zero;
	}
}
