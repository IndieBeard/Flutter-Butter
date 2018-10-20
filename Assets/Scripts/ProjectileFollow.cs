//will make the camera begin at a point on the screen, follow the rock, and stop at a point on the screen
//we dont want to camera to follow the rock too far

using UnityEngine;
using System.Collections;

public class ProjectileFollow : MonoBehaviour {

	public Transform projectile; //the object we are going to follow
	public Transform farLeft; //the far left of the game area
	public Transform farRight; //the far right of the game area
	
	void Update () {
		Vector3 newPosition = transform.position; //get a reference to the position of the camera
		newPosition.x = projectile.position.x; //set the newPosition to be position of the rock
		//we want to clamp the value of the newPosition between the left and right bounds
		newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
		transform.position = newPosition; //apply the transformation to move the camera
		//NOTE that we only want to do the x, if we did y as well, then the camera would go up and down
	}
}
