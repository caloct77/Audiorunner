using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class gameFlow : MonoBehaviour {
	//UI
	public Button pauseBtn;
	public Button resumeBtn;
	public Button backMenuBtn;
	public CanvasGroup mainCnvs;
	public CanvasGroup pauseMenuCnvs;
	public Text scoreTxt;
	//audio
	public AudioSource AS;
	GameObject clipCarrier;
	GA ga;
	//char+cam
	public GameObject character; // *2+8
	public GameObject camera;
	Transform charTrfm;

	//flow
	float resumeTime=0.0f;
	bool resumeClicked=false;
	public float score=0;
	float runtime = -3f;
	int scoreupdater=0;
	int scoremult = 1;
	float fadetime=0f;
	public Image blackOverlay;
	bool winning=false;
	public GameObject explosion;
	bool defeated=false;
	float defeatTime=0f;
	bool defeatTrans=false;
	bool stopscore=false;

	//song
	public AudioClip fade;
	// Use this for initialization
	void Start () {
		blackOverlay.CrossFadeAlpha (0f, 0.1f, true);
		updatescore ();
		clipCarrier = GameObject.Find ("GAcarrier");
		ga = clipCarrier.GetComponent<GA> ();
		AS.clip = ga.tempAC;
		charTrfm = character.transform;
		pauseBtn.onClick.AddListener (pauseClick);
		resumeBtn.onClick.AddListener (resumeClick);
		backMenuBtn.onClick.AddListener (backMenuClick);
		print ("gameflow");
		pauseMenuCnvs.alpha = 0.0f;
		pauseMenuCnvs.blocksRaycasts = false;
	}

	public void backMenuClick()
	{
		print ("main menu");
		Time.timeScale = 1.0f;
		SceneManager.LoadScene ("Defeat", LoadSceneMode.Single);
	}

	public void pauseClick()
	{
		print ("paused");
		Time.timeScale = 0.0f;
		AS.Pause ();
		pauseMenuCnvs.alpha = 1.0f;
		pauseMenuCnvs.blocksRaycasts = true;
		mainCnvs.alpha = 0.0f;
		mainCnvs.blocksRaycasts = false;
	}

	public void resumeClick()
	{
		resumeTime = Time.realtimeSinceStartup;
		//resumeClicked = true;
		pauseMenuCnvs.alpha = 0.0f;
		pauseMenuCnvs.blocksRaycasts = false;
		mainCnvs.alpha = 1.0f;
		mainCnvs.blocksRaycasts = true;
		print ("resumed");
		Time.timeScale = 1.0f;
		AS.UnPause ();

	}



	void updatescore()
	{
		
		if (scoreupdater % 10 == 0 && runtime>=0f) {
			scoreTxt.text =  "Score : " + (Mathf.Round(score));
			scoreupdater = 0;
		}
	}

	void calcScore()
	{
		if (charTrfm.position.x > camera.transform.position.x - 7.9f) {
			scoremult++;
		}
		if (charTrfm.position.x > camera.transform.position.x - 3.01f) {
			scoremult++;
		}
		if (charTrfm.position.x > camera.transform.position.x + 1.88f) {
			scoremult++;
		}
		if (charTrfm.position.x > camera.transform.position.x + 6.77f) {
			scoremult++;
		}
		if(runtime>=0f)
			score += ga.difficultyObstacle * (((runtime + Time.deltaTime) - runtime) / ga.songDur) * scoremult*100;
		//score+=
		scoremult=1;
		updatescore();
		scoreupdater++;
	}

	public void victory()
	{
		
		PlayerPrefs.SetInt ("scoreWin", Mathf.RoundToInt(score));
		AS.Stop ();
		AS.clip = fade;
		AS.Play ();
			fadetime = Time.realtimeSinceStartup;
			blackOverlay.CrossFadeAlpha (1f, 2f, true);
			print ("victory");
			Time.timeScale = 0.01f;
	}

	public void defeat()
	{
		
		character.GetComponent<Animator> ().Play ("Idle");
		character.GetComponent<Character> ().enabled = false;
		character.GetComponent<CharacterController> ().enabled = false;
		character.GetComponent<CapsuleCollider> ().enabled = false;
		camera.GetComponent<camInGame> ().moving = false;

		character.GetComponent<AudioSource> ().Play ();
		defeated = true;
		stopscore = true;
	}

	// Update is called once per frame
	void Update () {
		/* PAUSING WAIT
		if (resumeClicked == true && resumeTime < Time.realtimeSinceStartup - 4.0f) {
			print ("resumed");
			Time.timeScale = 1.0f;
			AS.UnPause ();
			mainCnvs.alpha = 1.0f;
			mainCnvs.blocksRaycasts = true;


			resumeClicked = false;
		}*/

		//if OOB, kalo telat=kalah, lebih=dikembaliin
		if (charTrfm.position.x > camera.transform.position.x + 12.3f) {
			//			charTrfm.position.x = transform.position.x + 12.79f;
			//print("OOB");
			charTrfm.position = new Vector3 (camera.transform.position.x + 12.2f, charTrfm.position.y, charTrfm.position.z);

		}

		if (character.transform.position.x < camera.transform.position.x - 12.3f) {
			//lose game
		//	print("lose");
		//	defeat ();
		}

		//TODO : DIKASIH MILESTONE 1-5
		//scoring, hitungan di camera bound thdp UI score
		/*if (charTrfm.position.x < camera.transform.position.x - 7.08f) {
			score += ga.difficultyObstacle * (((runtime+Time.deltaTime)-runtime) / ga.songDur);
		} else if (charTrfm.position.x >= camera.transform.position.x + 10.63f) {
			score += ga.difficultyObstacle * ((((runtime+Time.deltaTime)-runtime) / ga.songDur) * 2);
		} else { //17.71
			//charpos - 7.08 - cam x, /17.71
			score += ga.difficultyObstacle * (((runtime+Time.deltaTime)-runtime) / ga.songDur) *
				(charTrfm.position.x - ((camera.transform.position.x - 7.08f) / 17.71f));
		}
		*/


		//scoring pake milestone
		if (winning==false && camera.transform.position.x > ga.songDur*4+10) {
			winning = true;
			victory ();
		}

		if (winning == true) {
			if (Time.realtimeSinceStartup - 2f > fadetime) {
				print ("change");
				Time.timeScale = 1f;
				SceneManager.LoadScene("Victory", LoadSceneMode.Single);
			}
		}

		if (defeated == true ) {
			
			if (defeatTime < 0.25f) {//kamera mundur
				camera.transform.position = new Vector3 (charTrfm.position.x,charTrfm.position.y,charTrfm.position.z-15);
			}else if (defeatTime < 0.75f) {//char meledak+naik
				charTrfm.position = new Vector3 (charTrfm.position.x, charTrfm.position.y + 0.009f, charTrfm.position.z);

			} else if (defeatTime < 1.75f) {//char turun
				charTrfm.position = new Vector3 (charTrfm.position.x, charTrfm.position.y - 0.017f, charTrfm.position.z);
			} else {//fade out
				AS.Stop ();
				AS.clip = fade;
				AS.Play ();
				blackOverlay.CrossFadeAlpha (1f, 1.5f, true);
				defeated = false;
				defeatTrans = true;
			}
			if(defeated==true)
			defeatTime += Time.deltaTime;
		}
		if (defeatTrans == true) {
			if (defeatTime >= 3.8f) {
				defeatTrans = false;
				SceneManager.LoadScene ("Defeat", LoadSceneMode.Single);
			}
			defeatTime += Time.deltaTime;
		}
		if(stopscore==false)
		calcScore();
		runtime+=Time.deltaTime;

	}
}
