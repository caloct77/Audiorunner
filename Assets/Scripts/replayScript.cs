using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class replayScript : MonoBehaviour {

	public int []replaychrom;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}

	public void setReplay(int [] realchrom)
	{
		replaychrom = new int[realchrom.Length];

		for (int a = 0; a < realchrom.Length; a++) {
			replaychrom [a] = realchrom [a];
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
