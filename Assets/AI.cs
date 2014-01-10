using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour 
{


	public bool usingAStar;
	public float distanceUntilFade = 3;
	public float fadeSpeed = 5;
	public float turnSpeed = 10;
	public float walkSpeed = 5;
	public float runSpeed = 10;
	//flocking algorithm
	public Boids boids = new Boids ();

	private float timer = 0;
	private Vector3 randLoc = Vector3.zero;
	private AIState state;
	private float currentSpeed;
	public float CurrentSpeed { get { return currentSpeed; } set { currentSpeed = value; } }

	private const float DISTANCE_FROM_SELF_FOR_RANDOM = 4; 
	private const float DISTANCE_UNTIL_GO_TO_IDLE = 1;

	public bool Idle { get; set; }

	void Start()
	{
		currentSpeed = runSpeed;
	}




	void Update()
	{
		switch (state)
		{
		case AIState.Idle:

			timer -= Time.deltaTime;

			if(!Idle)
			{
				timer = Random.Range(0f, 2f);
				Idle = true;
			}

			if(timer <= 0)
			{
				state = usingAStar ? AIState.AStarWander : AIState.Wander;
				Idle = false;
			}
			break;

		case AIState.AStarWander:
			break;

		case AIState.Wander:
			if(randLoc == Vector3.zero)
				randLoc = new Vector3(Random.insideUnitCircle.x, transform.position.y, Random.insideUnitCircle.y) * DISTANCE_FROM_SELF_FOR_RANDOM;
			
			randLoc.y = transform.position.y;

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(randLoc - transform.position), Time.deltaTime * turnSpeed);

			currentSpeed = Mathf.Lerp (currentSpeed, Vector3.Distance(randLoc, transform.position) <= distanceUntilFade ? walkSpeed : runSpeed, Time.deltaTime * fadeSpeed);

			if(Vector3.Distance(transform.position, randLoc) <= DISTANCE_UNTIL_GO_TO_IDLE)
			{
				randLoc = Vector3.zero;
				state = AIState.Idle;
			}
			break;

		}

		boids.Update (this);

		//movement controller
		if (!Idle)rigidbody.AddRelativeForce (Vector3.forward * currentSpeed);

		if (rigidbody.velocity.magnitude > currentSpeed * .3f && currentSpeed > walkSpeed) 
						rigidbody.velocity = transform.forward * currentSpeed;


		//steering Controller  
		if (Physics.Raycast (transform.position, transform.forward, 2)) {
			{
				var direction = transform.right;
				if(Physics.Raycast(transform.position, (transform.forward + transform.right).normalized, 2))
				{
					direction = -direction;

					if(Physics.Raycast(transform.position, (transform.forward - transform.right).normalized, 2))
						direction = -transform.forward; 
				}
				transform.rotation = Quaternion.Slerp(Transform.rotation, Quaternion.LookRotation (direction), Time.deltaTime * turnSpeed);
			}
    }
}

public enum AIState
{
	Idle,
	AStarWander,
	Wander

} ;



	