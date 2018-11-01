using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {

	private BattleStateMachine BSM;
	public BaseEnemy enemy;

	public enum TurnState {
		PROCESSING,
		CHOOSEACTION,
		WAITING,
		ACTION,
		DEAD
	}

	public TurnState currentState;
	//For progress Bar
	private float cur_cooldown = 0.0f;
	private float max_cooldown = 5.0f;
	//this GameObject
	private Vector3 startPosition;
	//timeforaction stuff
	private bool actionStarted = false;
	public GameObject HeroToAttack;
	private float animSpeed = 5.0f;

	// Use this for initialization
	void Start () {
		currentState = TurnState.PROCESSING;
		BSM = GameObject.Find ("BattleManager").GetComponent<BattleStateMachine> ();
		startPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (currentState);
		switch(currentState) {
			case(TurnState.PROCESSING):
				UpgradeProgBar ();
				break;
			case(TurnState.CHOOSEACTION):
				ChooseAction ();
				currentState = TurnState.WAITING;
				break;
			case(TurnState.WAITING):
				//idle
				break;
			case(TurnState.ACTION):
				StartCoroutine(TimeForAction());
				break;
			case(TurnState.DEAD):

			break;
	}
}
	void UpgradeProgBar() {
		cur_cooldown = cur_cooldown + Time.deltaTime;
		if(cur_cooldown >= max_cooldown) {
			currentState = TurnState.CHOOSEACTION;
		}
	}
	void ChooseAction() {
		HandleTurns myAttack = new HandleTurns ();
		myAttack.Attacker = enemy.name;
		myAttack.Type = "Enemy";
		myAttack.AttacksGameObject = this.gameObject;
		myAttack.AttakersTarget = BSM.HerosInBattle[Random.Range(0,BSM.HerosInBattle.Count)];
		BSM.CollectActions (myAttack);
	}
	private IEnumerator TimeForAction () {
		if (actionStarted) {
			yield break;
		}
		actionStarted = true;

		//animate the enemy near the hero to attacks
		Vector3 heroPosition = new Vector3 (HeroToAttack.transform.position.x -1.5f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z);
		while (MoveTowardsEnemy (heroPosition)) {yield return null;}

		//WAIT
		yield return new WaitForSeconds(0.5f);
		//do damage

		//animate back to startPosition
		Vector3 firstPosition = startPosition;
		while (MoveTowardsStart (firstPosition)) {yield return null;}

		//remove this performer from list in BSM
		BSM.PerformList.RemoveAt(0);
		//reset BSM -> WAIT
		BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
		//end coroutine
		actionStarted = false;
		//reset this enemy TurnState
		cur_cooldown = 0.0f;
		currentState = TurnState.PROCESSING;
	}
	private bool MoveTowardsEnemy(Vector3 target){
		return target != (transform.position = Vector3.MoveTowards(transform.position,target,animSpeed * Time.deltaTime));
	}
	private bool MoveTowardsStart(Vector3 target){
		return target != (transform.position = Vector3.MoveTowards(transform.position,target,animSpeed * Time.deltaTime));
	}
}
