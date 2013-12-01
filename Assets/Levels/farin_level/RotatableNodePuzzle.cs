using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotatableNodePuzzle : MonoBehaviour
{
	public RotatableNode entryNode;
	List<RotatableNode> I_nodes = new List<RotatableNode>();
	public List<RotatableNode> AllNodes = new List<RotatableNode>();

	public GameObject torchLight;
	public GameObject torchFlame;
	
	void Start()
	{
		RotatableNode[] allNodes = transform.GetComponentsInChildren<RotatableNode>();
		AllNodes.AddRange(allNodes);
		foreach(RotatableNode node in AllNodes)
			if(node.nodeType == RotatableNode.NodeType.I)
				I_nodes.Add(node);
		TryCompletePuzzle();
	}
	public void TryCompletePuzzle()
	{
		foreach(RotatableNode node in AllNodes)
			node.Connect(false);
		entryNode.TryConnectPath(null);
		
		bool complete = true;
		foreach(RotatableNode node in I_nodes)
		{
			if(node.nodeState != RotatableNode.NodeState.Connected)
				complete = false;
		}
		
		if (complete)
		{
			Debug.Log("PUZZLE COMPLETE!");
		}
	}
}
