using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camInGame : MonoBehaviour {
	public bool moving=true;

	// Use this for initialization
	void Start () {
		

	}

	// Update is called once per frame
	void Update () {
		if(moving==true)
			transform.position = Vector3.MoveTowards(transform.position,new Vector3(transform.position.x+2,transform.position.y,transform.position.z),4*Time.deltaTime);
	

	}
}
