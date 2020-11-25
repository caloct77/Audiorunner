using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour {
	public Slider vol;
	public InputField ifVol;
	public Text txtV;
	float baseVolume;
	public void OnValueChanged()
	{
		AudioListener.volume = vol.value/100f;
		txtV.text = "" + vol.value.ToString ("F2");
		txtV.SetAllDirty();
	}
	public void OnClicked(Button button)
	{
		print (button.name);

		if (button.name.Equals ("btnCancel")) {
			AudioListener.volume = baseVolume;
		}

		SceneManager.LoadScene ("Main Menu", LoadSceneMode.Single);
	}

	// Use this for initialization
	void Start () {
		baseVolume = AudioListener.volume;
		vol.value = baseVolume * 100f;
		txtV.text = "" + vol.value.ToString("F2");


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
