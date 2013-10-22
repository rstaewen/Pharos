using UnityEngine;
using System.Collections;

public class CatchPlayer : MonoBehaviour {
	
	public AudioClip audioClip;
	private float resetAfterDeathTime = 5f;
	
	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			Destroy (other.gameObject);
			AudioSource gg = this.gameObject.GetComponent<AudioSource>();
			gg.Stop();
			AudioSource.PlayClipAtPoint(audioClip, transform.position);
			Invoke ("DoReset", resetAfterDeathTime);
		}
	}
	
	void DoReset()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

}
