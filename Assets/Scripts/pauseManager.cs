using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseManager : MonoBehaviour
{


    public Animator anim;
    public GameObject heroStatusPanel;


    bool isOpen;
    // Use this for initialization
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            PauseGame();
    }

    public void PauseGame()
    {
        if (isOpen == false)
        {
            CreateHeroStatus();
            anim.SetBool("isOpen", true);
            isOpen = true;
        }
        else
        {
            anim.SetBool("isOpen", false);
            isOpen = false;
            
        }
    }

    void CreateHeroStatus()
    {
        for (int i = 0; i <= GameManager.instance.party.Count - 1; i++)
        {
            heroStatusPanel.transform.GetChild(i).gameObject.SetActive(true);
            heroStatusPanel.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = "HP: " + GameManager.instance.party[i].curHP + "/" + GameManager.instance.party[i].maxHP;
            heroStatusPanel.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "MP: " + GameManager.instance.party[i].curMP + "/" + GameManager.instance.party[i].maxMP;
            heroStatusPanel.transform.GetChild(i).transform.GetChild(3).GetComponent<Text>().text = "LVL: " + GameManager.instance.party[i].level;
            heroStatusPanel.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>().text = "EXP: " + GameManager.instance.party[i].EXP;
            heroStatusPanel.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>().text = "To Next Level: " + GameManager.instance.party[i].toNextLevel;

            heroStatusPanel.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.party[i].portrait;
        }


    }
}
