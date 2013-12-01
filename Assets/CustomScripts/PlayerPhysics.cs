using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {
	
	public Vector3 pushPosition = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(pushPosition != Vector3.zero)
		{
			transform.position = (pushPosition - transform.position)*0.02f + transform.position;
		}
	}
	
	public void Freeze()
	{
		(transform.GetComponent("CustomThirdPersonController") as MonoBehaviour).Invoke("SetUncontrollable", 0f);
	}
}
