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
        public string BattleScene;
        public List<GameObject> possibleEnemies = new List<GameObject>();
    }

    public int curRagion;

    public List<RegionData> Regions = new List<RegionData>();

    public GameObject heroCharacter;

    public List<PartyMember> party = new List<PartyMember>();

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
        DUNGEON_STATE,
        IDLE
    }

    public int enemyAmt;
    public int heroAmt;
    public List<GameObject> enemiesToBattle = new List<GameObject>();
    public List<GameObject> heroesToBattle = new List<GameObject>();

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
        if (gameState == GameStates.WORLD_STATE || gameState == GameStates.TOWN_STATE)
        {
            if (!GameObject.Find("HeroCharacter"))
            {
                GameObject Hero = Instantiate(heroCharacter, nextHeroPosition, Quaternion.identity) as GameObject;
                Hero.name = "HeroCharacter";
            }
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
                StartBattle();
                gameState = GameStates.IDLE;
                //go to idle
                break;
            case (GameStates.DUNGEON_STATE):
                //Dungeon encounter RNG stuff here
                break;
            case (GameStates.IDLE):

                break;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene);
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
        enemyAmt = Random.Range(1, Regions[curRagion].maxEnemyAmt + 1);

        for(int i = 0; i< enemyAmt; i++)
        {
            enemiesToBattle.Add(Regions[curRagion].possibleEnemies[Random.Range(0, Regions[curRagion].possibleEnemies.Count)]);
        }
        lastHeroPosition = GameObject.Find("HeroCharacter").gameObject.transform.position;
        nextHeroPosition = lastHeroPosition;
        lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(Regions[curRagion].BattleScene);

        isWalking = false;
        canEncounter = false;
        getEncounter = false;
    }
}
