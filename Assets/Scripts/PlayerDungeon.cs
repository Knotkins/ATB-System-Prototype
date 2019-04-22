using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDungeon : MonoBehaviour {
    float movementSpeed;
    public bool canMoveUp;
    public bool canMoveDown;
    public Transform frontCheck;
    public Transform backCheck;
    public Vector3 checkVolume;
    public LayerMask checkLayer;
    Collider[] hitColliders;
    // Use this for initialization
    void Start () {
        canMoveUp = true;
        canMoveDown = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (frontCheck)
        {
            //canMoveUp = Physics2D.OverlapCircle(frontCheck.position, checkRadius, checkLayer);
            canMoveUp =! Physics.CheckBox(frontCheck.position, checkVolume, frontCheck.rotation, checkLayer);
        }
        if (frontCheck)
        {
            //canMoveUp = Physics2D.OverlapCircle(frontCheck.position, checkRadius, checkLayer);
            canMoveDown = !Physics.CheckBox(backCheck.position, checkVolume, backCheck.rotation, checkLayer);
        }
        if (Input.GetKeyDown("up") && canMoveUp == true)
        {
            transform.Translate(0,0,1, Space.Self);
            MyCollisions();
        }
        if (Input.GetKeyDown("down") && canMoveDown == true)
        {
            transform.Translate(0, 0, -1, Space.Self);
            MyCollisions();
        }
        if (Input.GetKeyDown("left"))
        {
            Vector3 nextRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - 90, transform.localEulerAngles.z);
            transform.localEulerAngles = nextRotation;
            MyCollisions();
        }
        if (Input.GetKeyDown("right"))
        {
            Vector3 nextRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 90, transform.localEulerAngles.z);
            transform.localEulerAngles = nextRotation;
            MyCollisions();
        }
        int i = 0;
        if (Input.GetKeyDown("z"))
        {
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "DungeonDoor")
                {
                    Destroy(hitColliders[i].gameObject);
                }
                if (hitColliders[i].tag == "Chest")
                {
                    hitColliders[i].GetComponent<Animator>().SetTrigger("Open"); //have chest open and give what it hold to the player
                }
                if (hitColliders[i].tag == "NPC")
                {
                    hitColliders[i].GetComponent<DialogueTrigger>().TriggerDialogue();
                }
                if (hitColliders[i].tag == "Portal")
                {
                    GameManager.instance.sceneToLoad = hitColliders[i].GetComponent<CollisionHandler>().sceneToLoad;
                    GameManager.instance.nextHeroPosition = hitColliders[i].GetComponent<CollisionHandler>().spawnPoint.transform.position;
                    GameManager.instance.LoadNextScene();
                }
                //Increase the number of Colliders in the array
                i++;
            }
        }
    }
    
    void MyCollisions()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        hitColliders = Physics.OverlapBox(frontCheck.position, checkVolume, frontCheck.rotation, checkLayer);

        
    }


}
