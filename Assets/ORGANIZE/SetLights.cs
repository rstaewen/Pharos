using UnityEngine;
using System.Collections;

public class SetLights : MonoBehaviour
{
	public delegate void UpdateFunc();
	UpdateFunc fixedUpdateFunction;
	public Transform directionalTargetTransform;
	private Transform lightSet;
	// Use this for initialization
	void Start ()
	{
		lightSet = transform.FindChild("Lights");
		if (directionalTargetTransform == null)
			directionalTargetTransform = transform.FindChild("WorldCenter");
		fixedUpdateFunction+=UpdateStub;
		lightSet.rotation = Quaternion.LookRotation(directionalTargetTransform.position - lightSet.position);
	}
	public void SetMobile()
	{
		fixedUpdateFunction+=UpdateWithRotation;
	}
	
	void UpdateStub(){}
	
	void UpdateWithRotation()
	{
		lightSet.rotation = Quaternion.LookRotation(directionalTargetTransform.position - lightSet.position);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		fixedUpdateFunction();
	}
}
