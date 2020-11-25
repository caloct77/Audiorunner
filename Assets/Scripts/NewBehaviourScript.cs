using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFTs;

[RequireComponent(typeof(AudioSource))]
public class NewBehaviourScript : MonoBehaviour {

	public AudioClip AC;
	public AudioSource AS;

	float[] spec = new float[2048];
 	float[] tmp = new float[4096];
	Complex[] spec2 = new  Complex[4096];

	int tester=0;
	// Use this for initialization
	void Start () {
		AS.clip = AC;
		AS.Play ();

	}
	
	// Update is called once per frame
	void Update () {
		


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


}
}
