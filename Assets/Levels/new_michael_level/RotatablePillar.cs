using UnityEngine;
using System.Collections;

public class RotatablePillar : ObjectController
{
	public enum PillarStateTypes{A,B,C,D, COUNT}
	public float angleToRotateOnClick = 90;
	public float rotationTime = 1f;
	public PillarStateTypes pillarState;
	
	private Vector3 rotation = Vector3.zero;
	private Vector3 targetRotation = Vector3.zero;
	private Vector3 rotationVelocity = Vector3.zero;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		rotation = Vector3.SmoothDamp(rotation, targetRotation, ref rotationVelocity, rotationTime);
		transform.localRotation = Quaternion.Euler(rotation);
	}
	
	public override void OnClickAction1()
	{
		targetRotation.y+=angleToRotateOnClick;
		Invoke("setPillarState", rotationTime);
	}
	
	private void setPillarState()
	{
		pillarState = pillarState +1;
		if(pillarState == PillarStateTypes.COUNT)
			pillarState = PillarStateTypes.A;
	}
}
