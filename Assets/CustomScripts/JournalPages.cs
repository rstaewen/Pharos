using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JournalPages : MonoBehaviour
{
	[HideInInspector] public int activePageIndex;
	[HideInInspector] public List<JournalPage> acquiredPageCollection = new List<JournalPage>();
	List<JournalPage> pageCollection = new List<JournalPage>();
	/// <summary>
	/// If this is set to -1, journal is not loaded on the level beginning.
	/// </summary>
	[SerializeField] int ActivatePageNumberOnLevelStart = -1;
	void Awake ()
	{
		pageCollection.Clear();
		acquiredPageCollection.Clear();
		for(int i=0; i<transform.childCount; i++)
		{
			JournalPage page = transform.GetChild(i).GetComponent<JournalPage>();
			pageCollection.Add(page);
			if(page.Acquired)
				acquiredPageCollection.Add(page);
			page.gameObject.SetActive(false);
		}
		if(ActivatePageNumberOnLevelStart >= 0)
			activePageIndex = ActivatePageNumberOnLevelStart;
		else
			activePageIndex = 0;
		Activate (activePageIndex);
	}
	
	public void AddPage(int addedPageID, bool MakeAddedPageActive)
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
				if(MakeAddedPageActive)
					Activate(acquiredPageCollection.Count-1);
				return;
			}
		}
	}
	
	void Activate(int newActiveIndex)
	{
		if(newActiveIndex < 0)
			return;
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
