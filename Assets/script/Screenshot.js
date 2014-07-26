#pragma strict

var count : Number = 1;

function Update () {
	if (Input.GetKey(KeyCode.P)) {
	    Application.CaptureScreenshot("image" + count +".png",2);
	    count += 1;
	}
}