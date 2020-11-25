using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelIntro : MonoBehaviour {

	public GameObject cam,plyr;
	float elapsed;
	public AudioSource AS;
	bool started=false;
	// Use this for initialization
	void Start () {
		//cam.SetActive (false);
		cam.GetComponent<camInGame> ().enabled = false;
		plyr.SetActive (false);
		elapsed = 0f;
		//cam.transform.position =new Vector3 (plyr.transform.position);
	}

	// Update is called once per frame
	void Update () {
		//4.98 1.953 -7.18 ---- posisi awal kamera

		if (elapsed < 1f && started == false) {
			
		}


		if (elapsed > 3f && started==false) {
			//cam.SetActive (true);
			cam.GetComponent<camInGame> ().enabled = true;
			plyr.SetActive (true);
			AS.Play ();
			started = true;
		}
		elapsed += Time.deltaTime;
	}
}
