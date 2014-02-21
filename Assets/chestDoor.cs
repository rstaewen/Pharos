using UnityEngine;
using System.Collections;

public class chestDoor : ObjectController {

	public enum HingeState{Open,Closed,Opening,Closing}
	private HingeState hingeState = HingeState.Closed;
	public float rotationTime = 1f;
	public float angleToRotateOnClick = 90;
	protected Vector3 rotation = Vector3.zero;
	public Vector3 targetRotationOpen = Vector3.zero;
	public Vector3 targetRotationClosed= Vector3.zero;
	private Vector3 rotationVelocity = Vector3.zero;

	void FixedUpdate ()
	{
		switch (hingeState)
		{
		case HingeState.Closing:
			rotation = Vector3.SmoothDamp(rotation, targetRotationClosed, ref rotationVelocity, rotationTime);
			transform.localRotation = Quaternion.Euler(rotation);
			if(rotation == targetRotationClosed)
				hingeState = HingeState.Closed;
			break;
		case HingeState.Opening:
			rotation = Vector3.SmoothDamp(rotation, targetRotationOpen, ref rotationVelocity, rotationTime);
			transform.localRotation = Quaternion.Euler(rotation);
			if(rotation == targetRotationOpen)
				hingeState = HingeState.Open;
			break;
		}
	}

	public override void OnClickAction1()
	{

		switch (hingeState)
		{
		case HingeState.Closed: case HingeState.Closing:
			hingeState = HingeState.Opening;
			break;
		case HingeState.Open: case HingeState.Opening:
			hingeState = HingeState.Closing;
			break;



		}
	}



	protected virtual void Start ()
	{
		hingeState = HingeState.Closed;
	}
}
