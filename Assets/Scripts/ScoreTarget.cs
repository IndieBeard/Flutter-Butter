using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreTarget : MonoBehaviour {

	public Text scoreText;
	public Image success;
	private int score;
	public bool landed;

	Animator anim;

	// Use this for initialization
	void Start () {
		score = 0;
		scoreText.text = "Score: " + score.ToString ();
		anim = GetComponent<Animator>();
		landed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Toast")) {
			score += 100;
			scoreText.text = "Score: " + score.ToString ();
			landed = true;
		}
	}
}
