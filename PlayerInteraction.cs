using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
	public float MaxCursorDistance = 50;
	public Transform cameraTransform;
	private Transform lookCursor;
	private LayerMask LSObjectsMask;
	private LayerMask TerrainMask;
	private LayerMask DefaultMask;
	private LayerMask WaterMask;
	private LayerMask PlayerMask;
	// Use this for initialization
	void Start ()
	{
		Screen.showCursor = false;
		lookCursor = transform.FindChild("Cursor");
		OnDisable();
		LSObjectsMask = 1<<9;
		TerrainMask = 1<<8;
		DefaultMask = 0;
		WaterMask = 1<<4;
		PlayerMask = 1<<11;
		enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		RaycastHit hit = new RaycastHit();
		Ray _ray = new Ray(cameraTransform.position, cameraTransform.forward);
		if (
			Physics.Raycast(_ray, out hit, MaxCursorDistance, LSObjectsMask) ||
			Physics.Raycast(_ray, out hit, MaxCursorDistance, TerrainMask) ||
			Physics.Raycast(_ray, out hit, MaxCursorDistance, DefaultMask) ||
			Physics.Raycast(_ray, out hit, MaxCursorDistance, WaterMask)
			)
		{
			Debug.DrawLine(hit.point, cameraTransform.position, Color.red);
			lookCursor.position = hit.point;
		}
	}
	
	void OnEnable()
	{
		if (lookCursor)
			lookCursor.gameObject.SetActive(true);
	}
	
	void OnDisable()
	{
		lookCursor.gameObject.SetActive(false);
	}
}
