using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class defeat : MonoBehaviour {

	public Text scoreTxt;
	public AudioSource AS;
	public AudioClip AC;
	float elapsedTime=0f;
	public Image vicOvl;
	// Use this for initialization
	void Start () {
		
		AS.clip = AC;
		AS.Play ();

	}

	public void MenuClick()
	{
		SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
	}

	public void replayClick()
	{
		print ("D");
		PlayerPrefs.SetInt ("replay", 1);


		SceneManager.LoadScene("inGame", LoadSceneMode.Single);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}