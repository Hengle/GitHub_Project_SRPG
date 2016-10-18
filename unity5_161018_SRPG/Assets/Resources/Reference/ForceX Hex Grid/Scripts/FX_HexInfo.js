private var ThisHex : GameObject;
var GridPosition : Vector3;
var IsSelected : boolean;
function Start(){
ThisHex = gameObject;
}

function OnMouseEnter(){
ThisHex.GetComponent.<Renderer>().material.color = Color.red;
}

function OnMouseExit(){
	if(!IsSelected){
		ThisHex.GetComponent.<Renderer>().material.color = Color.white;
	}else{
		ThisHex.GetComponent.<Renderer>().material.color = Color.blue;
	}
}