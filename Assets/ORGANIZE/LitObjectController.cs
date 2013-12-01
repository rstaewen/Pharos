using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LitObjectController : MonoBehaviour
{
	public List<ObjectController> litObjects = new List<ObjectController>();
	
	void OnTriggerStay(Collider enteringCollider)
	{
		ObjectController objControl = enteringCollider.GetComponent<ObjectController>();
		if (objControl)
			objControl.IsLit = true;
		else
			Physics.IgnoreCollision(collider, enteringCollider);
	}
}
