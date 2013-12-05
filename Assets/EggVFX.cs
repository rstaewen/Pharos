using UnityEngine;
using System.Collections.Generic;

public class EggVFX : MonoBehaviour
{
	public MeshRenderer skinRenderer;
	public MeshRenderer coreRenderer;
	private Material skinMaterial;
	private Material coreMaterial;

	float distortion;
	float distortionTarget;
	float minDistortion = 0f;
	float maxDistortion = 10f;
	float distortionVelocity = 0f;
	float distortionTime = 1f;

	Color emissionAlphaColor;
	float emissionAlpha;
	float emissionAlphaTarget;
	float minEmissionAlpha = 0.6f;
	float maxEmissionAlpha = 1f;
	float emissionAlphaVelocity = 0f;
	float emissionAlphaTime = 4f;

	void Start ()
	{	
		skinMaterial = skinRenderer.material;
		coreMaterial = coreRenderer.material;

		distortionTarget = maxDistortion;
		distortion = minDistortion;

		emissionAlphaTarget = maxEmissionAlpha;
		emissionAlpha = minEmissionAlpha;

		InvokeRepeating("SetDistortionTarget", distortionTime, distortionTime);
		InvokeRepeating("SetAlphaTarget", emissionAlphaTime, emissionAlphaTime);
	}

	void SetDistortionTarget()
	{
		distortionTarget = (distortionTarget==maxDistortion)? minDistortion : maxDistortion;
	}

	void SetAlphaTarget()
	{
		emissionAlphaTarget = (emissionAlphaTarget==maxEmissionAlpha)? minEmissionAlpha : maxEmissionAlpha;
	}
	
	void FixedUpdate ()
	{
		distortion = Mathf.SmoothDamp(distortion, distortionTarget, ref distortionVelocity, distortionTime*0.3f);
		emissionAlpha = Mathf.SmoothDamp(emissionAlpha, emissionAlphaTarget, ref emissionAlphaVelocity, emissionAlphaTime*0.3f);

		skinMaterial.SetFloat("_BumpAmt", distortion);
		emissionAlphaColor.r = emissionAlpha;
		emissionAlphaColor.g = emissionAlpha;
		emissionAlphaColor.b = emissionAlpha;
		emissionAlphaColor.a = emissionAlpha;
		coreMaterial.SetColor("_Color", emissionAlphaColor);
	}
}
