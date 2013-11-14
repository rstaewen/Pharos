using UnityEngine;
using System.Collections;

public class FireEventOnPickup : MonoBehaviour
{
	public ObjectController receivingObject;
	public bool IsPuzzleCompletion = false;
	public bool PlayMusic = false;
	public bool PlaySound = false;
	
	public void OnPickup()
	{
		receivingObject.TriggerEvent(false, false, "OpenDoor");
		if(IsPuzzleCompletion)
			receivingObject.OnCompletion(PlayMusic, PlaySound, "Door");
	}
}
