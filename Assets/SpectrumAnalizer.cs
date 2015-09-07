using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpectrumAnalizer : MonoBehaviour {

	public List<float> spectrumValues;
	public AudioSource AS;

	// Use this for initialization
	void Start () {
		int num = 128;
		float[] spectrum = AS.GetSpectrumData(num, 0, FFTWindow.Blackman);
		spectrumValues = new List<float>();
	}
	
	// Update is called once per frame
	void Update () {
		int num = 128;
		float[] spectrum = AS.GetSpectrumData(num, 0, FFTWindow.Blackman);
		spectrumValues = new List<float>();
		for (int i=0;i<num;i++){
			spectrumValues.Add(spectrum[i]);
			//Debug.Log(spectrumValues[i]);
		}
	}
}
