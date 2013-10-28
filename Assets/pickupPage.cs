using UnityEngine;
using System.Collections;

public class pickupPage : ObjectController
{
	public int pageID;
	public Journal journalController;
	
	public override void OnClickAction1()
	{
		journalController.AddPage(pageID);
		Destroy(gameObject);
	}
}
