using UnityEngine;
using System.Collections;
using Pathfinding;

public class AI_Pather : MonoBehaviour {
	
	public Transform target;
	
	Seeker seeker;
	Path path;
	int currentWaypoint;
	
	CharacterController characterController;
	
	float maxWaypointDistance = 2f;
	
	float speed = 10f;
	
	void Start()
	{
		seeker = GetComponent<Seeker>();
		seeker.StartPath(transform.position, target.position, OnPathComplete);
		characterController = GetComponent<CharacterController>();
	}
	
	void OnPathComplete(Path p)
	{
		if(!p.error){
			path = p;
			currentWaypoint = 0;
		}
		else{
			Debug.Log (p.error);
		}
	}
	
	void FixedUpdate()
	{
		if(path == null)
		{
			return;
		}
		
		if(currentWaypoint >= path.vectorPath.Length)
		{
			return;
		}
		Vector3 direction = (path.vectorPath[currentWaypoint]-transform.position).normalized * speed; //* Time.fixedDeltaTime;
		characterController.SimpleMove (direction);
		
		if(Vector3.Distance (transform.position, path.vectorPath[currentWaypoint]) < maxWaypointDistance)
		{
			currentWaypoint++;
		}
	}
}
