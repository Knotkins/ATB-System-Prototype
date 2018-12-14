using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour {

	private BattleStateMachine BSM;
	public BaseHero hero;

public enum TurnState {
	PROCESSING,
	ADDTOLIST,
	WAITING,
	SELECTING,
	ACTION,
	DEAD
}

public TurnState currentState;
//For progress Bar
private float cur_cooldown = 0.0f;
private float max_cooldown = 5.0f;
private Image ProgressBar;
private Image HealthBar;
private Image ManaBar;
public GameObject Selector;
    //IeNumerator
    public GameObject EnemyToAttack;
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animSpeed = 10.0f;
    //dead
    private bool alive = true;
    //heroPanel
    private HeroPanelStats stats;
    public GameObject HeroPanel;
    private Transform HeroPanelSpacer;


    // Use this for initialization
    void Start () {
        //find spacer obj
        HeroPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer");
        //create panel, fill w/ info
        CreateHeroPanel();

        startPosition = transform.position;
        cur_cooldown = Random.Range(0,2.5f); //Use LUCK, or SPEED?
		Selector.SetActive(false);
		BSM = GameObject.Find ("BattleManager").GetComponent<BattleStateMachine> ();
		currentState = TurnState.PROCESSING;
	}

	// Update is called once per frame
	void Update () {
        //Debug.Log (currentState);

        //Update stat bars
        HealthBar.transform.localScale = new Vector3(Mathf.Clamp((hero.curHP / hero.baseHP), 0, 1), HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        ManaBar.transform.localScale = new Vector3(Mathf.Clamp((hero.curMP / hero.baseMP), 0, 1), ManaBar.transform.localScale.y, ManaBar.transform.localScale.z);

        //states
        switch (currentState) {
			case(TurnState.PROCESSING):
				UpgradeProgBar ();
			break;
			case(TurnState.ADDTOLIST):
				BSM.HerosToManage.Add(this.gameObject);
				currentState = TurnState.WAITING;
			break;
			case(TurnState.WAITING):
				//idle
			break;
			case(TurnState.SELECTING):

			break;
			case(TurnState.ACTION):
                StartCoroutine(TimeForAction());
			break;
			case(TurnState.DEAD):
                if (!alive)
                {
                    return;
                }
                else
                {
                    //change tag of hero
                    this.gameObject.tag = "DeadHero";
                    //not attackable
                    BSM.HerosInBattle.Remove(this.gameObject);
                    //not mnagable
                    BSM.HerosToManage.Remove(this.gameObject);
                    //deactivate selector
                    Selector.SetActive(false);
                    //resest GUI
                    BSM.AttackPanel.SetActive(false);
                    BSM.EnemySelectPanel.SetActive(false);
                    //remove item from performlist
                    if (BSM.HerosInBattle.Count > 0)
                    {
                        for (int i = 0; i < BSM.PerformList.Count; i++)
                        {
                            if (BSM.PerformList[i].AttacksGameObject == this.gameObject)
                            {
                                BSM.PerformList.Remove(BSM.PerformList[i]);
                            }
                            if (BSM.PerformList[i].AttakersTarget == this.gameObject)
                            {
                                BSM.PerformList[i].AttakersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
                            }
                        }
                        //chagne colour / play animation
                        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
                        //reset heroinput
                        BSM.battleStates = BattleStateMachine.PerformAction.CHECK;
                        alive = false;
                    }
                }
			break;
		}
    }
	void UpgradeProgBar() {
		cur_cooldown = cur_cooldown + Time.deltaTime;
		float clac_cooldown = cur_cooldown / max_cooldown;
		ProgressBar.transform.localScale = new Vector3(ProgressBar.transform.localScale.x, Mathf.Clamp(clac_cooldown, 0, 1),ProgressBar.transform.localScale.z);
		if(cur_cooldown >= max_cooldown) {
			currentState = TurnState.ADDTOLIST;
		}
	}

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        //animate the enemy near the hero to attacks
        Vector3 enemyPosition = new Vector3(EnemyToAttack.transform.position.x + 1.5f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
        while (MoveTowardsEnemy(enemyPosition)) { yield return null; }

        //WAIT
        yield return new WaitForSeconds(0.5f);
        //do damage
        DoDamage();
        //animate back to startPosition
        Vector3 firstPosition = startPosition;
        while (MoveTowardsStart(firstPosition)) { yield return null; }

        //remove this performer from list in BSM
        BSM.PerformList.RemoveAt(0);
        //reset BSM -> WAIT
        if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
        {

            BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
            //end coroutine
            actionStarted = false;
            //reset this enemy TurnState
            cur_cooldown = 0.0f;
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }
    }
    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    public void takeDamage (float getDamageAmount)
    {
        hero.curHP -= getDamageAmount;
        if (hero.curHP <= 0)
        {
            hero.curHP = 0;
            currentState = TurnState.DEAD;
        }
        UpdateHeroPanel();
    }

    void DoDamage()
    {
        float calc_damage = hero.curATK + BSM.PerformList[0].chosenAttack.attackDamage;
        EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage);
        Debug.Log(hero.theName + " attacks " + EnemyToAttack.name + " with " + BSM.PerformList[0].chosenAttack.attackName + " and deals " + calc_damage + " damage!");
    }

    void CreateHeroPanel()
    {
        HeroPanel = Instantiate(HeroPanel) as GameObject;
        stats = HeroPanel.GetComponent<HeroPanelStats>();

        stats.HeroName.text = hero.theName;
        stats.HeroHP.text = "HP: " + hero.curHP;
        stats.HeroMP.text = "MP: " + hero.curMP;
        ProgressBar = stats.ProgressBar;
        HealthBar = stats.HealthBar;
        ManaBar = stats.ManaBar;
        HeroPanel.transform.SetParent(HeroPanelSpacer, false);
    }
    void UpdateHeroPanel()
    {
        stats.HeroHP.text = "HP: " + hero.curHP;
        stats.HeroMP.text = "MP: " + hero.curMP;
    }
}
