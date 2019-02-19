using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    //Class random monster
    [System.Serializable]
    public class RegionData
    {
        public string regionName;
        public int maxEnemyAmt = 4; //max amount of enemies in one region
        public List<GameObject> possibleEnemies = new List<GameObject>();
    }

    public List<RegionData> Regions = new List<RegionData>();

    public GameObject heroCharacter;

    //Positions
    public Vector3 nextHeroPosition;
    public Vector3 lastHeroPosition; //For battle

    //Scenes
    public string sceneToLoad;
    public string lastScene; //for battle

    //Bool Box
    public bool isWalking = false;
    public bool canEncounter = false;
    public bool getEncounter = false;

    //enums
    public enum GameStates
    {
        WORLD_STATE,
        TOWN_STATE,
        BATTLE_STATE,
        IDLE
    }
    public GameStates gameState;

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

    void Update()
    {
        switch (gameState)
        {
            case (GameStates.WORLD_STATE):
                if (isWalking)
                {
                    RandomEncounter();
                }
                if (getEncounter)
                {
                    gameState = GameStates.BATTLE_STATE;
                }
                break;
            case (GameStates.TOWN_STATE):

                break;
            case (GameStates.BATTLE_STATE):
                //load battle scene

                //go to idle
                break;
            case (GameStates.IDLE):

                break;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void RandomEncounter()
    {
        if (isWalking && canEncounter)
        {
            if (Random.Range(0, 1000) < 10)
            {
                Debug.Log("Got Ambushed!");
                getEncounter = true;
            }
        }
    }

    void StartBattle()
    {

    }
}
