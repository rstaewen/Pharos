using UnityEngine;
using System.Collections.Generic;

[System.Serializable] public class FootstepEffect
{
	public string name;
	public string particlesName;
	public List<int> SplatmapIndices = new List<int>();
	public ParticleSystem rightDustParticles;
	public ParticleSystem leftDustParticles;
	public List<AudioClip> footstepSFX = new List<AudioClip>();
}

public class LevelSettings : MonoBehaviour
{
	public List<Transform> spawnLocations;
	public enum SpawnLocation {Spawn1, Spawn2, Spawn3, Spawn4}
	public SpawnLocation activeSpawn;
	public List<FootstepEffect> footstepEffectCollection = new List<FootstepEffect>();
	public enum FootstepGroundTypes {grass, dirt, sand, wood, stone, COUNT}
	private Transform playerXform;
	private Transform playerCameraXform;
	private PlayerEffects playerFX;
	public enum LevelAmbientTemperature {Hot, Normal, Cold, Freezing}
	public LevelAmbientTemperature levelAmbientTemperature;

	void Reset()
	{
		for(int i = 0; i<(int)(FootstepGroundTypes.COUNT); i++)
		{
			FootstepEffect effect = new FootstepEffect();
			effect.name = ((FootstepGroundTypes)i).ToString();
			effect.particlesName = ((FootstepGroundTypes)i).ToString();
			footstepEffectCollection.Add(effect);
		}
	}
	public void Set ()
	{
		Debug.Log("setting level settings");
		playerXform = GameObject.FindGameObjectWithTag("Player").transform;
		playerCameraXform = GameObject.FindGameObjectWithTag("MainCamera").transform;
		playerFX = playerXform.GetComponent<PlayerEffects>();
		playerFX.SetFootsteps(footstepEffectCollection);
		playerFX.SetTemperature(levelAmbientTemperature);
		playerXform.position = spawnLocations[(int)activeSpawn].position;
		playerXform.rotation = spawnLocations[(int)activeSpawn].rotation;
		playerXform.localScale = spawnLocations[(int)activeSpawn].localScale;
	}
}
