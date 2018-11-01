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
public Image ProgressBar;
public GameObject Selector;

	// Use this for initialization
	void Start () {
		Selector.SetActive(false);
		BSM = GameObject.Find ("BattleManager").GetComponent<BattleStateMachine> ();
		currentState = TurnState.PROCESSING;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (currentState);
		switch(currentState) {
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

			break;
			case(TurnState.DEAD):

			break;
		}
	}
	void UpgradeProgBar() {
		cur_cooldown = cur_cooldown + Time.deltaTime;
		float clac_cooldown = cur_cooldown / max_cooldown;
		ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(clac_cooldown,0,1),ProgressBar.transform.localScale.y,ProgressBar.transform.localScale.z);
		if(cur_cooldown >= max_cooldown) {
			currentState = TurnState.ADDTOLIST;
		}
	}
}
