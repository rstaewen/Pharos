using UnityEngine;
using System.Collections.Generic;

public class ExpandOnClick : MonoBehaviour
{
	public Transform foldedXform;
	public UISprite spriteContainer;
	public bool startFolded = true;
	private enum FoldState {folded, unfolding, folding, unfolded}
	private FoldState state;
	private Vector3 endScale;
	private Vector3 endPosition;
	private Vector3 smoothVelocity = Vector3.zero;
	private Vector3 smoothScale = Vector3.zero;
	public float unfoldTime = 1f;
	private Vector3 foldScale = new Vector3(0.1f, 0.1f, 0.1f);
	private Vector3 depthOffset = new Vector3(0f,0f,-50f);

	void Awake()
	{
		endScale = foldedXform.localScale;
		endPosition = foldedXform.localPosition;
		if(startFolded)
		{
			state = FoldState.folded;
			foldedXform.position = spriteContainer.transform.position;
			foldedXform.localScale = foldScale;
			foldedXform.gameObject.SetActive(false);
		}
	}

	void FixedUpdate()
	{
		switch(state)
		{
		case FoldState.folding:
			foldedXform.localPosition = Vector3.SmoothDamp(foldedXform.localPosition, spriteContainer.transform.localPosition+depthOffset, ref smoothVelocity, unfoldTime*0.3f);
			foldedXform.localScale = Vector3.SmoothDamp(foldedXform.localScale, foldScale, ref smoothScale, unfoldTime*0.3f);
			break;
		case FoldState.unfolding:
			foldedXform.localPosition = Vector3.SmoothDamp(foldedXform.localPosition, endPosition, ref smoothVelocity, unfoldTime*0.3f);
			foldedXform.localScale = Vector3.SmoothDamp(foldedXform.localScale, endScale, ref smoothScale, unfoldTime*0.3f);
			break;
		default:
			break;
		}
	}

	public void Fold()
	{
		switch(state)
		{
		case FoldState.folded: case FoldState.folding:
			state = FoldState.unfolding;
			smoothVelocity = -smoothVelocity;
			smoothScale = -smoothScale;
			foldedXform.gameObject.SetActive(true);
			break;
		case FoldState.unfolded: case FoldState.unfolding:
			state = FoldState.folding;
			smoothVelocity = -smoothVelocity;
			smoothScale = -smoothScale;
			break;
		default:
			break;
		}
	}

	void Finish()
	{
		switch(state)
		{
		case FoldState.folding:
			state = FoldState.folded;
			foldedXform.gameObject.SetActive(false);
			break;
		 case FoldState.unfolding:
			state = FoldState.unfolded;
			break;
		default:
			break;
		}
	}
}
