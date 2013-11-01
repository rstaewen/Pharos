using UnityEngine;
using System.Collections;

public class FogCollider : ObjectController
{
	public FogWallController fogWall;
	public override bool IsLit {get {return lit; } set {lit = value; fogWall.IsLit = true; CancelInvoke("resetLight"); Invoke("resetLight", 1f);}}
	// Use this for initialization
	void Start ()
	{
	}
	
	protected override void resetLight()
	{
		lit = false;
		fogWall.UnlightColliders();
	}
}
