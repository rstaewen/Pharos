using UnityEngine;
using System.Collections.Generic;

public class MaskPuzzle : MonoBehaviour
{
	private List<ShadowController> shadowCollection = new List<ShadowController>();
	void Start ()
	{
		shadowCollection.AddRange(GetComponentsInChildren<ShadowController>());
	}
	
	void FixedUpdate ()
	{
		foreach(ShadowController controller in shadowCollection)
			if(!controller.isVisible)
				return;
		foreach(ShadowController controller in shadowCollection)
			controller.Kill ();
		this.enabled = false;
		GameObject.Destroy(gameObject, 5f);
	}
}
