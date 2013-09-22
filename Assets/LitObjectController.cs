using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LitObjectController : MonoBehaviour
{
	public List<ObjectController> litObjects = new List<ObjectController>();
	// Use this for initialization
	void Start ()
	{
		
	}
	
	void OnTriggerEnter(Collider enteringCollider)
	{
		ObjectController objControl = enteringCollider.GetComponent<ObjectController>();
		if (objControl)
		{
				objControl.IsLit = true;
		}
		else
		{
			Physics.IgnoreCollision(collider, enteringCollider);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}
