//this will make us reload the level if we hold down the R key

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Resetter : MonoBehaviour {

	public Rigidbody2D projectile;
	public float resetSpeed = 0.05f;

	private float resetSpeedSqr;
	private SpringJoint2D spring;

	// Use this for initialization
	void Start () {
		//resetSpeedSqr = resetSpeed * resetSpeed;
		spring = projectile.GetComponent<SpringJoint2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)){
			Reset ();
		}

		//if(spring == null && projectile.velocity.sqrMagnitude < resetSpeedSqr){
		//	Reset ();
		//}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.GetComponent<Rigidbody2D>() == projectile){
			Reset ();
		}
	}

	void Reset(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().name); //will load the current level
	}
}
