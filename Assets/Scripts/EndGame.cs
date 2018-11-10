using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (GameManager.instance.LandedOnToast){
			print("Success!");
			anim.SetTrigger ("Success");
		}
	}
}