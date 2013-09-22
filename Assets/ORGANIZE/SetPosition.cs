using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour
{
	public bool DayNightCycle = false;
	public Transform worldRotationCenter;
	private Vector3 initialOffset;
	private Vector3 currentOffset;
	// Use this for initialization
	void Start ()
	{
		if (worldRotationCenter == null)
			worldRotationCenter = transform.FindChild("WorldCenter");
		initialOffset = transform.position - worldRotationCenter.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!DayNightCycle)
			transform.position = worldRotationCenter.position+initialOffset;
	}
}
