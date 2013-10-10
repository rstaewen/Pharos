using UnityEngine;
using System.Collections;

public abstract class TriggeredDoor : ObjectController {
	
	public float doorOpenTime = 2f;
	public float doorCloseTime = 1f;
	public TriggerArea OpenTrigger;
	public TriggerArea CloseTrigger;
	public Transform doorHinge;
	private float currentDoorRotation;
	private float startingDoorRotation;
	private float finalDoorRotation;
	protected float rotationVelocity = 0f;
	public enum doorState {closed, open, opening, closing}
	protected doorState state = doorState.closed;
	public CharacterController characterController;
	// Use this for initialization
	protected virtual void Start ()
	{
		OpenTrigger.OnTrigger += openDoor;
		OpenTrigger.OnTriggerLeave += closeDoor;
		CloseTrigger.OnTrigger += closeDoorOverride;
		CloseTrigger.OnTriggerLeave += closeDoorStub;
		startingDoorRotation = doorHinge.localRotation.eulerAngles.y;
		currentDoorRotation = startingDoorRotation;
		finalDoorRotation = currentDoorRotation-90;
	}
	
	protected virtual void openDoor(Collider _collider)
	{
		if (_collider == characterController.collider)
		{
			rotationVelocity = 0f;
			state = doorState.opening;
		}
	}
	
	protected virtual void closeDoor(Collider _collider)
	{
		if (_collider == characterController.collider)
		{
			rotationVelocity = 0f;
			state = doorState.closing;
		}
	}
	
	protected void closeDoorOverride(Collider _collider)
	{
		if (_collider == characterController.collider)
		{
			rotationVelocity = 0f;
			state = doorState.closing;
		}
	}
	
	protected void closeDoorStub(Collider _collider){}
	
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
			currentDoorRotation = Mathf.SmoothDampAngle(currentDoorRotation, startingDoorRotation, ref rotationVelocity, doorCloseTime);
			doorHinge.localRotation = Quaternion.Euler(new Vector3(doorHinge.localRotation.eulerAngles.x, currentDoorRotation, doorHinge.localRotation.eulerAngles.z));
			if(currentDoorRotation == startingDoorRotation)
			{
				state = doorState.closed;
				OnClosed();
			}
			break;
		default:
			break;
		}
	}
	
	protected virtual void OnClosed(){}
}

public class Level1Door : TriggeredDoor
{
	private float currentAlpha;
	public float fakeWallFadeTime = 2f;
	private float alphaVelocity = 0f;
	private Color baseColor;
	public GameObject fakeTranspBarrier;
	private Material fakeTranspMaterial;
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
		if (_collider == characterController.collider)
		{
			if(active)
				return;
			CancelInvoke("startWallFade");
			base.closeDoor(_collider);
		}
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
	protected override void OnClosed() {if(active){characterController.transform.GetComponent<PlayerPhysics>().Freeze();}}
}
