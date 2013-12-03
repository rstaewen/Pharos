using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogBank : MonoBehaviour {
	public bool RotateMaterialInstead = false;
	public Transform centerTransform;
	public float rotationsPerSecond;
	private Vector3 positionOffset = Vector3.zero;
	public bool lockToCenterTransform = false;
	private List<Transform> fogTransformCollection = new List<Transform>();
	[HideInInspector] public List<Material> fogMaterialCollection = new List<Material>();
	// Use this for initialization
	void Start ()
	{
//		if(centerTransform)
                                                                    //			positionOffset = transform.position - centerTransform.position;
		for(int i = 0; i<transform.childCount; i++)
		{
			if(transform.GetChild(i).name.Contains("fog"))
			{
				fogTransformCollection.Add(transform.GetChild(i));
				fogMaterialCollection.Add(transform.GetChild(i).GetComponent<MeshRenderer>().material);

			}
		}
		if(RotateMaterialInstead)
		{
			float i = 0f;
			foreach(Material m in fogMaterialCollection)
			{
				m.SetTextureOffset("_MainTex", new Vector2(i/(float)fogMaterialCollection.Count, 0f));
				i = i+1f;
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(lockToCenterTransform)
			transform.position = centerTransform.position+positionOffset;
		int direction = 1;
		if(RotateMaterialInstead)
		{
			foreach(Material m in fogMaterialCollection)
			{
				m.SetTextureOffset("_MainTex", m.GetTextureOffset("_MainTex") + new Vector2(direction*(Time.fixedDeltaTime*rotationsPerSecond),0f));
				direction=-direction;
			}
		}
		else
		{
			foreach(Transform xform in fogTransformCollection)
			{
				Quaternion localRot = xform.localRotation;
				float newY = localRot.eulerAngles.y + direction*(Time.fixedDeltaTime*rotationsPerSecond*360);
				localRot = Quaternion.Euler(localRot.eulerAngles.x, newY, localRot.eulerAngles.z);
				xform.localRotation = localRot;
				direction=-direction;
			}
		}
	}
}
