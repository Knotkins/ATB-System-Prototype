using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour {

    public string itemName;
    public int buyValue; //Amt this item can be purchased for, sell value is derived from this
    public int amtHeld; //the amt of this item that is currently held
}
