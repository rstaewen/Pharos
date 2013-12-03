using UnityEngine;
using System.Collections.Generic;

public class PauseDuringCutscene : MonoBehaviour
{
	public bool StartWithCustcene = true;
	public List<Component> controlComponents;

	void Awake ()
	{
		if(StartWithCustcene)
			Invoke ("PauseControl", 1f);
	}

	public void PauseControl()
	{
		foreach(MonoBehaviour c in controlComponents)
			c.enabled = false;
	}

	public void ResumeControl()
	{
		foreach(MonoBehaviour c in controlComponents)
			c.enabled = true;
		SendMessage("StartLevel");
	}
}
