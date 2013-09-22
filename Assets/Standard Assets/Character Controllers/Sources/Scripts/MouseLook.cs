using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -30F;
	public float maximumX = 30F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;
	float rotationX = 0F;
	
	private float timeLeft = 1f;
	public float zoomTime = 1f;
	private bool zooming = false;
	
	private Vector3 cameraVelocity = Vector3.zero;
	private Vector3 cameraRotationalVelocity = Vector3.zero;
	public float distanceOffset = 2f;
	Transform shoulderTransform;
	Transform shoulderPos;
	Transform playerTransform;
	Transform neckTransform;
	Quaternion baseNeckRotation;

	private MonoBehaviour interactionScript;
	
	public void SetNeckTransform(Transform neckTransform)
	{
		this.neckTransform = neckTransform;
		baseNeckRotation = neckTransform.localRotation;
	}
	public void SetShoulderPos(Transform shoulderPos)
	{
		playerTransform = shoulderPos.parent;
		this.shoulderTransform = shoulderPos;
		interactionScript = (playerTransform.GetComponent("PlayerInteraction") as MonoBehaviour);
	}
	public void StartZoom()
	{
		timeLeft = zoomTime;
		zooming = true;
		CancelInvoke("StopZoom");
		Invoke("StopZoom", zoomTime);
	}
	void StopZoom()
	{
		zooming = false;
		interactionScript.enabled = true;
		transform.parent = shoulderTransform;
		transform.localRotation = Quaternion.identity;
		transform.localPosition = Vector3.zero;
		cameraVelocity = Vector3.zero;
		cameraRotationalVelocity = Vector3.zero;
	}

	void FixedUpdate ()
	{
		if (zooming)
		{
			transform.position = Vector3.SmoothDamp(transform.position, shoulderTransform.position, ref cameraVelocity, timeLeft);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, shoulderTransform.rotation, 360);
			timeLeft -= Time.fixedDeltaTime;
			return;
		}
		if (axes == RotationAxes.MouseXAndY)
		{
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			rotationX = Mathf.Clamp (rotationX, minimumX, maximumX);
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			neckTransform.localEulerAngles = new Vector3(-rotationX, 0, rotationY)+baseNeckRotation.eulerAngles;
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
	
	public void Reset()
	{
		neckTransform.localRotation = baseNeckRotation;
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
}