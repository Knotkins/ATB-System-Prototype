using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour {

    float moveSpeed = 10.0f;
    Vector3 curPos, lastPos;


	// Use this for initialization
	void Start () {
        transform.position = GameManager.instance.nextHeroPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0.0f, moveZ);
        GetComponent<Rigidbody>().velocity = movement * moveSpeed;// * Time.deltaTime;

        curPos = transform.position;
        if(curPos == lastPos)
        {
            GameManager.instance.isWalking = false;
        }
        else
        {
            GameManager.instance.isWalking = true;
        }
        lastPos = curPos;
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Portal")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            if (col.spawnPoint)
            {
                GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
            }
           
            GameManager.instance.sceneToLoad = col.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }
        if(other.tag == "Region1")
        {
            GameManager.instance.curRagion = 0;
        }
        if (other.tag == "Region2")
        {
            GameManager.instance.curRagion = 1;

        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Region1" || other.tag == "Region2")
        {
            GameManager.instance.canEncounter = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Region1" || other.tag == "Region2")
        {
            GameManager.instance.canEncounter = false;
        }
    }
}
