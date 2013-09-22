using UnityEngine;
using System.Collections;

public class EquippableItem : MonoBehaviour
{
	public GameObject prefab;
	public int ItemID;
	public Collider interactionZone;
	public Light lightScript;
	public LitObjectController litObjectControl;
	// Use this for initialization
	void Start ()
	{
		interactionZone = recursiveSeekCollider(transform);
		lightScript = GetComponentInChildren<Light>();
		if(lightScript)
			litObjectControl = lightScript.GetComponentInChildren<LitObjectController>();
	}
	Collider recursiveSeekCollider(Transform currentTransform)
	{
		Collider thisCollider = GetComponent<Collider>();
		if(thisCollider)
			return thisCollider;
		else
			for (int i = 0; i< currentTransform.childCount; i++)
			{
				Collider childCollider = recursiveSeekCollider(currentTransform.GetChild(i));
				if (childCollider)
					return childCollider;
			}
		return null;
	}
	// Update is called once per frame
	void Update ()
	{
	
	}
}
