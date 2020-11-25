using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SFB;

public class difficultyNav : MonoBehaviour {
	//public GUIText wtxt;
	public Text warnTxt;
	public AudioClip selectedSong;
	public AudioSource d;
	string a;


	public void OnClicked(Button button)
	{
		print (button.name);
		//print ("btnEasy");


		if (button.name != ("btnBack")) { //easy/normal/hard
			
			print ("back to main menu");
			var extensions = new [] {
				new ExtensionFilter ("Sound Files", "ogg")
			};
			var paths = StandaloneFileBrowser.OpenFilePanel ("Open File", "", extensions, true);

			print (paths.GetValue (0));
			a = paths.GetValue (0).ToString ();

			
			PlayerPrefs.SetString("songPath",a); //simpen path lagu buat testpass

			if (button.name == "btnEasy")
				PlayerPrefs.SetInt ("obsdiff", 3);
			else if (button.name == "btnNormal")
				PlayerPrefs.SetInt ("obsdiff", 5);
			else if (button.name == "btnHard")
				PlayerPrefs.SetInt ("obsdiff", 7); // buat dipake GA
			if (a.Length > 0) {
				DestroyObject (GameObject.Find ("AudioSourceCarrier"));
				PlayerPrefs.DeleteKey ("MMplaying");
			}
			SceneManager.LoadScene("Loading", LoadSceneMode.Single);
		} else {
			SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
		}
	}


	// Use this for initialization
	void Start () {
		Destroy(GameObject.FindGameObjectWithTag("GAcarrier"));
		if(PlayerPrefs.HasKey("songWarn"))
			warnTxt.text=PlayerPrefs.GetString("songWarn");
		PlayerPrefs.DeleteKey ("songWarn");
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
