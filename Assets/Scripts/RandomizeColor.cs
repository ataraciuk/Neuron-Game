using UnityEngine;
using System.Collections;

public class RandomizeColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SetColor();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void SetColor(){
		this.renderer.material.color = new Color(Random.value, Random.value, Random.value);
	}
}
