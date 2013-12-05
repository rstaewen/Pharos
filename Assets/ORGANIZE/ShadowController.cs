using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ObjectController : MonoBehaviour
{
	protected bool lit = false;
	public virtual bool IsLit {get {return lit; } 
		set {
			lit = value;
			CancelInvoke("resetLight"); 
			Invoke("resetLight",1f);
		}
	}
	
	protected virtual void resetLight()
	{
		lit = false;
	}
	public GlobalStuff.OnEvent OnCompletion;
	public GlobalStuff.OnEvent TriggerEvent;
	
	public virtual void OnClickAction1(){}
	public virtual void OnClickAction2(){}
	public virtual void OnHoldAction1(){}
}

public class ShadowController : ObjectController
{
	public bool bodyInvisible = true;
	private Animator _animator;
	private List<Material> fadeMaterials = new List<Material>();
	private List<Color> transpColors = new List<Color>();
	private float currAlpha = 0f;
	private float alphaVelocity = 0f;
	public float fadeTime = 1.5f;
	public GameObject mask;
	public ParticleSystem smoke;
	[HideInInspector] public bool isVisible = false;
	bool killed = false;
	private float maxEmissionRate;
	public bool DestroyMaskAfterCompletion = true;
	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();
		SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
		foreach(Material m in renderer.materials) if(m.shader.name == "Transparent/VertexLit")
		{
			fadeMaterials.Add(m);
			transpColors.Add(m.color);
		}
		maxEmissionRate = smoke.emissionRate;
		smoke.emissionRate = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		_animator.SetBool("IsAfraid", IsLit);
		int i = 0;
		foreach(Material m in fadeMaterials)
		{
			currAlpha = Mathf.SmoothDamp(currAlpha, (IsLit&&!killed)?1f:0f, ref alphaVelocity, fadeTime);
			transpColors[i] = new Color(transpColors[i].r, transpColors[i].g, transpColors[i].b, currAlpha);
			if(bodyInvisible)
				m.color = transpColors[i];
			i++;
		}
		if((1f - currAlpha) < 0.05f)
		{
			if(!isVisible)
				smoke.emissionRate = maxEmissionRate;
			isVisible = true;
		}
		else
		{
			if(isVisible)
				smoke.emissionRate = 0f;
			isVisible = false;
		}
	}

	public void Kill()
	{
		killed = true;
		Vector3 position = mask.transform.position;
		mask.transform.parent = null;
		mask.AddComponent<Rigidbody>();
		mask.transform.position = position;
		smoke.emissionRate = 2f*maxEmissionRate;
		Invoke("destroy", 1f);
	}

	void destroy()
	{
		if(DestroyMaskAfterCompletion)
		{
			smoke.transform.parent = null;
			smoke.maxParticles = 200;
			smoke.Emit(100);
			GameObject.Destroy(smoke, 2f);
			GameObject.Destroy(mask);
			GameObject.Destroy(gameObject);
		}
		else
		{
			smoke.transform.parent = null;
			smoke.maxParticles = 500;
			smoke.Emit(400);
			GameObject.Destroy(smoke, 2f);
			GameObject.Destroy(gameObject);
		}
	}
}
