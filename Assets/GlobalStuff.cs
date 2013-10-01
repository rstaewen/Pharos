using UnityEngine;
using System.Collections;

public class GlobalStuff : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButton("Quit"))
			Application.Quit();
		if (Input.GetButton("Pause"))
		{
				Debug.Break();
			//add real pause later
		}
	}
}
