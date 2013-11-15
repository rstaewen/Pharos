using UnityEngine;
using System.Collections;

public class AudioItem
{
	AudioClip audio;
	float defaultVolume;
}
public class SoundControl : MonoBehaviour
{
	public DictionaryBase musicDictionary;
	public Transform playerTransform;
	public float playMusicDelay = 60f;
	public float musicVolume = 1.0f;
	public float ambienceVolume = 1.0f;
	AudioSource musicSrc;
	AudioSource ambienceSrc;
	AudioSource onPuzzleCompletionSrc;
	
	// Use this for initialization
	void Start ()
	{
		musicSrc = transform.FindChild("Music").GetComponent<AudioSource>();
		ambienceSrc = transform.FindChild("Ambience").GetComponent<AudioSource>();
		onPuzzleCompletionSrc = transform.FindChild("OnPuzzleCompletion").GetComponent<AudioSource>();
		if(playMusicDelay != 0f)
			musicSrc.PlayDelayed(playMusicDelay);
	}
	
	public void StartMusic()
	{
			musicSrc.Play();
	}
	
	public void StopMusic()
	{
			musicSrc.SetScheduledEndTime(2f);
	}
	
	public void PlayOnPuzzleCompletion()
	{
		onPuzzleCompletionSrc.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
