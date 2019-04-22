using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {
    public AudioClip levelMusic;
	// Use this for initialization
	void Awake () {
        if (levelMusic && SoundManager.instance)
        {
            Debug.Log("level music play");
            SoundManager.instance.playMusic(levelMusic, 0.6f);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
