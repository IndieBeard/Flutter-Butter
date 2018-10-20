using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	public ScoreTarget scoreTarget;
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (scoreTarget.landed){
			anim.SetTrigger ("success");
		}
	}
}