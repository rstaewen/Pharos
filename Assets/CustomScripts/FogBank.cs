using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogBank : MonoBehaviour {
	
	public Transform centerTransform;
	public float rotationsPerSecond;
	private Vector3 positionOffset = Vector3.zero;
	public bool lockToCenterTransform = false;
	private List<Transform> fogTransformCollection = new List<Transform>();
	// Use this for initialization
	void Start ()
	{
		if(centerTransform)
			positionOffset = transform.position - centerTransform.position;
		for(int i = 0; i<transform.childCount; i++)
		{
			if(transform.GetChild(i).name.Contains("fog"))
				fogTransformCollection.Add(transform.GetChild(i));
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(lockToCenterTransform)
			transform.position = centerTransform.position+positionOffset;
		int direction = 1;
		foreach(Transform xform in fogTransformCollection)
		{
			Quaternion localRot = xform.localRotation;
			float newY = localRot.eulerAngles.y + direction*(Time.fixedDeltaTime*rotationsPerSecond*360);
			localRot = Quaternion.Euler(localRot.eulerAngles.x, newY, localRot.eulerAngles.z);
			xform.localRotation = localRot;
			direction=-direction;
		}
	}
}
