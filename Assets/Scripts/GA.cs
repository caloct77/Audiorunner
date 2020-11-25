using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFTs;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class GA : MonoBehaviour {

	// GameObject.Find("ThePlayer").GetComponent<PlayerScript>().Health -= 10.0f;;
	//---PENGUJIAN
	public int[] cocok;
	public int []totalnol;

	public int totalnolbest;
	public int cocokbest;
	//
	//AudioClip tempAC = GameObject.Find("Capsule").GetComponent<secEarlier>().AC;
	public AudioClip tempAC;
	int timeAccuracy = 500; //time accuracy in ms
	public int difficultyObstacle = 3; //3,5,7 E,N,H
	public int songDur = 0; //song duration
	int samplerate=0; //sample rate

	public int[][] gens; //array utama GA
	public int gentotal; //total gen per kromosom
	public int []genfits; //array fitness GA
	public int chromNum=10; //total kromosom

	int totalsample; //total sample in song
	float[]samples; //all audio samples

	public int []spectrumDom; //spectrum dominance per 4s
	Complex []spec; //spectrum data

	public float[] bands = new float[4];

	double band1;
	double band2;
	double band3;
	double band4; //band terkuat yg mana buat dimasukin spectrumdom

	public int []crossed1; //kromosom hasil crossover
	public int []crossed2;


	public int bestchromFit=-1;
	public int[]bestchrom;
	public int bestchromnum = -1;

	public bool isLoadDone=false;
	FileInfo fileSize;

	public void randomPopulation(int chrom) //chrom = kromosom ke brp yg random
	{
		int totalrandom = difficultyObstacle * (songDur / (4000/timeAccuracy)); 

		int[] randoms = new int[totalrandom];

		for (int a = 0; a < totalrandom; a++) {
			randoms [a] = Random.Range (0, gentotal-1);
			for (int b = 0; b < a; b++) {
				if (randoms [a] == randoms [b]) {
					randoms[a]=Random.Range (0, gentotal-1);
					b = 0;
					
				}
			}
		
		} //random gen ke brp yang diisi

		for (int a = 0; a < gentotal; a++) {
			gens [chrom] [a] = 0;		
		}
	
		for (int a = 0; a < totalrandom; a++) {
			//print (randoms [a]);
		/*	for (int b = 0; b < gentotal; b++) {
				if (b == randoms [a]) {
					gens [chrom] [b] = Random.Range (1, 5);
				} else
					gens [chrom] [b] = 0;
			
			}
*/
			gens [chrom] [randoms [a]] = Random.Range (1, 5);
		} // random isi dari gen
	}


	public void calcSpectrum()
	{
		
		//8192 per 4s, spt pake 2048 di fungsi unity biasa
	//	print(spectrumDom.Length);
		int currentSample=0; //sample sudah sampe mana
		for (int d = 0; d < 4; d++) {
			bands [d] = 0;
		}

		int c=0;
		for (int a = 0; a < spectrumDom.Length; a++) {
			c = 0;
			spec = new Complex[8192];
			for (int b = currentSample; b <(samplerate*4)+currentSample && b<totalsample && c<8192; b +=samplerate*4/8192) {
				

				spec [c] = new Complex(samples [b],0);
				c++;
				//TODO: cek getoutputdata itu bener skip2 ato ngambil sample terakhir
			}
			currentSample += ((samplerate * 4));


	
			FFT.CalculateFFT(spec,false);
		
			// utk array size 8192, per index=2.93
			// each element represent amplitude of freq N*24000/(arrsize) hz
			float []tmpmag = new float[spec.Length/2];
			for (int d = 0; d < tmpmag.Length; d++) {
			
				tmpmag [d] = spec [d].fMagnitude;
			}
		//	int samplerate = tempAC.samples / songDur;
			int skip = samplerate  / 8192;
			//5.85 - > skip
			for (int d = 0; d < spec.Length / 2; d++) {
				if (spec [d].fMagnitude  > Mathf.Max(tmpmag)/10) { // 10% max magnitude buat eliminasi
					if (d * skip < 4000) {
						
						bands [0] += spec [d].fMagnitude * 1000;
						//print ("band1 " + d);
					} else if (d * skip < 8000) {
						
						bands [1] += spec [d].fMagnitude * 1000;//print ("band2 " + d);
					} else if (d * skip < 12000) {
						
						bands [2] += spec [d].fMagnitude * 1000;//print ("band3 " + d);
					} else if (d * skip < 16000) {
						
						bands [3] += spec [d].fMagnitude * 1000;//print ("band4 " + d);
						//print(d*skip);
					}
				}
				//print (spec [d].magnitude + " " + spec [d].fMagnitude);
			}



		//	spectrumDom [a] = Mathf.Max (band1, band2, band3, band4);
			for (int d = 0; d < 4; d++) {
			//	print (bands [d] + " " + d);
			}
			if (Mathf.Max (bands) == bands[0])
				spectrumDom [a] = 1;
			else if (Mathf.Max (bands) == bands[1])
				spectrumDom [a] = 2;
			else if (Mathf.Max (bands) == bands[2])
				spectrumDom [a] = 3;
			else if (Mathf.Max (bands) == bands[3])
				spectrumDom [a] = 4;
		//	print(Mathf.Max(bands) + " " + bands[0] + " " + bands[1] + " " + bands[2] + " " + bands[3]);


		} //dapetin band terkuat

		var fileName = "spectrumDom.txt";
			var sr = File.CreateText (fileName);
		for (int a = 0; a < spectrumDom.Length; a++) {
			sr.Write ((int)spectrumDom [a]);
			}
		sr.Close ();
	}

	public void calcFitness(int chrom) //kromosom ke brp
	{
		//diff obs, gens, spectrumdom, insert ke genfits

		int currfit=0;//temp fit
		int obsc = 0;//count obs

		cocok [chrom] = 0;
		totalnol [chrom] = 0;
		for(int a=0;a<spectrumDom.Length;a++)
		{
			for(int b=0;b<8 && b<gentotal;b++){
			//print (b + " " + gens[chrom].Length + " " + gentotal);
		//	print(spectrumDom.Length + " " + ske);
				if (gens [chrom] [b] == spectrumDom [a]) {
					currfit += 600;
					obsc++;
					cocok[chrom]++;

				} else if (gens [chrom] [b] == 0) {
					//currfit+=200
					currfit+=(5-difficultyObstacle)*50; //TAMBAHAN BARU
					totalnol [chrom]++;
				} else {
					currfit += 150; //TODO:balancing sesuai proporsi spectrum
					obsc++;

				}//cek isi gen


			}
			int obscmp =Mathf.Abs( difficultyObstacle - obsc ); //optimal-actual
			int defaultfit=600*difficultyObstacle; 
			int factor=(int)Mathf.Pow (2f,Mathf.Abs(difficultyObstacle-obscmp));
			//print (obscmp + " " + factor);
			int thisfit=defaultfit- (defaultfit/factor); //fitness skrg

			obsc = 0;
			currfit+=thisfit;
			//TODO : rebalancing


		genfits [chrom] = currfit;
		//currfit = 0;
		}
	}

	public void crossover(int chrom1,int chrom2)
	{
		int bp1 = Random.Range (1, gentotal - 2);
		int bp2 = Random.Range (bp1 + 1, gentotal);

		for (int a = 0; a < gentotal; a++) {
			if (a < bp1 || a > bp2) {
				crossed1 [a] = gens [chrom2][a];
				crossed2 [a] = gens [chrom1][a];
			//	if(a<0 || a>gentotal)
			//	print (a + " " + chrom1 + " " + chrom2);
				/*int t = gens [chrom1] [a];
				gens [chrom1] [a] = gens [chrom2] [a];
				gens [chrom2] [a] = t;*/
			} 
			else {
				crossed1 [a] = gens [chrom1][a];	
				crossed2 [a] = gens [chrom2][a];
			}
		}

	}

	public void mutation(int chrom) //mutasi kromosom ke brp
	{
		int rnd = Random.Range (0, 100);
		int mutateCnt = 0;//brp gen yg mutasi
		if (rnd < 10)
			mutateCnt++;
		if (rnd < 15)
			mutateCnt++;
		if (rnd < 18)
			mutateCnt++;
		if (rnd < 20)
			mutateCnt++;
		//print ("mutasi " + mutateCnt);
		//print (rnd);
		//mutateCnt=0; //TODO:ilangno, buat test tok
		int[] chosenOne = new int[mutateCnt]; //index yg dimutasi

		for (int a = 0; a < mutateCnt; a++)
			chosenOne [a] = -1;

		for (int a = 0; a < mutateCnt; a++) {
			rnd = Random.Range (0, gentotal);
			for (int b = 0; b < mutateCnt; b++) {
				if (chosenOne [b] == rnd) {
					rnd = Random.Range (0, gentotal);
					b = 0;
				}
			}
			chosenOne [a] = rnd;
		}


		for (int a = 0; a < mutateCnt; a++) {
			gens [chrom] [chosenOne [a]]=Random.Range(0,5);
		//	print (chosenOne [a] +" " + chrom);
		}
	}

	public void crossenter() //insert hasil crossover, ambil yang terkecil di gens 2x
	{
		int min = Mathf.Min (genfits);
		for (int a = 0; a < chromNum; a++) {
			if (genfits [a] == min) {
				gens [a].CopyTo(crossed1,0);// = crossed1;
				calcFitness (a);
				//print (genfits [a]);
			}
		}
		min = Mathf.Min (genfits);
		for (int a = 0; a < chromNum; a++) {
			if (genfits [a] == min) {
				gens [a].CopyTo (crossed2,0);
				calcFitness (a);
				//print (genfits [a]);
			}
		}
	}


	public void selection()
	{
		int replace = -1;
		int min = Mathf.Min (genfits);
		for (int a = 0; a < chromNum; a++) {
			if (genfits [a] == min)
				replace = a;
		}
		randomPopulation (replace);

	}

	public void getBestChrom()
	{
		int bestchromNum=0;
		if (bestchromFit < Mathf.Max (genfits)) {
			
			bestchromFit = Mathf.Max (genfits);

			for (int a = 0; a < chromNum; a++) {
				if (genfits [a] == bestchromFit)
					bestchromNum = a;
			}
			gens [bestchromNum].CopyTo (bestchrom,0);
			bestchromnum = bestchromNum; // testing = temp
			totalnolbest=totalnol[bestchromnum];
			cocokbest = cocok [bestchromnum];
			//bestchrom = gens [bestchromNum];
		}
	}

	// Use this for initialization
	void Start () {

		//Init ();

	}

	public void Awake()
	{

		PlayerPrefs.DeleteKey ("songWarn");
		StartCoroutine(loadMusic());
		//isLoadDone = true;
		print (AudioSettings.outputSampleRate);

	}
	IEnumerator loadMusic()
	{
		string invSlash = PlayerPrefs.GetString ("songPath");
		print (invSlash);

		fileSize = new System.IO.FileInfo (invSlash);
		StringBuilder use = new StringBuilder(invSlash);
		for (int a = 0; a < use .Length; a++) {
			if (use [a] == '\\')
				use [a] = '/';
			
		}
		invSlash = use.ToString ();
		print (invSlash);
		WWW www = new WWW("file:///" + invSlash);

		//WWW www = new WWW("file:///h:/at2.ogg");
		while (www.progress < 0.1) {
			Debug.Log (www.progress);
			yield return new WaitForSeconds (0.1f);
		}

		
		//print (PlayerPrefs.GetString ("songPath"));
		tempAC = www.GetAudioClipCompressed(true,AudioType.OGGVORBIS);
		print (tempAC.name);
		isLoadDone = true;
		yield return www;




	}

	public void Init()
	{
		
		difficultyObstacle = PlayerPrefs.GetInt ("obsdiff");
		songDur = Mathf.RoundToInt(Mathf.Floor (tempAC.length));
		print (fileSize.Length);
		if (songDur > 600 || fileSize.Length > 12000000) {
			string warningSong = "File size must not exceed 12 Megabytes and song duration shorter than 10 minutes(600 seconds)";
			PlayerPrefs.SetString ("songWarn", warningSong);

			SceneManager.LoadScene("Difficulty", LoadSceneMode.Single);
		}


		totalsample = tempAC.samples;
		samples = new float[totalsample];
		tempAC.GetData (samples, 0);
		samplerate = tempAC.samples / songDur;
		spectrumDom = new int[Mathf.RoundToInt(Mathf.Ceil(tempAC.length / 4))];
		//spec = new Complex[samples.Length];

		genfits = new int[chromNum]; 
		gentotal = (songDur * (1000 / timeAccuracy)); // 1000/timeAccuracy tick per seconds
		bestchrom = new int[gentotal];
		gens = new int[chromNum][];
		for (int a = 0; a < chromNum; a++) {
			gens [a] = new int[gentotal]; 
		}

		for (int a = 0; a < 4; a++)
			bands [a] = 0;

		crossed1 = new int[gentotal]; //kromosom hasil crossover
		crossed2 = new int[gentotal];
		float []amp= new float[tempAC.samples];
		tempAC.GetData (amp, 0);
		for (int a = 0; a < tempAC.samples; a++) {
			amp [a] *= 18;
		
		}
		cocok = new int[10];
		totalnol = new int[10];
		//tempAC.SetData (amp, 0);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
