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
	}

	public void PlayRunFootstep(int foot){ playFootstep(foot, footstepRunVolume); }
	public void PlayWalkFootstep(int foot){	playFootstep(foot, footstepWalkVolume);	}

	void playFootstep(int foot, float volume)
	{
		if(isJumping)
			return;
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
