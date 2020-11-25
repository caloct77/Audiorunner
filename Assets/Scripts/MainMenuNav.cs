using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SFB;

public class MainMenuNav : MonoBehaviour {


	public AudioSource AS;

	public void OnClicked(Button button)
	{
		print (button.name);

		if (button.name.Equals ("btnPlay")) {
			SceneManager.LoadScene ("Difficulty", LoadSceneMode.Single);


		} else if (button.name.Equals ("btnOption")) {
			SceneManager.LoadScene ("Option", LoadSceneMode.Single);
		} else if (button.name.Equals ("btnExit")) {
			Application.Quit ();
		}
	}

	// Use this for initialization
	void Start () {
		//GameObject.Find ("AudioSourceCarrier").GetComponent<AudioSource> ().isPlaying;
		Destroy(GameObject.FindGameObjectWithTag("GAcarrier"));
		PlayerPrefs.DeleteAll ();
		if (!GameObject.Find ("AudioSourceCarrier").GetComponent<AudioSource> ().isPlaying) {
			AudioListener.volume = 0.5f;
			AS.Play ();
			DontDestroyOnLoad (this.gameObject);
			//PlayerPrefs.SetInt ("MMplaying", 1);
		}
			

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
