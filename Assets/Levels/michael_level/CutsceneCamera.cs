using UnityEngine;
using System.Collections;

public class CutsceneCamera : MonoBehaviour
{	
	public float TimeToDestroy = 11f;
	public GameObject NotifyObjectOnCompletion;
	public string functionToCall;
	public Camera nextCamera;
	public Light playerLight;

	void Start () {
		if(!NotifyObjectOnCompletion)
			Invoke ("DestroyCamera", TimeToDestroy);
		if(nextCamera)
		{
			nextCamera.enabled = false;
			foreach(Camera cam in nextCamera.transform.GetComponentsInChildren<Camera>())
				cam.enabled = false;
		}
	}
	
	void DestroyCamera(){
		Destroy (this.gameObject, 1f);
	}

	void OnCompletion()
	{
		if(playerLight && nextCamera)
		{
			playerLight.transform.parent = nextCamera.transform;
			playerLight.transform.localRotation = Quaternion.identity;
			playerLight.transform.localPosition = Vector3.zero;
			nextCamera.enabled = true;
			foreach(Camera cam in nextCamera.transform.GetComponentsInChildren<Camera>())
				cam.enabled = true;
		}
		if(NotifyObjectOnCompletion)
		{
			FogBank bank = GetComponentInChildren<FogBank>();
			if(bank)
			{
				bank.transform.parent = null;
				bank.centerTransform = NotifyObjectOnCompletion.transform;
			}
			DestroyCamera();
			NotifyObjectOnCompletion.SendMessage(functionToCall);
		}
	}
}
