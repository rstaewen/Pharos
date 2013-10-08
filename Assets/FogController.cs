using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogController : MonoBehaviour {
	
	public Transform centerTransform;
	public float rotationsPerSecond;
	private Vector3 positionOffset;
	public bool lockToCenterTransform = false;
	private Color baseColor;
	private Vector3 currentRGB = Vector3.zero;
	private Vector3 targetRGB;
	private Vector3 colorVelocity = Vector3.zero;
	private float colorAdjustTime = 10f;
	private List<Material> fogWallMaterialCollection = new List<Material>();
	// Use this for initialization
	void Start ()
	{
		positionOffset = transform.position - centerTransform.position;
		for(int i =0; i< transform.childCount; i++)
		{
			if(transform.GetChild(i).name == "fogCylinder2")
			{
				Material m = transform.GetChild(i).GetComponent<MeshRenderer>().material;
				baseColor = m.GetColor("_TintColor");
				fogWallMaterialCollection.Add(transform.GetChild(i).GetComponent<MeshRenderer>().material);
			}
		}
		InvokeRepeating("newColorTarget", colorAdjustTime, colorAdjustTime);
		targetRGB = new Vector3(baseColor.r, baseColor.g, baseColor.b);
		currentRGB = targetRGB;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(lockToCenterTransform)
			transform.position = centerTransform.position+positionOffset;
		int direction = 1;
		for(int i = 0; i<transform.childCount; i++)
		{
			Quaternion localRot = transform.GetChild(i).localRotation;
			float newY = localRot.eulerAngles.y + direction*(Time.fixedDeltaTime*rotationsPerSecond*360);
			localRot = Quaternion.Euler(localRot.eulerAngles.x, newY, localRot.eulerAngles.z);
			transform.GetChild(i).localRotation = localRot;
			direction=-direction;
		}
		foreach(Material m in fogWallMaterialCollection)
		{
			currentRGB = Vector3.SmoothDamp(currentRGB, targetRGB, ref colorVelocity, colorAdjustTime);
			m.SetColor("_TintColor", new Color(currentRGB.x, currentRGB.y, currentRGB.z, 0f));
		}
	}
	
	void newColorTarget()
	{
		targetRGB.x = baseColor.r * (((float)Random.Range(-20,20)/100f) + 1f);
		targetRGB.y = baseColor.g * (((float)Random.Range(-20,20)/100f) + 1f);
		targetRGB.z = baseColor.b * (((float)Random.Range(-20,20)/100f) + 1f);
	}
}
