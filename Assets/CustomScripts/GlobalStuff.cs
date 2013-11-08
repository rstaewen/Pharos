﻿using UnityEngine;
using System.Collections;

public class GlobalStuff : MonoBehaviour
{
	public delegate void OnEvent(bool startEvent, string text);
	public OnEvent OnPuzzleCompletion;
	
	// Use this for initialization
	void Start ()
	{
		OnPuzzleCompletion+=CompletePuzzle;
		GameObject[] puzzleObjects = GameObject.FindGameObjectsWithTag("puzzle");
		foreach(GameObject puzzle in puzzleObjects)
		{
			puzzle.GetComponent<ObjectController>().OnCompletion += OnPuzzleCompletion;
		}
	}
	
	void CompletePuzzle(bool startMusic, string text)
	{
		print (text);
		if(startMusic)
			GetComponent<SoundControl>().StartMusic();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButton("Quit"))
			Application.Quit();
		if (Input.GetButton("Pause"))
		{
				Debug.Break();
			//add real pause later
		}
	}
}