using UnityEngine;
using System.Collections;

public class FireEventOnPickup : MonoBehaviour
{
	public ObjectController receivingObject;
	public bool IsPuzzleCompletion = false;
	public bool PlayMusic = false;
	public bool PlaySound = false;
	private GameObject volume_control;

	void Start()
	{
		volume_control = GameObject.FindGameObjectWithTag("volume_control");
	}
	
	public void OnPickup()
	{
		receivingObject.TriggerEvent(false, false, "OpenDoor");
		if(IsPuzzleCompletion)
		{
			receivingObject.OnCompletion(PlayMusic, PlaySound, "Door");
			if(volume_control != null)
				volume_control.SendMessage("PlayOnPuzzleCompletion");
		}
	}
}
