using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstTest : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		GameObject foo = GameObject.Instantiate((GameObject)Resources.Load("instTest"));
		GameObject clone = Instantiate (foo, Vector3.zero, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
