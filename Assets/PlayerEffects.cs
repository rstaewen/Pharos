using UnityEngine;
using System.Collections.Generic;

[System.Serializable] public class FootstepEffect
{
	public string name;
	public ParticleSystem rightDustParticles;
	public ParticleSystem leftDustParticles;
	public List<AudioClip> footstepSFX = new List<AudioClip>();
}

public class PlayerEffects : MonoBehaviour
{
	public List<FootstepEffect> footstepEffectCollection = new List<FootstepEffect>();
	public enum FootstepGroundTypes {grass, dirt, sand, wood, stone, COUNT}
	public FootstepGroundTypes currentFootsteps;
	public AudioSource footstepAudioSource;
	float moveSpeed = 0f;
	bool isJumping = false;
	public AudioClip landingSFX;
	public AudioClip jumpingSFX;
	public float footstepRunVolume = 1.0f;
	public float footstepWalkVolume = 0.6f;
	public ParticleSystem breathParticles;
	private float breathParticleEmission;
	private float playParticles = 1f;

	void Awake()
	{
		breathParticleEmission = breathParticles.emissionRate;
		Invoke("breathe", 1f);
	}

	void Reset()
	{
		for(int i = 0; i<(int)(FootstepGroundTypes.COUNT); i++)
		{
			FootstepEffect effect = new FootstepEffect();
			effect.name = ((FootstepGroundTypes)i).ToString();
			footstepEffectCollection.Add(effect);
		}
	}

	void DidLand()
	{
		isJumping = false;
		footstepAudioSource.PlayOneShot(landingSFX);
		footstepEffectCollection[(int)currentFootsteps].leftDustParticles.Emit(300);
		footstepEffectCollection[(int)currentFootsteps].rightDustParticles.Emit(300);
	}

	void DidJump()
	{
		isJumping = true;
		footstepAudioSource.PlayOneShot(jumpingSFX);
		footstepEffectCollection[(int)currentFootsteps].leftDustParticles.Emit(150);
		footstepEffectCollection[(int)currentFootsteps].rightDustParticles.Emit(150);
	}

	void SetMoveSpeed(float moveSpeed)
	{
		this.moveSpeed = moveSpeed;
		breathParticles.emissionRate = ((moveSpeed/6f)+0.5f)*breathParticleEmission*playParticles;
		Debug.Log("emission: "+breathParticles.emissionRate.ToString());
	}

	void breathe()
	{
		playParticles = playParticles==0f ? 1f : 0f;
		Invoke("breathe", (playParticles==1f)?1f:3f/(moveSpeed+1f));
	}

	public void PlayRunFootstep(int foot){ playFootstep(foot, footstepRunVolume); }
	public void PlayWalkFootstep(int foot){	playFootstep(foot, footstepWalkVolume);	}

	void playFootstep(int foot, float volume)
	{
		int index = Random.Range(0,footstepEffectCollection[(int)currentFootsteps].footstepSFX.Count-1);
		footstepAudioSource.PlayOneShot(footstepEffectCollection[(int)currentFootsteps].footstepSFX[index], volume);
		
		switch(foot)
		{
		case 0:
			footstepEffectCollection[(int)currentFootsteps].leftDustParticles.Emit((int)(moveSpeed*30f));
			break;
		case 1:
			footstepEffectCollection[(int)currentFootsteps].rightDustParticles.Emit((int)(moveSpeed*30f));
			break;
		}
	}
}
