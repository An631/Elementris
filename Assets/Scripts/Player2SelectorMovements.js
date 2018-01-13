#pragma strict
var positionHor:int;
var positionVer:int;

function Start () {

}

function Update () {

	if(Input.GetButtonDown("P2Left")){
		
		if(positionHor>0)
		{
			positionHor-=1;
			transform.Translate( -1, 0,0);
		}
	}
	if(Input.GetButtonDown("P2Right")){
		if(positionHor<4)
		{
			positionHor+=1;
			transform.Translate( 1,0,0);
		}
	}
	if(Input.GetButtonDown("P2Up")){
		if(positionVer<10)
		{
			positionVer+=1;
			transform.Translate( 0, 1,0);
		}
	}
	if(Input.GetButtonDown("P2Down")){
		if(positionVer>0)
		{
			positionVer-=1;
			transform.Translate( 0, -1,0);
		}
	}
}