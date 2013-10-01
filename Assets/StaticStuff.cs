using UnityEngine;
using System.Collections;

public class StaticStuff : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButtonDown("Quit"))
				Application.Quit();
	}
}
