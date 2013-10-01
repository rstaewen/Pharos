﻿//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// A UI script that keeps an eye on the slot in character equipment.
/// </summary>

[AddComponentMenu("NGUI/Examples/UI Equipment Slot")]
public class UIEquipmentSlot : UIItemSlot
{
	public InvEquipment equipment;
	public InvBaseItem.Slot slot;

	override protected InvGameItem observedItem
	{
		get
		{
			return (equipment != null) ? equipment.GetItem(slot) : null;
		}
	}

	/// <summary>
	/// Replace the observed item with the specified value. Should return the item that was replaced.
	/// </summary>

	override protected InvGameItem Replace (InvGameItem item)
	{
		string msg = "replacing if not null. equipment is: ";
		if (equipment == null)
			msg += "null!";
		else
			msg += equipment.name;
		Debug.Log(msg);
		return (equipment != null) ? equipment.Replace(slot, item) : item;
	}
	override protected InvGameItem ReplaceExisting (InvGameItem item)
	{
		string msg = "replacing if not null. equipment is: ";
		if (equipment == null)
			msg += "null!";
		else
			msg += equipment.name;
		Debug.Log(msg);
		return (equipment != null) ? equipment.Replace(slot, item) : item;
	}
}