using UnityEngine;

[AddComponentMenu("NGUI/Examples/Item Attachment Point")]
public class InvAttachmentPoint : MonoBehaviour
{
	/// <summary>
	/// Item slot that this attachment point covers.
	/// </summary>

	public InvBaseItem.Slot slot;

	GameObject mPrefab;
	GameObject mChild;

	/// <summary>
	/// Attach an instance of the specified game object.
	/// </summary>

	public GameObject Attach (GameObject prefab)
	{
		if (mPrefab != prefab)
		{
			mPrefab = prefab;

			// Remove the previous child
			if (mChild != null) Destroy(mChild);

			// If we have something to create, let's do so now
			if (mPrefab != null)
			{
				// Create a new instance of the game object
				Transform t = transform;
				Vector3 tempPosition = mPrefab.transform.localPosition;
				Vector3 tempScale = mPrefab.transform.localScale;
				Quaternion tempRotation = mPrefab.transform.localRotation;
				mChild = Instantiate(mPrefab, t.position, t.rotation) as GameObject;
				// Parent the child to this object
				Transform ct = mChild.transform;
				
				ct.parent = t;

				// Reset the pos/rot/scale, just in case
				ct.localPosition = tempPosition;
				ct.localRotation = tempRotation;
				ct.localScale = tempScale;
				
				Debug.Log("ctpos: "+ct.localPosition.ToString()+"ctrot: "+ct.localRotation.ToString()+
					"ctscl: "+ct.localScale.ToString());
			}
		}
		return mChild;
	}
}