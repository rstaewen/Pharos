
// Require a character controller to be attached to the same game object
@script RequireComponent(CharacterController)

private var _animator : Animator;
private var _controller : CharacterController;

enum CharacterState {
	Idle = 0,
	Walking = 1,
	Trotting = 2,
	Running = 3,
	Jumping = 4,
}

private var playerEffects : MonoBehaviour;

private var _characterState : CharacterState;

// The speed when walking
var walkSpeed = 2.0;
// after trotAfterSeconds of walking we trot with trotSpeed
var trotSpeed = 4.0;
// when pressing "Fire3" button (cmd) we start running
var runSpeed = 6.0;

var inAirControlAcceleration = 3.0;

// How high do we jump when pressing jump and letting go immediately
var jumpHeight = 0.5;

// The gravity for the character
var gravity = 20.0;
// The gravity in controlled descent mode
var speedSmoothing = 10.0;
var rotateSpeed = 500.0;
var trotAfterSeconds = 3.0;

var canJump = true;

private var jumpRepeatTime = 0.05;
private var jumpTimeout = 0.15;
private var groundedTimeout = 0.25;

// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
private var lockCameraTimer = 0.0;

// The current move direction in x-z
private var moveDirection = Vector3.zero;
// The current vertical speed
private var verticalSpeed = 0.0;
// The current x-z move speed
private var moveSpeed = 0.0;

// The last collision flags returned from controller.Move
private var collisionFlags : CollisionFlags; 

// Are we jumping? (Initiated with jump button and not grounded yet)
private var jumping = false;
private var jumpingReachedApex = false;

// Are we moving backwards (This locks the camera to not do a 180 degree spin)
public var movingBack : boolean = false;
// Is the user pressing any keys?
private var isMoving = false;
// When did the user start walking (Used for going into trot after a while)
private var walkTimeStart = 0.0;
// Last time the jump button was clicked down
private var lastJumpButtonTime = -10.0;
// Last time we performed a jump
private var lastJumpTime = -1.0;

public var cameraTransform : Transform;


// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
private var lastJumpStartHeight = 0.0;


private var inAirVelocity = Vector3.zero;

private var lastGroundedTime = 0.0;
private var TerrainMask : LayerMask;

private var isControllable = true;
private var isMobile = true;

public var WorldCenter : Transform;

function Awake ()
{
	moveDirection = transform.TransformDirection(Vector3.forward);
	_animator = GetComponentInChildren(Animator);
	_controller = GetComponent(CharacterController);
	playerEffects = GetComponent("PlayerEffects");
	TerrainMask = 1<<8;
}

function Drown()
{
	var cameraOffset = cameraTransform.position - transform.position;
	transform.position = (WorldCenter.position - transform.position).normalized*10f + transform.position;
	cameraTransform.position = transform.position + cameraOffset + new Vector3(0f,2f,0f);
	transform.position.y = Terrain.activeTerrain.SampleHeight(transform.position)+2f;
}

var pushPower = 2.0;
function OnControllerColliderHit (hit : ControllerColliderHit)
{
    var body : Rigidbody = hit.collider.attachedRigidbody;
 
    // no rigidbody
    if (body == null || body.isKinematic) { return; }
 
    // We dont want to push objects below us
    if (hit.moveDirection.y < -0.3) { return; }
 
    // Calculate push direction from move direction,
    // we only push objects to the sides never up and down
    var pushDir = Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);
 
    // If you know how fast your character is trying to move,
    // then you can also multiply the push velocity by that.
 
    // Apply the push
    body.velocity = pushDir * pushPower;
}

function UpdateSmoothedMovementDirection ()
{
	var grounded = IsGrounded();
	
	// Forward vector relative to the camera along the x-z plane	
	var forward = cameraTransform.TransformDirection(Vector3.forward);
	forward.y = 0;
	forward = forward.normalized;

	// Right vector relative to the camera
	// Always orthogonal to the forward vector
	var right = Vector3(forward.z, 0, -forward.x);

	var v = Input.GetAxisRaw("Vertical");
	var h = Input.GetAxisRaw("Horizontal");

	// Are we moving backwards or looking backwards
	if (v < -0.2)
		movingBack = true;
	else
		movingBack = false;
	
	var wasMoving = isMoving;
	isMoving = Mathf.Abs (h) > 0.1 || Mathf.Abs (v) > 0.1;
		
	// Target direction relative to the camera
	var targetDirection = h * right + v * forward;
	
	// Grounded controls
	if (grounded)
	{
		// Lock camera for short period when transitioning moving & standing still
		lockCameraTimer += Time.deltaTime;
		if (isMoving != wasMoving)
			lockCameraTimer = 0.0;

		// We store speed and direction seperately,
		// so that when the character stands still we still have a valid forward direction
		// moveDirection is always normalized, and we only update it if there is user input.
		if (targetDirection != Vector3.zero)
		{
			var oldDirection = moveDirection;
			// If we are really slow, just snap to the target direction
			if (moveSpeed < walkSpeed * 0.9 && grounded)
			{
				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 10);
				moveDirection = moveDirection.normalized;
			}
			// Otherwise smoothly turn towards it
			else
			{
				var angle = Quaternion.Angle(Quaternion.LookRotation(targetDirection), Quaternion.LookRotation(moveDirection));
				var powAngle = 	Mathf.Pow(angle,0.2);
				var angleTurnSpeed = rotateSpeed*(1/moveSpeed);
				var calcSpeed = angleTurnSpeed*	powAngle;
				var adjusted = calcSpeed * Mathf.Deg2Rad * Time.deltaTime;
				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection,  adjusted, 0.1);
				moveDirection = moveDirection.normalized;
			}
			 
			// get a numeric angle for each vector, on the X-Z plane (relative to world forward)
			var angleA = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
			var angleB = Mathf.Atan2(oldDirection.x, oldDirection.z) * Mathf.Rad2Deg;
			 
			// get the signed difference in these angles
			var angleDiff = Mathf.DeltaAngle( angleA, angleB );
			
			if(angleDiff < 1)
				angleDiff = 0;

			_animator.SetFloat("AngularSpeed", -angleDiff);
		}
		else
		{
			_animator.SetFloat("AngularSpeed", 0f);
		}
		
		
		// Smooth the speed based on the current target direction
		var curSmooth = speedSmoothing * Time.deltaTime;
		
		// Choose target speed
		//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
		var targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0);
	
		_characterState = CharacterState.Idle;
		
		// Pick speed modifier
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
		{
			targetSpeed *= runSpeed;
			_characterState = CharacterState.Running;
		}
		else if (Time.time - trotAfterSeconds > walkTimeStart)
		{
			targetSpeed *= trotSpeed;
			_characterState = CharacterState.Trotting;
		}
		else
		{
			targetSpeed *= walkSpeed;
			_characterState = CharacterState.Walking;
		}
		
		if(isMoving)
			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		else
			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth*6);
		
		// Reset walk time start when we slow down
		if (moveSpeed < walkSpeed * 0.3)
			walkTimeStart = Time.time;
	}
	// In air controls
	else
	{
		// Lock camera while in air
		if (jumping)
		{
			lockCameraTimer = 0.0;
		}

		if (isMoving)
			inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
	}
	

		
}


function ApplyJumping ()
{
	// Prevent jumping too fast after each other
	if (lastJumpTime + jumpRepeatTime > Time.time)
		return;

	if (IsGrounded()) {
		// Jump
		// - Only when pressing the button down
		// - With a timeout so you can press the button slightly before landing		
		if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
			verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
			SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
		}
	}
}


function ApplyGravity ()
{
	if (isControllable)	// don't move player at all if not controllable.
	{
		// Apply gravity
		var jumpButton;
		if(isMobile)
			jumpButton = Input.GetButton("Jump");
		else
			jumpButton = false;
		
		
		// When we reach the apex of the jump we send out a message
		if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0)
		{
			jumpingReachedApex = true;
			SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
		}
	
		if (IsGrounded ())
			verticalSpeed *= 0.95f;
		else
			verticalSpeed -= gravity * Time.deltaTime;
	}
}

function SetMobile()
{
	isMobile = true;
	targetDirection = (transform.TransformDirection(Vector3.forward));
	moveDirection = targetDirection;
}

function SetImmobile()
{
	isMobile = false;
}

function SetUncontrollable()
{
	isControllable = false;
}

function CalculateJumpVerticalSpeed (targetJumpHeight : float)
{
	// From the jump height and gravity we deduce the upwards speed 
	// for the character to reach at the apex.
	return Mathf.Sqrt(2 * targetJumpHeight * gravity);
}

function DidJump ()
{
	jumping = true;
	jumpingReachedApex = false;
	lastJumpTime = Time.time;
	lastJumpStartHeight = transform.position.y;
	lastJumpButtonTime = -10;
	
	_characterState = CharacterState.Jumping;
}
function Update() {
	
	if (!isControllable)
	{
		// kill all inputs if not controllable.
		Input.ResetInputAxes();
	}

	if (Input.GetButtonDown ("Jump"))
	{
		lastJumpButtonTime = Time.time;
	}
	
	if(isMobile)
		UpdateSmoothedMovementDirection();
	
	// Apply gravity
	// - extra power jump modifies gravity
	// - controlledDescent mode modifies gravity
	ApplyGravity ();

	// Apply jumping logic
	ApplyJumping ();
	
	// Calculate actual motion
	var movement = moveDirection * moveSpeed + Vector3 (0, verticalSpeed, 0) + inAirVelocity;
	movement *= Time.deltaTime;
	
	// Move the controller
	if (isMobile)
	{
		// ANIMATION sector
		_animator.SetFloat("Speed", moveSpeed);
		if(!IsGrounded())
			_animator.SetFloat("VerticalSpeed", verticalSpeed);
		else
		{
			_animator.SetFloat("VerticalSpeed", 0f);
			SendMessage("SetMoveSpeed", moveSpeed);
		}
		collisionFlags = _controller.Move(movement);
		var downRay : Ray = new Ray();
		downRay.direction = Vector3.down;
		downRay.origin = transform.position;
		var hit : RaycastHit;
		var hitGround = Physics.Raycast(downRay, hit, 1f, TerrainMask);
		if(!jumping&&hitGround)
			_controller.Move(Vector3(0f, 0.5f*(Terrain.activeTerrain.SampleHeight(transform.position) - transform.position.y), 0f));
		_animator.SetBool("Jumping", jumping);
	}
	else
	{
		moveSpeed = 0;
		_animator.SetFloat("Speed", moveSpeed);
		_animator.SetFloat("VerticalSpeed", 0f);
		collisionFlags = _controller.Move(Vector3(0f, Terrain.activeTerrain.SampleHeight(transform.position) - transform.position.y - 0.1f, 0f));
	}
	//"glue" the player to the terrain.
	
	
	// Set rotation to the move direction
	if (isMobile)
	{
		if (IsGrounded())
		{
			transform.rotation = Quaternion.LookRotation(moveDirection);
		}	
		else
		{
			var xzMove = movement;
			xzMove.y = 0;
			if (xzMove.sqrMagnitude > 0.001)
			{
				transform.rotation = Quaternion.LookRotation(xzMove);
			}
		}	
		
		// We are in jump mode but just became grounded
		if (IsGrounded())
		{
			lastGroundedTime = Time.time;
			inAirVelocity = Vector3.zero;
			if (jumping)
			{
				jumping = false;
				SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
				moveSpeed *= 0.2f;
			}
		}
	}
}

function GetSpeed () {
	return moveSpeed;
}

function IsJumping () {
	return jumping;
}

function IsGrounded () {
	return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
}

function GetDirection () {
	return moveDirection;
}

function IsMovingBackwards () {
	return movingBack;
}

function GetLockCameraTimer () 
{
	return lockCameraTimer;
}

function MoveInputActive ()  : boolean
{
	return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f;
}

function IsMoving ()  : boolean
{
	return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5;
}

function HasJumpReachedApex ()
{
	return jumpingReachedApex;
}

function IsGroundedWithTimeout ()
{
	return lastGroundedTime + groundedTimeout > Time.time;
}

function Reset ()
{
	gameObject.tag = "Player";
}

