using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryScreenManager : MonoBehaviour {

    Inventory inv;
    public GameObject itemPanel;
    public GameObject itemButton;
    public Transform Spacer;
    public List<GameObject> itmBtns = new List<GameObject>();

   
	// Use this for initialization
	void Start () {
        UpdateItems();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateItems()
    {
        //grab all items from gamemanger
        inv = GameManager.instance.GetComponent<Inventory>();
    }

    public void CreateButtons()
    {
        //render the buttons
    for(int i = 0; i < inv.Inv.Count; i++)
        {
            GameObject invButton = Instantiate(itemButton) as GameObject;
            Text ButtonText = invButton.transform.Find("Text").gameObject.GetComponent<Text>();
            ButtonText.text = inv.Inv[i].itemName;
          // Button.GetComponent<Button>().onClick.AddListener(() => Input1());
            invButton.transform.SetParent(Spacer, false);
            itmBtns.Add(invButton);
        }
       
    }

    public void ClearButtons()
    {
        //delete all the buttons
        foreach (GameObject itmBtn in itmBtns)
        {
            Destroy(itmBtn);
        }
        itmBtns.Clear();
    }
}
