using UnityEngine;
using System.Collections;

public class Journal : MonoBehaviour
{
	private Transform rootTransform;
	// Use this for initialization
	void Start ()
	{
		rootTransform = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButtonDown("Journal"))
			Toggle ();
	}
	
	public void Toggle()
	{
			rootTransform.gameObject.SetActive(!rootTransform.gameObject.activeSelf);
	} 
}
