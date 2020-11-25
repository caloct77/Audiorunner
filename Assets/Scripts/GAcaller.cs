using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GAcaller : MonoBehaviour {

	public float rand;
	public float randomtotal;
	public float []crossProportion;
	public int p1 = -1;
	public int p2 = -1;
	public float totalfits=0;
	public Text loadtxt;
	GA called; //script yg dipanggil
	replayScript rp; //buat replay
	int iterationTotal=0; //jalan brp kali
	float hudTimer=0; //animasi ui
	int lasthighestfit=0; //fitness tertinggi terakhir berubah
	int samefor=0; //udah ga naik brp kali
	float time=0; //GA jalan brp lama
	float starttime=0; //kapan mulai jalan
	bool pause=false;

	public int nolt = 0;
	public int nnolt= 0;




	bool done=false;
	// Use this for initialization
	private static bool created = false;
	void Awake()
	{
		Application.targetFrameRate = 60;
		if (!created)
		{
			
			created = true;
			Debug.Log("Awake: " + this.gameObject);
		}
	}

	void Start () {
		DontDestroyOnLoad(this.gameObject);
		called = GameObject.Find ("GAcarrier").GetComponent<GA> (); //manggil GA
		 //loading text
		starttime=Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (called.isLoadDone == true) {
			called.Init ();
			//print (called.chromNum);
			for (int a = 0; a < called.chromNum; a++) {
				called.randomPopulation (a);

			}//random seluruh populasi
			called.calcSpectrum ();

			for (int a = 0; a < called.chromNum; a++) { //hitung fitness
				called.calcFitness (a);
			}

			print ("AA" + called.gentotal);
			called.isLoadDone = false;
		}

		/* PURE TESTING

		if (Input.GetKeyDown (KeyCode.Space) && pause == false) {
			pause = true;
			print ("paused");
			nolt = 0;
			for (int a = 0; a < called.gentotal; a++) {
				if (called.bestchrom [a] == 0) {
					nolt++;
				}

			}
		} else if (Input.GetKeyDown (KeyCode.Space) && pause == true) {
			pause = false;
			print ("unpaused");
		}
		if (Input.GetKeyDown (KeyCode.F1) && pause == true) {
			SceneManager.LoadScene("inGame", LoadSceneMode.Single);
		}


*/
		if (Input.GetKeyDown (KeyCode.Space) && done == false) {
			samefor = 4000;
		}

		if (Input.anyKeyDown && done == true) {
			samefor = 0;
			done = false;
			rp = GameObject.Find ("Replayer").GetComponent<replayScript> ();
			rp.setReplay (called.bestchrom);
			//=called getbestchrom
			SceneManager.LoadScene("inGame", LoadSceneMode.Single);

		}
		//if(samefor>=3542){
		//if (pause == false && time <= 600.0f) {  //&& samefor<600

		if (samefor > 3542 ) {
			pause = true;
			done = true;
			loadtxt.text = "Press any key to continue...";
		}
		if (pause == false && time <= 600.0f && starttime < Time.time-2){
			print ("time 1 " + time);
		
		//	loadtxt.text = samefor.ToString ();
			//crossover
			/*
		int max = Mathf.Max (called.genfits);

		int p1fit = -1;
		for (int a = 0; a < called.chromNum; a++) {
			if (called.genfits [a] == max) {
				p1 = a;
				p1fit = max;
				called.genfits [a] = -1;
				a = called.chromNum;
			}
		}
		max = Mathf.Max (called.genfits);


		for (int a = 0; a < called.chromNum; a++) {
			if (called.genfits [a] == max) {
				p2 = a;
				called.genfits [p1] = p1fit;
				a = called.chromNum;
			}
		}
		called.crossover (p1, p2);
		called.crossenter ();
*/

			//crossover v2
			p1 = -1;
			p2 = -1;

			totalfits = 0;
			for (int a = 0; a < 10; a++) {
				totalfits += called.genfits [a];
			}

			crossProportion = new float[10];
			for (int a = 0; a < 10; a++) {
				crossProportion [a] = called.genfits [a];
			}

			rand = Random.Range (0f, totalfits);

			randomtotal = 0;

			//rand = Random.Range (0f, 100f);
			for (int a = 0; a < 10; a++) {
				
				if (rand < crossProportion [a] + randomtotal && rand >= randomtotal) {
					p1 = a;
					a = 10;
				} else
					randomtotal += crossProportion [a];
			}

			randomtotal = 0;

			rand = Random.Range (0f, totalfits);
			for (int a = 0; a < 10; a++) {
				
				if (rand < crossProportion [a] + randomtotal && rand >= randomtotal) {
					if (p1 == a) {
						a = -1;
						rand = Random.Range (0, totalfits);
						randomtotal = 0;
					} else {
						p2 = a;
						a = 10;
					}
				} else
					randomtotal += crossProportion [a];
			}


			called.crossover (p1, p2);
			called.crossenter ();
		
			//mutasi
			for (int a = 0; a < called.chromNum; a++) {
				called.mutation (a);
			}
			called.selection ();

			for (int a = 0; a < called.chromNum; a++) { //hitung fitness
				called.calcFitness (a);
			}

			called.getBestChrom ();

			time += Time.deltaTime;
			iterationTotal++;
			if (lasthighestfit < called.bestchromFit) {
				lasthighestfit = called.bestchromFit;
				File.AppendAllText ("test boker.txt", lasthighestfit.ToString () + " " + iterationTotal.ToString () + " \r\n");
				samefor = 0;
			} else
				samefor++;
			print ("samefor : " + samefor);
			print ("all best fitness" + called.bestchromFit); //
			print ("temp best fitness : " + Mathf.Max (called.genfits));
			print ("iteration " + iterationTotal);
			print ("time 2 " + time);






			if (Mathf.Max (called.genfits) == 0) {
				pause = true;
				print ("caught");
				string filename = "log.txt";
				var sr = File.CreateText (filename);
				for (int a = 0; a < called.chromNum; a++) {
					for (int b = 0; b < called.gentotal; b++) {
						sr.Write ((int)called.gens [a] [b]);
					}
					sr.WriteLine ("");
					sr.WriteLine ("");
				}
				sr.Close ();
			}
		} 

		{
		//	SceneManager.LoadScene("inGame", LoadSceneMode.Single);
		}

		//animasi teks loading
		if (SceneManager.GetActiveScene ().name == "Loading" && done==false) {
			loadtxt = GameObject.Find("txtLoad").GetComponent<Text>();
			if (hudTimer > 2.667f) {
				loadtxt.text = "Loading";
				hudTimer = 0;
			} else if (hudTimer > 2.000f) {
			loadtxt.text = "Loading . . .";
			} else if (hudTimer > 1.333f) {
				loadtxt.text = "Loading . .";
			} else if (hudTimer > 0.667f) {
				loadtxt.text = "Loading .";
			} 




			hudTimer += Time.deltaTime;
		}
	}
}
