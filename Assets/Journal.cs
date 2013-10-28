using UnityEngine;
using System.Collections;

public class Journal : MonoBehaviour
{
	private Transform rootTransform;
	public JournalPages journalPages;
	public enum BehaviorOnPageAcquisition {Nothing, TurnToNewPage, TurnToPageAndOpenJournal}
	public BehaviorOnPageAcquisition behaviorOnPageAcquisition 
		= BehaviorOnPageAcquisition.TurnToPageAndOpenJournal;
	public AudioClip OnPagePickup;
	public AudioClip OnTurnPage;
	AudioSource audioSrc;
	// Use this for initialization
	void Start ()
	{
		rootTransform = transform.GetChild(0);
		audioSrc = GetComponent<AudioSource>();
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
	
	public void AddPage(int ID)
	{
		switch(behaviorOnPageAcquisition)
		{
		case BehaviorOnPageAcquisition.Nothing:
			journalPages.AddPage(ID, false);
			break;
		case BehaviorOnPageAcquisition.TurnToNewPage:
			journalPages.AddPage(ID, true);
			break;
		case BehaviorOnPageAcquisition.TurnToPageAndOpenJournal:
			journalPages.AddPage(ID, true);
			rootTransform.gameObject.SetActive(true);
			break;
		}
		if(OnPagePickup)
			audioSrc.PlayOneShot(OnPagePickup);
	}
	
	public void NextPage()
	{
		if(journalPages.NextPage())
			if(OnTurnPage)
				audioSrc.PlayOneShot(OnTurnPage);
	}
	
	public void PrevPage()
	{
		if(journalPages.LastPage())
			if(OnTurnPage)
				audioSrc.PlayOneShot(OnTurnPage);
	}
}
