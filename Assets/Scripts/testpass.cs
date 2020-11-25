using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class testpass : MonoBehaviour {

	GameObject obj;
	public AudioSource AS;
	AudioClip selectedSong;
	// Use this for initialization
	IEnumerator setmusic(string inp)
	{
		print ('d');
		WWW www = new WWW ("file://" + inp); //stream file

		yield return www;
		selectedSong=www.GetAudioClip (); //set clip

		print ("adsdsa");

		print(selectedSong.samples);

		print (selectedSong.channels);
		AS.clip = selectedSong;

		AS.Play ();
	//	SceneManager.LoadScene("Loading", LoadSceneMode.Single);


	}

	void Start () {
		//obj = GameObject.Find ("persistentObject");
		//AudioClip td = obj.GetComponent<difficultyNav> ().selectedSong;
		//AS.clip = td;
		//AS.Play ();
		string cpy=PlayerPrefs.GetString("songPath"); //ambil path lagu dari difficultyNav
		print (cpy);
		StartCoroutine (setmusic (cpy)); //fungsi setmusic
		PlayerPrefs.DeleteKey ("songPath"); //clean
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
