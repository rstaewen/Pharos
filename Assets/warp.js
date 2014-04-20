#pragma strict

var playerSpawn : Transform; //drag your spawn point to here in the inspector - probably make it an empty gameobject so you can move it about on the diving board
var warpParticlesEnter : ParticleSystem;
var warpParticlesExit : ParticleSystem;
var warpDelay = 0f;
private var warpTarget : Collider;
	
function OnTriggerEnter(other : Collider)
{
    if (other.tag == "Player")
    {
    	warpTarget = other;
    	if(warpParticlesEnter == null || warpParticlesExit == null) FindParticles();
   		PlayEffect(warpParticlesEnter, warpTarget.transform.position, 500);
   		//if there's no delay, just warp immediately.
   		//if there is a delay, play warp enter particles and then warp.
    	if(warpDelay == 0f)
    		Warp();
    	else {
    		warpTarget.GetComponent("CustomThirdPersonController").SendMessage("SetImmobile");
    		Invoke("Warp", warpDelay);
    	}
    }
}

function Warp()
{
	warpTarget.transform.position = playerSpawn.position;
   	PlayEffect(warpParticlesExit, warpTarget.transform.position, 500);
    warpTarget.GetComponent("CustomThirdPersonController").SendMessage("SetMobile");
}

function FindParticles()
{
	var objs = GameObject.FindGameObjectsWithTag("GlobalUseParticles");

   	for(var i = 0; i< objs.Length; i++)
   	{
   		if (objs[i].name == "WarpEnter")
   			warpParticlesEnter = objs[i].GetComponent("ParticleSystem");
   		if (objs[i].name == "WarpExit")
   			warpParticlesExit = objs[i].GetComponent("ParticleSystem");
   	}
}
function PlayEffect(playParticles : ParticleSystem, playPosition : Vector3, emitCount : int)
{
	playParticles.transform.position = playPosition;
	//playParticles.Play();
	playParticles.Emit(emitCount);
	var src : AudioSource = playParticles.GetComponent("AudioSource");
	src.Play();
}