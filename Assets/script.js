#pragma strict

var playerSpawn : Transform; //drag your spawn point to here in the inspector - probably make it an empty gameobject so you can move it about on the diving board
 
function OnTriggerEnter(other : Collider)
{
    if (other.tag == "Player")
    {
        other.transform.position = playerSpawn.position;
    }
}