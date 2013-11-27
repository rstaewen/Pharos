using UnityEngine;
using System.Collections.Generic;

public class Flicker : MonoBehaviour
{
	public GameObject TorchLight;
	public float MaxLightIntensity;
	public float IntensityLight;
	
	
	void Start () {
		TorchLight.light.intensity=IntensityLight;
	}
	
	
	void Update () {
//		if (IntensityLight<0) IntensityLight=0;
//		if (IntensityLight>MaxLightIntensity) IntensityLight=MaxLightIntensity;		
//		
//		TorchLight.light.intensity=IntensityLight/2f+Mathf.Lerp(IntensityLight-0.1f,IntensityLight+0.1f,Mathf.Cos(Time.time*30));
	}
}
