using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotatableNode : RotatablePillar
{
	private Light fireLight;
	private List<ParticleSystem> fireParticles = new List<ParticleSystem>();
	public Material inactiveMaterial;
	public Material activeMaterial;
	public RotatableNodePuzzle puzzleController;
	public Transform leftTransform;
	public Transform rightTransform;
	public Transform upTransform;
	public Transform bottomTransform;
	RotatableNode leftNode;
	RotatableNode rightNode;
	RotatableNode upNode;
	RotatableNode bottomNode;
	public enum NodeType {I,T,L}
	public enum NodeState {Disconnected, Connected}
	public NodeState nodeState = NodeState.Disconnected;
	public NodeType nodeType = NodeType.I;
	List<RotatableNode> neighborNodes = new List<RotatableNode>();
	
	protected override void Awake ()
	{
		base.Awake();
		fireParticles.AddRange(GetComponentsInChildren<ParticleSystem>());
		fireLight = GetComponentInChildren<Light>();
		rotation = transform.localRotation.eulerAngles;
		targetRotation = transform.localRotation.eulerAngles;
		if(leftTransform)	leftNode = leftTransform.GetComponent<RotatableNode>();
		if(rightTransform)	rightNode = rightTransform.GetComponent<RotatableNode>();
		if(upTransform)		upNode = upTransform.GetComponent<RotatableNode>();
		if(bottomTransform)	bottomNode = bottomTransform.GetComponent<RotatableNode>();
		neighborNodes.Add(leftNode);
		neighborNodes.Add(rightNode);
		neighborNodes.Add(upNode);
		neighborNodes.Add(bottomNode);
	}
	protected override void setPillarState ()
	{
		base.setPillarState ();
		puzzleController.TryCompletePuzzle();
	}
	
	public void TryConnectPath(RotatableNode sourceNode)
	{
		switch(nodeType)
		{
		case NodeType.I:
			switch(pillarState)
			{
			case PillarStateTypes.A:
				if(sourceNode == bottomNode)
					Connect(true);
				break;
			case PillarStateTypes.B:
				if(sourceNode == leftNode)
					Connect(true);
				break;
			case PillarStateTypes.C:
				if(sourceNode == upNode)
					Connect(true);
				break;
			case PillarStateTypes.D:
				if(sourceNode == rightNode)
					Connect(true);
				break;
			}
			break;
		case NodeType.L:
			switch(pillarState)
			{
			case PillarStateTypes.A:
				if(sourceNode == upNode || sourceNode == rightNode)
					Connect(true);
				break;
			case PillarStateTypes.B:
				if(sourceNode == rightNode || sourceNode == bottomNode)
					Connect(true);
				break;
			case PillarStateTypes.C:
				if(sourceNode == bottomNode || sourceNode == leftNode)
					Connect(true);
				break;
			case PillarStateTypes.D:
				if(sourceNode == leftNode || sourceNode == upNode)
					Connect(true);	
				break;
			}
			if(nodeState == NodeState.Connected)
				testNearbyL(sourceNode);
			break;
		case NodeType.T:
			switch(pillarState)
			{
			case PillarStateTypes.A:
				if(sourceNode == leftNode || sourceNode == rightNode || sourceNode == bottomNode || sourceNode == null)
					Connect(true);
				break;
			case PillarStateTypes.B:
				if(sourceNode == leftNode || sourceNode == upNode || sourceNode == bottomNode|| sourceNode == null)
					Connect(true);
				break;
			case PillarStateTypes.C:
				if(sourceNode == leftNode || sourceNode == rightNode || sourceNode == upNode|| sourceNode == null)
					Connect(true);
				break;
			case PillarStateTypes.D:
				if(sourceNode == upNode || sourceNode == rightNode || sourceNode == bottomNode|| sourceNode == null)
					Connect(true);	
				break;
			}
			if(nodeState == NodeState.Connected)
				testNearbyT(sourceNode);
			break;
		}
	}
	
	public void Connect(bool doConnect)
	{
		if(doConnect)
		{
			nodeState = NodeState.Connected;
			GetComponent<MeshRenderer>().material = activeMaterial;
			foreach(ParticleSystem system in fireParticles)
				system.emissionRate = 20;
			fireLight.enabled = true;
			//turn on particles/change texture
		}
		else
		{
			nodeState = NodeState.Disconnected;
			GetComponent<MeshRenderer>().material = inactiveMaterial;
			foreach(ParticleSystem system in fireParticles)
				system.emissionRate = 0;
			fireLight.enabled = false;
			//turn off particles/change texture
		}
	}
	
	void testNearbyL(RotatableNode sourceNode)
	{
		switch(pillarState)
		{
		case PillarStateTypes.A:
			if(sourceNode!=null)
			{
				if(rightNode!=null) if(sourceNode!=rightNode)	rightNode.TryConnectPath(this);
				if(upNode!=null) if(sourceNode!=upNode)		upNode.TryConnectPath(this);
			}
			else
			{
				if(rightNode!=null)rightNode.TryConnectPath(this);
				if(upNode!=null)upNode.TryConnectPath(this);
			}
			break;
		case PillarStateTypes.B:
			if(sourceNode!= null)
			{
				if(rightNode!=null)if(sourceNode!=rightNode)	rightNode.TryConnectPath(this);
				if(bottomNode!=null)if(sourceNode!=bottomNode)	bottomNode.TryConnectPath(this);
			}
			else
			{
				if(rightNode!=null)rightNode.TryConnectPath(this);
				if(bottomNode!=null)bottomNode.TryConnectPath(this);
			}
			break;
		case PillarStateTypes.C:
			if(sourceNode!= null)
			{
				if(bottomNode!=null)if(sourceNode!=bottomNode)	bottomNode.TryConnectPath(this);
				if(leftNode!=null)if(sourceNode!=leftNode)	leftNode.TryConnectPath(this);
			}
			else
			{
				if(bottomNode!=null)bottomNode.TryConnectPath(this);
				if(leftNode!=null)leftNode.TryConnectPath(this);
			}
			break;
		case PillarStateTypes.D:
			if(sourceNode!= null)
			{
				if(leftNode!=null)if(sourceNode!=leftNode)	leftNode.TryConnectPath(this);
				if(upNode!=null)if(sourceNode!=upNode)		upNode.TryConnectPath(this);
			}
			else
			{
				if(leftNode!=null)leftNode.TryConnectPath(this);
				if(upNode!=null)upNode.TryConnectPath(this);
			}
			break;
		}
	}
	
	void testNearbyT(RotatableNode sourceNode)
	{
		switch(pillarState)
		{
		case PillarStateTypes.A:
			if(sourceNode!= null)
			{
				if(rightNode!=null)if(sourceNode!=rightNode)	rightNode.TryConnectPath(this);
				if(bottomNode!=null)if(sourceNode!=bottomNode)	bottomNode.TryConnectPath(this);
				if(leftNode!=null)if(sourceNode!=leftNode)	leftNode.TryConnectPath(this);
			}
			else
			{
				if(rightNode!=null)rightNode.TryConnectPath(this);
				if(bottomNode!=null)bottomNode.TryConnectPath(this);
				if(leftNode!=null)leftNode.TryConnectPath(this);
			}
			break;
		case PillarStateTypes.B:
			if(sourceNode!= null)
			{
				if(upNode!=null)if(sourceNode!=upNode)		upNode.TryConnectPath(this);
				if(bottomNode!=null)if(sourceNode!=bottomNode)	bottomNode.TryConnectPath(this);
				if(leftNode!=null)if(sourceNode!=leftNode)	leftNode.TryConnectPath(this);
			}
			else
			{
				if(upNode!=null)upNode.TryConnectPath(this);
				if(bottomNode!=null)bottomNode.TryConnectPath(this);
				if(leftNode!=null)leftNode.TryConnectPath(this);
			}
			break;
		case PillarStateTypes.C:
			if(sourceNode!= null)
			{
				if(rightNode!=null)if(sourceNode!=rightNode)	rightNode.TryConnectPath(this);
				if(upNode!=null)if(sourceNode!=upNode)		upNode.TryConnectPath(this);
				if(leftNode!=null)if(sourceNode!=leftNode)	leftNode.TryConnectPath(this);
			}
			else
			{
				if(rightNode!=null)rightNode.TryConnectPath(this);
				if(upNode!=null)upNode.TryConnectPath(this);
				if(leftNode!=null)leftNode.TryConnectPath(this);
			}
			break;
		case PillarStateTypes.D:
			if(sourceNode!= null)
			{
				if(rightNode!=null)if(sourceNode!=rightNode)	rightNode.TryConnectPath(this);
				if(upNode!=null)if(sourceNode!=upNode)		upNode.TryConnectPath(this);
				if(bottomNode!=null)if(sourceNode!=bottomNode)	bottomNode.TryConnectPath(this);
			}
			else
			{
				if(rightNode!=null)rightNode.TryConnectPath(this);
				if(upNode!=null)upNode.TryConnectPath(this);
				if(bottomNode!=null)	bottomNode.TryConnectPath(this);
			}
			break;
		}
	}
}
