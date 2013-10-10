using UnityEngine;
using System.Collections;

public class rotateScript : MonoBehaviour
{
	public Vector3 rotation = Vector3.zero;
	public Vector3 targetRotation = Vector3.zero;
	private Vector3 rotationVelocity = Vector3.zero;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			targetRotation.y += 90;
		}
		rotation = Vector3.SmoothDamp(rotation, targetRotation, ref rotationVelocity, 1f);
		transform.localRotation = Quaternion.Euler(rotation);
	}
}
