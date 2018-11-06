using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	private bool playerDead = false;
	private bool gameOver = false;
	private bool playerReleased = false;

	public bool PlayerDead {
		get {return playerDead; }
	}

	public bool GameOver {
		get {return gameOver; }
	}

	public bool PlayerReleased {
		get {return playerReleased; }
	} 

	public void weeee(){
		playerReleased = true;
	}

	public void setOnSlingshot(){
		playerReleased = false;
	}

	public void PlayerSplat(){
		playerDead = true;
	}

	public void PlayerRespawn(){
		playerDead = false;
	}

	public void OutOfLives(){
		gameOver = true;
	}

	void Awake(){
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
