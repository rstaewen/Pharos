using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JournalPages : MonoBehaviour
{
	[HideInInspector] public bool MakeAddedPageActive = false;
	[HideInInspector] public int activePageIndex;
	[HideInInspector] public List<JournalPage> acquiredPageCollection = new List<JournalPage>();
	[HideInInspector] public List<JournalPage> pageCollection = new List<JournalPage>();
	// Use this for initialization
	void Reset ()
	{
		for(int i=0; i<transform.childCount; i++)
		{
			JournalPage page = transform.GetChild(i).GetComponent<JournalPage>();
			pageCollection.Add(page);
			if(page.Acquired)
				acquiredPageCollection.Add(page);
			page.gameObject.SetActive(false);
		}
		if(pageCollection.Count > 0)
			activePageIndex = 0;
		else
			activePageIndex = -1;
		Activate (activePageIndex);
	}
	
	public void AddPage(int addedPageID)
	{
		foreach(JournalPage page in pageCollection)
		{
			if(page.ID == addedPageID)
			{
				page.Acquired = true;
				int i = 0;
				foreach(JournalPage acquiredPage in acquiredPageCollection)
				{
					if(acquiredPage.ID > page.ID)
					{
						acquiredPageCollection.Insert(i, page);
						if(MakeAddedPageActive)
							Activate(i);
						return;
					}
					i++;
				}
				acquiredPageCollection.Add(page);
				return;
			}
		}
	}
	
	void Activate(int newActiveIndex)
	{
		if(newActiveIndex < 0)
			return;
		if(activePageIndex >= 0)
			acquiredPageCollection[activePageIndex].gameObject.SetActive(false);
		acquiredPageCollection[newActiveIndex].gameObject.SetActive(true);
		activePageIndex = newActiveIndex;
	}
	
	public bool NextPage()
	{
		if(activePageIndex < acquiredPageCollection.Count-1)
		{
			Activate (activePageIndex+1);
			return true;
		}
		return false;
	}
	
	public bool LastPage()
	{
		if(activePageIndex > 0)
		{
			Activate (activePageIndex-1);
			return true;
		}
		return false;
	}
}
