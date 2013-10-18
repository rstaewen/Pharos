//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Equip the specified items on the character when the script is started.
/// </summary>

[AddComponentMenu("NGUI/Examples/Equip Items")]
public class LoadInventory : MonoBehaviour
{
	public int[] itemIDs;
	public UIItemStorage satchel;
	
	void Start ()
	{
		if (itemIDs != null && itemIDs.Length > 0)
		{
			int qualityLevels = (int)InvGameItem.Quality._LastDoNotUse;

			for (int i = 0, imax = itemIDs.Length; i < imax; ++i)
			{
				int index = itemIDs[i];
				InvBaseItem item = InvDatabase.FindByID(index);

				if (item != null)
				{
					InvGameItem gi = new InvGameItem(index, item);
					gi.quality = (InvGameItem.Quality)Random.Range(0, qualityLevels);
					gi.itemLevel = NGUITools.RandomRange(item.minItemLevel, item.maxItemLevel);
					satchel.PlaceItemInNextAvailableSlot(gi);
				}
				else
				{
					Debug.LogWarning("Can't resolve the item ID of " + index);
				}
			}
		}
		Destroy(this);
	}
}