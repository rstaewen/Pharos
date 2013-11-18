using UnityEngine;
using System.Collections.Generic;

public class DetachableObject : MonoBehaviour
{
	Vector3 baseLocalPosition;
	Quaternion baseLocalRotation;
	Transform startingParent;
	bool hasAwoken = false;
	void Awake ()
	{
		startingParent = transform.parent;
		baseLocalPosition = transform.localPosition;
		baseLocalRotation = transform.localRotation;
		hasAwoken = true;
	}

	void OnEnable()
	{
		if(!hasAwoken)
			Awake();
		transform.parent = null;
	}

	void OnDisable()
	{
		transform.parent = startingParent;
		transform.localPosition = baseLocalPosition;
		transform.localRotation = baseLocalRotation;
	}
}
