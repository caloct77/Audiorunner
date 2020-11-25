using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class levelGen : MonoBehaviour {
	GameObject go;
	int []obsData;
	GA called;
	public int []totOb;

	public bool initiated=false;
	int[] cpyArr;
	// Use this for initialization


	void testarray(string sr) //buat tes biar gausah GA sek
	{
		//print (sr [10]);
		cpyArr = new int[sr.Length];
		for (int a = 0; a < sr.Length; a++) {
			cpyArr [a] = sr [a] - '0';

		}

	}

	void createBase()//bikin lantai kosong
	{
		GameObject baseTile;
		baseTile = (GameObject)Resources.Load ("Obstacles/ground");
		if (baseTile == null)
			print ("nope");
		for (int a = 0; a < 10; a++) {//buat kirie player
			var inst = Instantiate (baseTile, new Vector3 (a * 2 - 20, 1, 1), new Quaternion (0, 0, 0, 1));
		}
		for (int a = 0; a < called.bestchrom.Length+20; a++) { //+20 biar ga putus
			var inst = Instantiate(baseTile,new Vector3(a*2,1,1),new Quaternion(0,0,0,1));
			
		}
		print ("basetile done");
	}

	void putObstacles()//naruh susunan e
	{
		GameObject []obs;

		obs = new GameObject[4];
		obs[0] = (GameObject)Resources.Load ("Obstacles/obsJump");
		obs[1] = (GameObject)Resources.Load ("Obstacles/obsSlide");
		obs[2] = (GameObject)Resources.Load ("Obstacles/obsRev");
		obs[3] = (GameObject)Resources.Load ("Obstacles/obsAtk");
		int obsDeploy;

		for (int a = 0; a < called.bestchrom.Length; a++) {
			
			obsDeploy=called.bestchrom[a]-1;
			if(obsDeploy>=0)
			totOb [obsDeploy] += 1;
			if (obsDeploy == 0)
				Instantiate (obs [obsDeploy], new Vector3 (a * 2 + 4, 2f, 1), new Quaternion (0, 0, 0, 1));
			else if (obsDeploy == 1)
				Instantiate (obs [obsDeploy], new Vector3 (a * 2 + 4, 2.5f, 1), new Quaternion (0, 0, 0, 1));
			else if (obsDeploy == 2) {
				GameObject ramp = Instantiate (obs [obsDeploy], new Vector3 (a * 2 + 4f, 2f, 1), new Quaternion (0, 0, 0, 1));
				ramp.transform.Rotate(new Vector3(0,270,270));
			}
			else if(obsDeploy==3)
				Instantiate(obs[obsDeploy],new Vector3(a*2+4,2.25f,1), new Quaternion(0,0,0,1));
		}
	
	}
	void Awake()
	{
		totOb = new int[4];

		for (int a = 0; a < 4; a++) {
			totOb [a] = 0;
		}
	}
	void Start () {
		
		go = GameObject.Find ("GAcarrier");
		called = go.GetComponent<GA>();
		obsData = called.bestchrom;

		called.bestchrom.CopyTo (obsData,0);
		print (called.bestchromFit);
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene ().name == "inGame" && initiated==false) {
			
			/*																	INTERNAL TEST
			string sr = File.ReadAllText ("tes pass.txt");
			testarray (sr);
			obsData = new int[cpyArr.Length];

			//called.bestchrom.CopyTo (obsData, 0);
			cpyArr.CopyTo(obsData,0);
			*/                                        
			//print (called.bestchromFit);
			print(obsData.Length);
			createBase ();
			putObstacles ();
			initiated = true;
			/*var fileName = "tes pass.txt";
			var sr = File.CreateText (fileName);
			for (int a = 0; a < called.gentotal; a++) {
				sr.Write ((int)called.bestchrom [a]);
			}*/
			//sr.Close ();
		}
	}
}
