using UnityEngine;
using System.Collections.Generic;


public class PlayerEffects : MonoBehaviour
{
	public List<FootstepEffect> footstepEffectCollection;
	private LevelSettings.LevelAmbientTemperature temperature;
	public LevelSettings.FootstepGroundTypes currentFootsteps;
	public AudioSource footstepAudioSource;
	public Transform leftFootTransform;
	public Transform rightFootTransform;
	float moveSpeed = 0f;
	bool isJumping = false;
	public AudioClip landingSFX;
	public AudioClip jumpingSFX;
	public float footstepRunVolume = 1.0f;
	public float footstepWalkVolume = 0.6f;
	public ParticleSystem breathParticles;
	public float breathParticleEmission;
	private float playParticles = 1f;

	void Awake()
	{
		if(breathParticleEmission!=0f)
			Invoke("breathe", 1f);
	}

	public void SetFootsteps( List<FootstepEffect> footstepEffectCollection)
	{
		Debug.Log("setting footsteps...");
		this.footstepEffectCollection = footstepEffectCollection;
		foreach(FootstepEffect effect in footstepEffectCollection)
		{
			for(int i = 0; i<leftFootTransform.childCount; i++)
				if(leftFootTransform.GetChild(i).name.ToLower() == effect.particlesName.ToLower())
					effect.leftDustParticles = leftFootTransform.GetChild(i).GetComponent<ParticleSystem>();
			for(int i = 0; i<rightFootTransform.childCount; i++)
				if(rightFootTransform.GetChild(i).name.ToLower() == effect.particlesName.ToLower())
					effect.rightDustParticles = rightFootTransform.GetChild(i).GetComponent<ParticleSystem>();
		}
	}

	public void SetTemperature (LevelSettings.LevelAmbientTemperature levelAmbientTemperature)
	{
		Debug.Log("setting temperature...");
		temperature = levelAmbientTemperature;
		setBreath();
	}

	void setBreath()
	{
		switch(temperature)
		{
		case LevelSettings.LevelAmbientTemperature.Freezing:
			breathParticleEmission = breathParticles.emissionRate;
			break;
		case LevelSettings.LevelAmbientTemperature.Cold:
			breathParticleEmission = breathParticles.emissionRate*0.5f;
			break;
		case LevelSettings.LevelAmbientTemperature.Normal:
			breathParticleEmission = 0f;
			break;
		case LevelSettings.LevelAmbientTemperature.Hot:
			breathParticleEmission = 0f;
			break;
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
	}

	void breathe()
	{
		playParticles = playParticles==0f ? 1f : 0f;
		Invoke("breathe", (playParticles==1f)?(0.75f-(moveSpeed/12f)):3f/(moveSpeed+1f));
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
