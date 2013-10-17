using UnityEngine;
using System.Collections;

public abstract class ObjectController : MonoBehaviour
{
	protected bool lit = false;
	public virtual bool IsLit {get {return lit; } set {lit = value; CancelInvoke("resetLight"); Invoke("resetLight", 1f);}}
	
	protected virtual void resetLight()
	{
		lit = false;
	}
	public GlobalStuff.OnEvent OnCompletion;
	
	public virtual void OnClickAction1(){}
	public virtual void OnClickAction2(){}
	public virtual void OnHoldAction1(){}
}

public class ShadowController : ObjectController
{
	private Animator _animator;
	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		_animator.SetBool("IsAfraid", IsLit);
	}
}
