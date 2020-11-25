using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFTs;

[RequireComponent(typeof(AudioSource))]
public class secEarlier : MonoBehaviour {

	public AudioClip AC;
	public AudioClip ac2;
	public AudioSource AS;
	public AudioSource later;

	float[] spec = new float[8192];
	float[] speclater = new float[2048];
	float[] tmp = new float[4096];
	Complex[] spec2 = new  Complex[4096];
	int count = 1;
	float timer=4f;
	public int tester=0;
	bool hasrun=false;

	float timerloop=0.04f;
	int timerloopcount=0;
//	int[][] accumulate= new int[100][2048];

	// Use this for initialization
	void Start () {
		
		AS.clip = AC;
		AS.Play ();
		Debug.Log ("d");
		later.clip = ac2;Debug.Log (AudioSettings.outputSampleRate);
	}

	// Update is called once per frame
	void Update () {

		//if (tester == 0) {
			AS.GetSpectrumData (spec, 0, FFTWindow.BlackmanHarris);
			tester++;

			float big = 0;
		int c = 0;
			//AS.volume = 0;
		for (int a = 0; a < 8192; a++)
			if (spec [a] > big) {
				big = spec [a];
				c = a;
			}
			Debug.Log (big + " " + c);
		//}
		if (timerloopcount >= 99) {
			timerloopcount = 0;
		}

		if (timer <= 0f && !hasrun) {
			later.Play ();
			later.GetSpectrumData (speclater, 0, FFTWindow.BlackmanHarris);

		/*	Debug.Log (timer);
			Debug.Log (spec [0] + " " + speclater [0]);
			Debug.Log (spec [200] + " " + speclater [200]);
			Debug.Log (spec [400] + " " + speclater [400]);
			Debug.Log (spec [600] + " " + speclater [600]);
			Debug.Log (spec [800] + " " + speclater [800]);
			Debug.Log (spec [1000] + " " + speclater [1000]);
			Debug.Log (spec [1200] + " " + speclater [1200]);
			Debug.Log (spec [1400] + " " + speclater [1400]);
			Debug.Log (spec [1600] + " " + speclater [1600]);
			Debug.Log (spec [1800] + " " + speclater [1800]);*/
			Debug.Log (later.clip.samples +" " + later.clip.length + " " + later.clip.samples / later.clip.length);
			hasrun = true;
			if (timerloop < 0f) {
			//	speclater.CopyTo(accumulate[timerloopcount]);
				timerloopcount++;
				timerloop = 0.04f;
			
			}
		}


		timer -= Time.deltaTime;
		timerloop -= Time.deltaTime;
		//Debug.Log (timer);
		/*

		// Unity's FFT function
		AudioListener.GetSpectrumData(spec, 0, FFTWindow.BlackmanHarris);
		for (int i = 0; i < spec.Length; i++)
		{
			Debug.DrawLine(new Vector3(i, 0,0), new Vector3(i, spec[i]*18000), Color.black);

		}

		// My FFT based on the output samples.
		AudioListener.GetOutputData(tmp, 0);
		// copy the output data into the complex array
		for(int i = 0; i < tmp.Length; i++)
		{
			spec2[i] = new  Complex(tmp[i],0);
		}
		// calculate the FFT
		FFT.CalculateFFT(spec2, false);

		for (int i = 0; i < spec2.Length/2; i++) // plot only the first half
		{
			// multiply the magnitude of each value by 2
			Debug.DrawLine(new Vector3(i, 0,40), new Vector3(i, (float)spec2[i].magnitude*36000,40), Color.white);
			if (spec2 [i].magnitude * 18000 > 300)
				Debug.Log (i);
		}

		*/


	}
}
