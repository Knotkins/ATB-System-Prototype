using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour {


	public enum PerformAction {
		WAIT,
		TAKEACTION,
		PERFORMACTION,
        CHECK,
        WIN,
        LOSE  
	}
	public PerformAction battleStates;

	public List<HandleTurns> PerformList = new List<HandleTurns> ();
	public List<GameObject> HerosInBattle = new List<GameObject> ();
	public List<GameObject> EnemiesInBattle = new List<GameObject> ();

	public enum HeroGUI {
		ACTIVATE,
		WAITING,
		INPUT1,
		INPUT2,
		DONE
	}

	public HeroGUI HeroInput;

	public List<GameObject> HerosToManage = new List<GameObject> ();
	private HandleTurns HeroChoice;
	public GameObject enemyButton;
	public Transform Spacer;

	public GameObject AttackPanel;
	public GameObject EnemySelectPanel;
    public GameObject MagicsPanel;

    //magic attacks
    public Transform actionSpacer;
    public Transform magicsSpacer;
    public GameObject actionButton;
    private List<GameObject> atkBtns = new List<GameObject>();
    public GameObject magicButton;

    //enemy buttons
    private List<GameObject> enemyBtns = new List<GameObject>();


	// Use this for initialization
	void Start () {
		battleStates = PerformAction.WAIT;
		EnemiesInBattle.AddRange (GameObject.FindGameObjectsWithTag ("Enemy"));
		HerosInBattle.AddRange (GameObject.FindGameObjectsWithTag ("Hero"));
		HeroInput = HeroGUI.ACTIVATE;

		AttackPanel.SetActive (false);
		EnemySelectPanel.SetActive (false);
        MagicsPanel.SetActive(false);

		EnemyButtons();
	}

	// Update is called once per frame
	void Update () {
		switch(battleStates) {
			case(PerformAction.WAIT):
				if (PerformList.Count > 0){
					battleStates = PerformAction.TAKEACTION;
				}
			break;
			case(PerformAction.TAKEACTION):
				GameObject performer = GameObject.Find (PerformList[0].Attacker);
				if (PerformList[0].Type == "Enemy") {
					EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine> ();
                    for(int i = 0; i<HerosInBattle.Count; i++)
                    {
                        if(PerformList[0].AttakersTarget == HerosInBattle[i])
                        {
                            ESM.HeroToAttack = PerformList[0].AttakersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                            break;
                        }
                        else
                        {
                            PerformList[0].AttakersTarget = HerosInBattle[Random.Range(0, HerosInBattle.Count)];
                            ESM.HeroToAttack = PerformList[0].AttakersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                        }
                    }
				}
				if (PerformList[0].Type == "Hero") {
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                    HSM.EnemyToAttack = PerformList[0].AttakersTarget;
                    HSM.currentState = HeroStateMachine.TurnState.ACTION;
				}
				battleStates = PerformAction.PERFORMACTION;
			break;
			case(PerformAction.PERFORMACTION):
                //idle
			break;
            case (PerformAction.CHECK):
                if(HerosInBattle.Count < 1)
                {
                    battleStates = PerformAction.LOSE;
                }
                else if(EnemiesInBattle.Count < 1)
                {
                    battleStates = PerformAction.WIN;
                }
                else
                {
                    //call function
                    clearAttackPanel();
                    HeroInput = HeroGUI.ACTIVATE;
                }
            break;

            case (PerformAction.LOSE):
                {
                    Debug.Log("You lose the battle!");
                }
            break;
            case (PerformAction.WIN):
                {
                    Debug.Log("You win the battle!");
                    for(int i = 0; i< HerosInBattle.Count; i++)
                    {
                        HerosInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING;
                    }
                }
            break;
        }

		switch (HeroInput) {
			case (HeroGUI.ACTIVATE):
				if(HerosToManage.Count > 0) {
					HerosToManage[0].transform.Find("Selector").gameObject.SetActive(true);
					HeroChoice = new HandleTurns ();
					AttackPanel.SetActive (true);
                    CreateAttackButtons();
                    HeroInput = HeroGUI.WAITING;
				}
			break;
			case (HeroGUI.WAITING):
				//idle
			break;
			case (HeroGUI.DONE):
				HeroInputDone ();
			break;

		}

	}
	public void CollectActions(HandleTurns input) {
		PerformList.Add(input);
	}

	public void EnemyButtons () {
        //cleanup
        foreach(GameObject enemyBtn in enemyBtns)
        {
            Destroy(enemyBtn);
        }
        enemyBtns.Clear();
        //create buttons
		foreach(GameObject enemy in EnemiesInBattle) {
			GameObject newButton = Instantiate (enemyButton) as GameObject;
			EnemySelectButton button = newButton.GetComponent<EnemySelectButton> ();

			EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();

			Text buttonText = newButton.GetComponentInChildren<Text>();
			buttonText.text = cur_enemy.enemy.theName;

			button.EnemyPrefab = enemy;

			newButton.transform.SetParent (Spacer,false);
            enemyBtns.Add(newButton);
		}
	}

	public void Input1() { //attack button
		HeroChoice.Attacker = HerosToManage [0].name;
		HeroChoice.AttacksGameObject = HerosToManage [0];
		HeroChoice.Type = "Hero";
        HeroChoice.chosenAttack = HerosToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0];

		AttackPanel.SetActive (false);
		EnemySelectPanel.SetActive (true);
	}

	public void Input2(GameObject chosenEnemy) { //enemy selection
		HeroChoice.AttakersTarget = chosenEnemy;
		HeroInput = HeroGUI.DONE;
	}

	void HeroInputDone () {
		PerformList.Add(HeroChoice);
        //clean attack panel
        clearAttackPanel();

		HerosToManage[0].transform.Find("Selector").gameObject.SetActive(false);
		HerosToManage.RemoveAt (0);
		HeroInput = HeroGUI.ACTIVATE;
	}

    void clearAttackPanel()
    {
        EnemySelectPanel.SetActive(false);
        AttackPanel.SetActive(false);
        MagicsPanel.SetActive(false);
        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }

    //craete actiobuttons
    void CreateAttackButtons()
    {
        GameObject AttackButton = Instantiate(actionButton) as GameObject;
        Text AttackButtonText = AttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        AttackButtonText.text = "Attack";
        AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());
        AttackButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(AttackButton);

        GameObject MagicAttackButton = Instantiate(actionButton) as GameObject;
        Text MagicsMenuButtonText = MagicAttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        MagicsMenuButtonText.text = "Magics";
        MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => Input3());
        MagicAttackButton.transform.SetParent(actionSpacer, false);
        atkBtns.Add(MagicAttackButton);

        if(HerosToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks.Count > 0)
        {
            foreach (BaseAttack magicAtk in HerosToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks)
            {
                GameObject MagicButton = Instantiate(magicButton) as GameObject;
                Text MagicButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent<Text>();
                MagicButtonText.text = magicAtk.attackName;
                AttackButton ATB = MagicButton.GetComponent<AttackButton>();
                ATB.magicAttackToPerform = magicAtk;
                MagicButton.transform.SetParent(magicsSpacer, false);
                atkBtns.Add(MagicButton);

            }
        }
        else
        {
            MagicAttackButton.GetComponent<Button>().interactable = false;
        }
    }

    public void Input4(BaseAttack chosenMagic) //chosen magic attack
    {
        HeroChoice.Attacker = HerosToManage[0].name;
        HeroChoice.AttacksGameObject = HerosToManage[0];
        HeroChoice.Type = "Hero";
        HerosToManage[0].GetComponent<HeroStateMachine>().hero.curMP -= chosenMagic.attackCost;

        HeroChoice.chosenAttack = chosenMagic;
        MagicsPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }

    public void Input3() //switching to magic attack 
    {
        AttackPanel.SetActive(false);
        MagicsPanel.SetActive(true);
    }
}
