using UnityEngine;
using System.Collections.Generic;

public class PharosDebug : MonoBehaviour
{
	void Start ()
	{	
	}
	
	void Update ()
	{
		if(Input.GetButtonDown("Pause"))
			Debug.Break();
	}
}
