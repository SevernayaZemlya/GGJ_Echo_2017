using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipLighting : MonoBehaviour {

	private GameObject target;

	// if the current object's Z is less than the target's Z, rotate it on the y axis by 150 

	void Start () {
		target = GameObject.FindWithTag("Player");
	}

	void Update () {
		if (transform.position.z < target.transform.position.z) {
			Debug.Log(target.transform.position.z);
			// Debug.Log(transform.position.z);
			//transform.eulerAngles = new Vector3(0, 180, 0);
		} else {
			//transform.eulerAngles = new Vector3(0, 0, 0);
		}
	}
}
