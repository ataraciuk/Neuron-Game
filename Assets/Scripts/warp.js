
var exit : GameObject;



function OnTriggerEnter(other : Collider) {

other.gameObject.transform.position = exit.transform.position;

}