//will make the camera begin at a point on the screen, follow the rock, and stop at a point on the screen
//we dont want to camera to follow the rock too far

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform player; //the object we are going to follow
	public Transform farLeft; //the far left of the game area
	public Transform farRight; //the far right of the game area
	private Vector3 startPosition;
	private bool resettingCamera = false;
	private float speed = 6;

	void Start(){
		startPosition = transform.localPosition;
		FindPlayer();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
	
	}
	
	void FixedUpdate () {

		if(!GameManager.instance.PlayerDead){
			if(GameManager.instance.PlayerReleased)
				lerpSmooth(player);
		}
		else{
			FindPlayer();
			//print("The camera should have moved...");
		}
	}

	private void followPlayer(){
		Vector3 newPosition = transform.position; //get a reference to the position of the camera
		newPosition.x = player.position.x; //set the newPosition to be position of the butter
		//we want to clamp the value of the newPosition between the left and right bounds
		newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
		transform.position = newPosition; //apply the transformation to move the camera
		//NOTE that we only want to do the x, if we did y as well, then the camera would go up and down
	}

	private void lerpSmooth(Transform target){
		float interpolation = speed * Time.deltaTime;
        Vector3 position = this.transform.position;
        position.x = Mathf.Lerp(this.transform.position.x + 1f, target.transform.position.x, interpolation);
        this.transform.position = position;
	}

	private void lerpToNewPlayer(Transform target){
		float interpolation = speed * Time.deltaTime;
        Vector3 position = this.transform.position;
        position.x = Mathf.Lerp(this.transform.position.x + 1f, target.transform.position.x, interpolation);
        this.transform.position = position;
		if((target.transform.position).magnitude < 0.001f){
			GameManager.instance.setOnSlingshot();
			GameManager.instance.PlayerRespawn();
		}
	}

	private void ResetCamera(){
		FindPlayer();
	}

	private void FindPlayer(){
		player = GameObject.FindGameObjectWithTag("Player").transform;
		lerpSmooth(player);
		
	}


}
