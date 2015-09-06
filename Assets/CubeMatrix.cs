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
	GameObject tempCube;//used to hold reference to cube as it is modified
	
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

				for (int k = 0;k<=3;k++){//for each quadrant
					if (k==0)
						tempCube = matrix[i][j];
					else if (k==1)
						tempCube = matrix[j][i];
					else if (k==2)
						tempCube = matrix[j][lastBinIndex-i];
					else if (k==3)
						tempCube = matrix[lastBinIndex-i][j];

					//modify the selected cube
					Vector3 scale = tempCube.transform.localScale;
					scale.y += value*inputMultiplier;
					scale.y *= dampening;//depreciation
					if (scale.y > binMaxHeight)
						scale.y = binMaxHeight;
					tempCube.transform.localScale = scale;
					tempCube.transform.position = new Vector3(tempCube.transform.position.x, tempCube.transform.localScale.y/2, tempCube.transform.position.z);

					if (colorsEnabled)
						hsbColor = new HSBColor(scale.y/binMaxHeight,1.0f,1.0f,1.0f);
					tempCube.gameObject.GetComponent<Renderer>().material.color = hsbColor.ToColor();
				}
			}
			i++;
		}
	}
}