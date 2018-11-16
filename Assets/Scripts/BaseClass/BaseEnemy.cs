using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy: BaseClass {
	public enum Type {
		FIRE,
		WATER,
		WIND,
		EARTH
	}
	public enum Rarity {
		COMMON,
		UNCOMMON,
		RARE,
		MYTHICAL
	}
	public Type enemyType;
	public Rarity rarity;
}
