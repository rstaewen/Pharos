using UnityEngine;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
 public class Boids 
{
	public string tagToSearch;
	public float searchRadius;

	public AI leader;
	public List<AI> boidGroup = new List<AI>();

	public BoidState boidState = BoidState.Ignore;
	public float seperationRadius;

	public void Update(AI ai)
	{
		var goa = GameObject.FindGameObjectsWithTag (tagToSearch);

		foreach (var gameObject in goa.Where<GameObject>(gameObject => Vector3.Distance(gameObject.transform.position, ai.transform.position) <=searchRadius).Where(gameObject =>gameObject.GetComponent<AI>() && !boidGroup.Contains (gameObject.GetComponent<AI>()))) {
						boidGroup.Add (gameObject.GetComponent<AI> ());
				}

		if (leader == null && boidGroup.Count > 0) {
				
			leader = boidGroup[Random.Range (0, boidGroup.Count)];
			foreach (var boid in boidGroup)
			{
				if(boid.boids.leader != leader && boid.boids.leader != null) leader = boid.boids.leader;
			}
		}
		if (leader == ai) 
		{
			boidState = BoidState.Ignore;
			foreach (var boid in boidGroup.Where (boid => boid.boids.leader !=ai).Where (boid => boid.boids.leader == boid || boid.boids.leader == null))
				boid.boids.leader = ai;
			return;
		}

		foreach (var boid in boidGroup)
						boid.boids.leader = leader;
		if (leader != null) {
			//ai.aStar = null; 
			if(boidState == BoidState.Ignore)
				boidState = 0;
		}
		//else ai.astar = null;

		ai.Idle = leader.Idle;
		switch (boidState) {
				
			case BoidState.Alignment:
			ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, leader.transform.rotation, Time.deltaTime * ai.turnSpeed);
			ai.CurrentSpeed = leader.CurrentSpeed;

			if(Vector3.Distance(ai.transform.position, leader.transform.position) <= seperationRadius / 2)
			boidState = BoidState.Seperation;

			if(Vector3.Distance(ai.transform.position, leader.transform.position) >= seperationRadius)
				boidState = BoidState.Cohesion;


				
			break;


			case BoidState.Cohesion:
			ai.CurrentSpeed = ai.runSpeed;
			ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, Quaternion.LookRotation(leader.transform.position - leader.transform.forward * -1), Time.deltaTime * ai.turnSpeed);

			if(Vector3.Distance(ai.transform.position, leader.transform.position) <= seperationRadius - .5f)
				boidState = 0;
			break;

			case BoidState.Seperation:
			ai.CurrentSpeed = ai.runSpeed;
			ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, Quaternion.LookRotation(ai.transform.position - leader.transform.position), Time.deltaTime * ai.turnSpeed);


		    if(Vector3.Distance(ai.transform.position, leader.transform.position) >= seperationRadius /2)
				boidState = 0;
			break;

			case BoidState.Ignore:
			break;
			}

		foreach (var boid in boidGroup)
		{
			if(Vector3.Distance (boid.transform.position, ai.transform.position) <= seperationRadius / 2)
				ai.rigidbody.AddForce((ai.transform.position - boid.transform.position)*ai.runSpeed * Time.deltaTime);
		}

	}

}

public enum BoidState
{
	Alignment = 0,
	Cohesion = 1,
	Seperation = 2,
	Ignore = -1
} ;
