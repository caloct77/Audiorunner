using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animTest : MonoBehaviour {
	Animator ani;
	// Use this for initialization
	void Start () {
		ani = GetComponent<Animator> ();
		ani.Play ("Hurricane Kick");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
