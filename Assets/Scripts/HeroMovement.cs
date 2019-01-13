using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour {

    float moveSpeed = 10.0f;

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
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Portal")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = col.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }
    }
}
