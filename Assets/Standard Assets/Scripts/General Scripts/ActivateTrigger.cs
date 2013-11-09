using UnityEngine;

public class ActivateTrigger : MonoBehaviour {
	public enum Mode {
		Trigger   = 0, // Just broadcast the action on to the target
		Replace   = 1, // replace target with source
		Activate  = 2, // Activate the target GameObject
		Enable    = 3, // Enable a component
		Animate   = 4, // Start animation on target
		Deactivate= 5 // Decativate target GameObject
	}

	/// The action to accomplish
	public Mode onEnter = Mode.Activate;
	public Mode onExit = Mode.Deactivate;
	public float WaitSecondsForExitAction = 0f;
	public float WaitSecondsForEnterAction = 0f;

	/// The game object to affect. If none, the trigger work on this game object
	public Collider triggeringCollider;
	public Object target;
	public GameObject source;
	public int triggerEnterCount = 1;///
	public int triggerExitCount = 1;///
	public bool repeatTriggerEnter = false;
	public bool repeatTriggerExit = false;
	
	void onEnterTrigger () {
		triggerEnterCount--;

		if (triggerEnterCount == 0 || repeatTriggerEnter) {
			Object currentTarget = target != null ? target : gameObject;
			Behaviour targetBehaviour = currentTarget as Behaviour;
			GameObject targetGameObject = currentTarget as GameObject;
			if (targetBehaviour != null)
				targetGameObject = targetBehaviour.gameObject;
		
			switch (onEnter) {
				case Mode.Trigger:
					targetGameObject.BroadcastMessage ("DoActivateTrigger");
					break;
				case Mode.Replace:
					if (source != null) {
						Object.Instantiate (source, targetGameObject.transform.position, targetGameObject.transform.rotation);
						DestroyObject (targetGameObject);
					}
					break;
				case Mode.Activate:
					targetGameObject.active = true;
					break;
				case Mode.Enable:
					if (targetBehaviour != null)
						targetBehaviour.enabled = true;
					break;	
				case Mode.Animate:
					targetGameObject.animation.Play ();
					break;	
				case Mode.Deactivate:
					targetGameObject.active = false;
					break;
			}
		}
	}
	
	void onExitTrigger () {
		triggerExitCount--;

		if (triggerExitCount == 0 || repeatTriggerExit) {
			Object currentTarget = target != null ? target : gameObject;
			Behaviour targetBehaviour = currentTarget as Behaviour;
			GameObject targetGameObject = currentTarget as GameObject;
			if (targetBehaviour != null)
				targetGameObject = targetBehaviour.gameObject;
		
			switch (onExit) {
				case Mode.Trigger:
					targetGameObject.BroadcastMessage ("DoActivateTrigger");
					break;
				case Mode.Replace:
					if (source != null) {
						Object.Instantiate (source, targetGameObject.transform.position, targetGameObject.transform.rotation);
						DestroyObject (targetGameObject);
					}
					break;
				case Mode.Activate:
					targetGameObject.active = true;
					break;
				case Mode.Enable:
					if (targetBehaviour != null)
						targetBehaviour.enabled = true;
					break;	
				case Mode.Animate:
					targetGameObject.animation.Play ();
					break;	
				case Mode.Deactivate:
					targetGameObject.active = false;
					break;
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if(other == triggeringCollider)
		{
			if(WaitSecondsForEnterAction > 0f)
			{
				CancelInvoke("onEnterTrigger");
				Invoke ("onEnterTrigger", WaitSecondsForEnterAction);
			}
			else
				onEnterTrigger ();
		}
		else
			Physics.IgnoreCollision(collider, other);
	}
	
	void OnTriggerExit (Collider other) {
		if(other == triggeringCollider)
		{
			if(WaitSecondsForExitAction > 0f)
			{
				CancelInvoke("onExitTrigger");
				Invoke ("onExitTrigger", WaitSecondsForExitAction);
			}
			else
				onExitTrigger ();
		}
		else
			Physics.IgnoreCollision(collider, other);
	}
}