using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurns {

	public string Attacker; //Name of Attacker
	public string Type;
	public GameObject AttacksGameObject; //who attacks
	public GameObject AttakersTarget; //who is being attacked

	//which attack is performed
}
