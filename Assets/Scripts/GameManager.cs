using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject heroCharacter;

    //Positions
    public Vector3 nextHeroPosition;
    public Vector3 lastHeroPosition; //For battle

    //Scenes
    public string sceneToLoad;
    public string lastScene; //for battle

	// Use this for initialization
	void Awake () {
		//check if instance exists alrady
        if(instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //if it exists, but not this one
        else if (instance!=this)
        {
            //destroy
            Destroy(gameObject);
        }
        //set this to be not destroyable
        DontDestroyOnLoad(gameObject);
        //if no hero is found, make one
        if(!GameObject.Find("HeroCharacter"))
        {
            GameObject Hero = Instantiate(heroCharacter, nextHeroPosition, Quaternion.identity) as GameObject;
            Hero.name = "HeroCharacter";
        }
	}
    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
