#pragma strict

var spin : float = 0;
var maxUpAndDown         : float = 1;        // amount of meters going up and down
var speed             : float = 50;        // up and down speed
 
protected var angle        : float = -90;       // angle to determin the height by using the sinus
protected var toDegrees     : float = Mathf.PI/180;    // radians to degrees
protected var startHeight   : float;          // height of the object when the script starts
 
 


function Start () {
startHeight = transform.localPosition.y;

}

function Update () {
angle += speed * Time.deltaTime;
    if (angle > 270) angle -= 360;
    transform.localPosition.y = startHeight + maxUpAndDown * (1 + Mathf.Sin(angle * toDegrees)) / 2;
transform.localRotation.eulerAngles = (transform.localRotation.eulerAngles + new Vector3(0f, angle/360f, 0f));
}