using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

	[SerializeField]
    private AudioSource sfxSource;                   //Drag a reference to the audio source which will play the sound effects.

	//[SerializeField]
    //private AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.

    public static SoundManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip)
    {
        //Set the clip of our sfxSource audio source to the clip passed in as a parameter.
        sfxSource.clip = clip;

        //Play the clip.
        sfxSource.Play();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
