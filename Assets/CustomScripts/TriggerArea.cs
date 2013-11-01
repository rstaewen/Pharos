using UnityEngine;
using System.Collections;

public class TriggerArea : MonoBehaviour {
	
	public delegate void TriggerFunction(Collider _collider);
	public TriggerFunction OnTrigger;
	public TriggerFunction OnTriggerLeave;
	
	void OnTriggerEnter(Collider _collider)
	{
		Debug.Log("trigger enter");
		OnTrigger(_collider);
	}
	
	void OnTriggerExit(Collider _collider)
	{
		Debug.Log("trigger exit");
		OnTriggerLeave(_collider);
	}
}
