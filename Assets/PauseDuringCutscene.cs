using UnityEngine;
using System.Collections.Generic;

public class PauseDuringCutscene : MonoBehaviour
{
	public List<Component> controlComponents;
	Animator animator;

	void Awake ()
	{
		animator = GetComponent<Animator>();
		if(enabled)
			Invoke ("PauseControl", 1f);
		else
			animator.SetBool("StartGame", true);
	}

	void Update()
	{
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
		animator.SetBool("StartGame", true);
		SendMessage("StartLevel");
	}
}
