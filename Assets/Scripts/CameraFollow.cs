using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = gameObject.transform.position;
      pos.x = FindObjectOfType<HeroMovement>().transform.position.x;
      pos.y = FindObjectOfType<HeroMovement>().transform.position.y + 5.9f;
      pos.z = FindObjectOfType<HeroMovement>().transform.position.z + -15f;
       transform.position = pos;
    }
}
