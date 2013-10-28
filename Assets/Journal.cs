using UnityEngine;
using System.Collections;

public class Journal : MonoBehaviour
{
	private Transform rootTransform;
	public JournalPages journalPages;
	// Use this for initialization
	void Start ()
	{
		rootTransform = transform.GetChild(0);
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
		journalPages.AddPage(ID);
	}
	
	public void NextPage()
	{
		journalPages.NextPage();
	}
	
	public void PrevPage()
	{
		journalPages.LastPage();
	}
}
