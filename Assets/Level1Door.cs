using UnityEngine;
using System.Collections;

public abstract class TriggeredDoor : ObjectController {
	
	public float doorOpenTime = 2f;
	public TriggerArea OpenTrigger;
	public Transform doorHinge;
	private float currentDoorRotation;
	private float startingDoorRotation;
	private float finalDoorRotation;
	protected float rotationVelocity = 0f;
	public enum doorState {closed, open, opening, closing}
	protected doorState state = doorState.closed;
	// Use this for initialization
	protected virtual void Start ()
	{
		OpenTrigger.OnTrigger += openDoor;
		OpenTrigger.OnTriggerLeave += closeDoor;
		startingDoorRotation = doorHinge.localRotation.eulerAngles.y;
		currentDoorRotation = startingDoorRotation;
		finalDoorRotation = currentDoorRotation-90;
	}
	
	protected virtual void openDoor(Collider _collider)
	{
		rotationVelocity = 0f;
		state = doorState.opening;
	}
	
	protected virtual void closeDoor(Collider _collider)
	{
		rotationVelocity = 0f;
		state = doorState.closing;
	}
	
	// Update is called once per frame
	protected virtual void FixedUpdate ()
	{
		switch(state)
		{
		case doorState.opening:
			currentDoorRotation = Mathf.SmoothDampAngle(currentDoorRotation, finalDoorRotation, ref rotationVelocity, doorOpenTime);
			doorHinge.localRotation = Quaternion.Euler(new Vector3(doorHinge.localRotation.eulerAngles.x, currentDoorRotation, doorHinge.localRotation.eulerAngles.z));
			if(currentDoorRotation == finalDoorRotation)
				state = doorState.open;
			break;
		case doorState.closing:
			currentDoorRotation = Mathf.SmoothDampAngle(currentDoorRotation, startingDoorRotation, ref rotationVelocity, doorOpenTime);
			doorHinge.localRotation = Quaternion.Euler(new Vector3(doorHinge.localRotation.eulerAngles.x, currentDoorRotation, doorHinge.localRotation.eulerAngles.z));
			if(currentDoorRotation == startingDoorRotation)
				state = doorState.closed;
			break;
		default:
			break;
		}
	}
}

public class Level1Door : TriggeredDoor
{
	private float currentAlpha;
	public float fakeWallFadeTime = 2f;
	private float alphaVelocity = 0f;
	private Color baseColor;
	public GameObject fakeTranspBarrier;
	private Material fakeTranspMaterial;
	public CharacterController characterController;
	bool fading = false;
	bool active = false;
	
	protected override void Start()
	{
		base.Start();
		fakeTranspMaterial = fakeTranspBarrier.GetComponent<MeshRenderer>().material;
		baseColor = fakeTranspMaterial.color;
		currentAlpha = baseColor.a;
	}
	
	protected override void openDoor(Collider _collider)
	{
		if (_collider == characterController.collider && !active)
			Invoke("startWallFade", 5f);
	}
	protected override void closeDoor(Collider _collider)
	{
		if(active)
			return;
		CancelInvoke("startWallFade");
		base.closeDoor(_collider);
	}
	
	void startWallFade()
	{
		fading = true;
	}
	
	protected override void FixedUpdate()
	{
		if(fading)
		{
			currentAlpha = Mathf.SmoothDamp(currentAlpha, 0f, ref alphaVelocity, fakeWallFadeTime);
			baseColor.a = currentAlpha;
			fakeTranspMaterial.color = baseColor;
		}
		if(currentAlpha <= 0.05f && !active)
		{
			Destroy(fakeTranspBarrier);
			fading = false;
			active = true;
			state = doorState.opening;
			rotationVelocity = 0f;
		}
		base.FixedUpdate();
	}
}
