using UnityEngine;
using System.Collections.Generic;

public class Bob : MonoBehaviour
{
	private Vector3 baseRotation;
	private Vector3 targetRotation;
	public float waveTime;
	public float maxRotationDisplacement;
	private Vector3 smoothVelocity = Vector3.zero;
	int direction = 1;

	void Start ()
	{
		baseRotation = transform.localRotation.eulerAngles;
		targetRotation = baseRotation;
		targetRotation.z += maxRotationDisplacement/2f;
		InvokeRepeating("newTargetRotation", waveTime/2f, waveTime/3f);
	}
	
	void FixedUpdate ()
	{
		transform.localRotation = Quaternion.Euler(Vector3.SmoothDamp(
			transform.localRotation.eulerAngles, targetRotation, ref smoothVelocity, waveTime*0.3f));
	}

	void newTargetRotation()
	{
		int random = Random.Range(0,2);
		switch(random)
		{
		case 0:
			direction = -direction;
			targetRotation = baseRotation;
			targetRotation.z += (direction*maxRotationDisplacement);
			break;
		default:
			break;
		}
	}
}
