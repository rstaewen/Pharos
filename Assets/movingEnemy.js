#pragma strict

var timer : float = 0.0;
var timerMax : float = 1.5;
var randDir : int = 1;
var Hspeed : int = 20;
var person : GameObject;
protected var startHeight   : float;

function Start () {
startHeight = transform.localPosition.y;
}

 
function Update()
{
    if ( timer > timerMax )
    {
        // pick a new direction
        randDir = Random.Range(1,5);
        // make a new random Max time
        timerMax = Random.Range( 0.5, 2.5 );
    }
 
    // movement here
    // ....
    switch( randDir )
    {
        case 1 :
            person.transform.Translate(-Vector2.up*Hspeed*Time.deltaTime);
        break;
        case 2 :
            person.transform.Translate(Vector2.up*Hspeed*Time.deltaTime);
        break;
        case 3 :
            person.transform.Translate(Vector2.right*Hspeed*Time.deltaTime);
        break;
        case 4 :
            person.transform.Translate(-Vector2.right*Hspeed*Time.deltaTime);
        break;
    }
}
