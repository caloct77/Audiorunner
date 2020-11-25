using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class victory : MonoBehaviour {

	public Text scoreTxt;
	public AudioSource AS;
	public AudioClip AC;
	float elapsedTime=0f;
	public Image vicOvl;
	// Use this for initialization
	void Start () {
		//PlayerPrefs.SetInt ("scoreWin", 1000);
		AS.clip = AC;
		AS.Play ();
		if (PlayerPrefs.HasKey ("scoreWin")) {
			scoreTxt.text = "Score : " + PlayerPrefs.GetInt ("scoreWin");
			PlayerPrefs.DeleteKey ("scoreWin");
		}
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
		if (elapsedTime >= 2.5f) {
			vicOvl.CrossFadeAlpha (0.0f, 0.2f, true);
		}
		elapsedTime += Time.deltaTime;

	}
}
