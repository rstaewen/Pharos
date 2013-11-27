using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LitObjectController : MonoBehaviour
{
	public List<ObjectController> litObjects = new List<ObjectController>();
	
	void OnTriggerEnter(Collider enteringCollider)
	{
		ObjectController objControl = enteringCollider.GetComponent<ObjectController>();
		if (objControl)
			objControl.IsLit = true;
		else
			Physics.IgnoreCollision(collider, enteringCollider);
	}

	void OnTriggerExit(Collider exitingCollider)
	{
		ObjectController objControl = exitingCollider.GetComponent<ObjectController>();
		if (objControl)
			objControl.IsLit = false;
		else
			Physics.IgnoreCollision(collider, exitingCollider);
	}
}
