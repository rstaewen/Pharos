using UnityEngine;
using System.Collections;

public class MazeEnd : MonoBehaviour {
	public GameObject endCamera;
	private MazeEnd mazeEnd;
	
	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player"))
		{
			Instantiate (endCamera, endCamera.transform.position, endCamera.transform.rotation);
			mazeEnd = (MazeEnd) GetComponent("MazeEnd");
			mazeEnd.enabled = false;
		}
	}
	
}
