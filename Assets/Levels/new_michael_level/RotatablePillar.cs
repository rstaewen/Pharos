using UnityEngine;
using System.Collections;

public class RotatablePillar : ObjectController
{
	public enum PillarStateTypes{A,B,C,D, COUNT}
	public float angleToRotateOnClick = 90;
	public float rotationTime = 1f;
	public PillarStateTypes pillarState;
	public AudioClip pillarNoise;
	
	protected Vector3 rotation = Vector3.zero;
	protected Vector3 targetRotation = Vector3.zero;
	private Vector3 rotationVelocity = Vector3.zero;
	private int numberRotations = 0;
	// Use this for initialization
	protected virtual void Awake()
	{
	}
	protected virtual void Start ()
	{
	}
	
	//return number of rotations.  0= start, 1=90 
	
	public int rotationCount()
	{
		return numberRotations%4;
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
		numberRotations++;
		Debug.Log(numberRotations);
		audio.PlayOneShot(pillarNoise);
	}
	
	protected virtual void setPillarState()
	{
		pillarState = pillarState +1;
		if(pillarState == PillarStateTypes.COUNT)
			pillarState = PillarStateTypes.A;
	}
}
