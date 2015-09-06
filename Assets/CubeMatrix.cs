using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeMatrix : MonoBehaviour {
	
	public GameObject cubePrefab;
	public float Multiplier = 200;
	public int bins = 25;
	public bool colorsEnabled = true;
	float binMaxHeight = 20.0f;
	float inputMultiplier = 0.15f;
	float dampening = 0.9f;
	float binSpacing = 2.0f;
	public AudioSource AS;

	List<List<GameObject>> matrix;
	HSBColor hsbColor;
	
	// Use this for initialization
	void Start () {
		//Audio source will begin playing automatically
		//Vector3 pos = new Vector3(0,0,-bins*0.75f - 1);
		//transform.position = pos;
		
		matrix = new List<List<GameObject>> ();
		for (int i=0; i<bins*2; i++) {
			List<GameObject> row = new List<GameObject>();
			for (int j=0;j<bins*2;j++){
				GameObject temp = Instantiate(cubePrefab, new Vector3(i*binSpacing+0.5f-bins*binSpacing,0.0f,j*binSpacing + 0.5f - bins*binSpacing),Quaternion.identity) as GameObject;
				//temp.transform.parent = transform;
				row.Add(temp);
			}
			matrix.Add(row);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		int num = 128;
		float[] spectrum = AS.GetSpectrumData(num, 0, FFTWindow.Blackman);
		int i = 0;
		
		int lastBinIndex = (bins * 2) - 1;
		if (!colorsEnabled)
			hsbColor = new HSBColor(Color.white);
		
		while (i<bins){
			float value = spectrum[i] * Multiplier;
			for (int j=0;j<bins*2;j++){
				
				Vector3 scale = matrix[i][j].transform.localScale;
				scale.y += value*inputMultiplier;
				scale.y *= dampening;//depreciation
				if (scale.y > binMaxHeight)
					scale.y = binMaxHeight;
				matrix[i][j].transform.localScale = scale;
				matrix[i][j].transform.position = new Vector3(matrix[i][j].transform.position.x, matrix[i][j].transform.localScale.y/2, matrix[i][j].transform.position.z);

				if (colorsEnabled)
					hsbColor = new HSBColor(scale.y/binMaxHeight,1.0f,1.0f,1.0f);
				matrix[i][j].gameObject.GetComponent<Renderer>().material.color = hsbColor.ToColor();
				
				scale = matrix[j][i].transform.localScale;
				scale.y += value*inputMultiplier;
				scale.y *= dampening;//depreciation
				if (scale.y > binMaxHeight)
					scale.y = binMaxHeight;
				matrix[j][i].transform.localScale = scale;
				matrix[j][i].transform.position = new Vector3(matrix[j][i].transform.position.x, matrix[j][i].transform.localScale.y/2, matrix[j][i].transform.position.z);

				if (colorsEnabled)
					hsbColor = new HSBColor(scale.y/binMaxHeight,1.0f,1.0f,1.0f);
				matrix[j][i].gameObject.GetComponent<Renderer>().material.color = hsbColor.ToColor();
				
				scale = matrix[j][lastBinIndex-i].transform.localScale;
				scale.y += value*inputMultiplier;
				scale.y *= dampening;//depreciation
				if (scale.y > binMaxHeight)
					scale.y = binMaxHeight;
				matrix[j][lastBinIndex-i].transform.localScale = scale;
				matrix[j][lastBinIndex-i].transform.position = new Vector3(matrix[j][lastBinIndex-i].transform.position.x, matrix[j][lastBinIndex-i].transform.localScale.y/2, matrix[j][lastBinIndex-i].transform.position.z);
				if (colorsEnabled)
					hsbColor = new HSBColor(scale.y/binMaxHeight,1.0f,1.0f,1.0f);
				matrix[j][lastBinIndex-i].gameObject.GetComponent<Renderer>().material.color = hsbColor.ToColor();
				
				scale = matrix[lastBinIndex-i][j].transform.localScale;
				scale.y += value*inputMultiplier;
				scale.y *= dampening;//depreciation
				if (scale.y > binMaxHeight)
					scale.y = binMaxHeight;
				matrix[lastBinIndex-i][j].transform.localScale = scale;
				matrix[lastBinIndex-i][j].transform.position = new Vector3(matrix[lastBinIndex-i][j].transform.position.x, matrix[lastBinIndex-i][j].transform.localScale.y/2, matrix[lastBinIndex-i][j].transform.position.z);

				if (colorsEnabled)
					hsbColor = new HSBColor(scale.y/binMaxHeight,1.0f,1.0f,1.0f);
				matrix[lastBinIndex-i][j].gameObject.GetComponent<Renderer>().material.color = hsbColor.ToColor();

			}
			i++;
		}
	}
}