using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class varFromAnotherScriptTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//GameObject.Find("GameObjectZ").GetComponent<secEarlier>().tester -= 10.0f;;
		Debug.Log (GameObject.Find("GameObjectZ").GetComponent<secEarlier>().tester);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
