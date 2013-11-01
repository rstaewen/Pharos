using UnityEngine;
using System.Collections;

public class WaterLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "MC")
		{
			Debug.Log("Drown!");
			(other.gameObject.GetComponent("CustomThirdPersonController") as MonoBehaviour).Invoke("Drown",0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
