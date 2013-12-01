using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogWallController : ObjectController
{
	private Color baseColor;
	private Vector3 currentRGB = Vector3.zero;
	private Vector3 targetRGB;
	private Vector3 colorVelocity = Vector3.zero;
	private float colorAdjustTime = 10f;
	private List<Material> fogWallMaterialCollection = new List<Material>();
	private List<FogCollider> lightColliderCtlCollection = new List<FogCollider>();
	private bool isLit = false;
	public override bool IsLit {get {return isLit;} set{if(!isLit) {colorVelocity = Vector3.zero;} isLit = value;}}
	
	// Use this for initialization
	void Start ()
	{
		for(int i =0; i< transform.childCount; i++)
		{
			if(transform.GetChild(i).name == "fogCylinder2")
			{
				Material m = transform.GetChild(i).GetComponent<MeshRenderer>().material;
				baseColor = m.GetColor("_TintColor");
				fogWallMaterialCollection.Add(transform.GetChild(i).GetComponent<MeshRenderer>().material);
			}
			else if (transform.GetChild(i).name == "LightColliders")
			{
				Transform lightCollTrans = transform.GetChild(i);
				for(int j = 0; j<lightCollTrans.childCount; j++)
				{
					lightColliderCtlCollection.Add(lightCollTrans.GetChild(j).GetComponent<FogCollider>());
					lightColliderCtlCollection[lightColliderCtlCollection.Count-1].fogWall = this;
				}
			}
		}
		InvokeRepeating("newColorTarget", colorAdjustTime, colorAdjustTime);
		targetRGB = new Vector3(baseColor.r, baseColor.g, baseColor.b);
		currentRGB = targetRGB;
	}

	void OnTriggerStay(Collider other)
	{
		if(other.GetComponent<CharacterController>())
			other.transform.position += transform.forward*0.1f;
	}
	
	public void UnlightColliders()
	{
		IsLit = false;
		foreach(ObjectController controller in lightColliderCtlCollection)
		{
			IsLit = controller.IsLit||IsLit;
		}
	}
	void FixedUpdate ()
	{
		if(!IsLit)
		{
			foreach(Material m in fogWallMaterialCollection)
			{
				currentRGB = Vector3.SmoothDamp(currentRGB, targetRGB, ref colorVelocity, colorAdjustTime);
				m.SetColor("_TintColor", new Color(currentRGB.x, currentRGB.y, currentRGB.z, 0f));
			}
		}
		else
		{
			if(Vector3.Distance(currentRGB, Vector3.zero)<0.01f)
			{
				OnCompletion(true, false, "fog wall escaped");
				Destroy(gameObject);
			}
			else
				foreach(Material m in fogWallMaterialCollection)
				{
					currentRGB = Vector3.SmoothDamp(currentRGB, Vector3.zero, ref colorVelocity, 3f);
					m.SetColor("_TintColor", new Color(currentRGB.x, currentRGB.y, currentRGB.z, 0f));
				}
		}
	}
	
	void OnDisable()
	{
		/////TODO: send sound event for puzzle completion
	}
	
	void newColorTarget()
	{
		targetRGB.x = baseColor.r * (((float)Random.Range(-20,20)/100f) + 1f);
		targetRGB.y = baseColor.g * (((float)Random.Range(-20,20)/100f) + 1f);
		targetRGB.z = baseColor.b * (((float)Random.Range(-20,20)/100f) + 1f);
	}
}
