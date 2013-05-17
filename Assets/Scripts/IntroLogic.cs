using UnityEngine;
using System.Collections;

public class IntroLogic : MonoBehaviour {
	
	public GameObject Badge03;
	public GameObject Badge04;
	
	// Use this for initialization
	void Start () {
		iTween.FadeTo(Badge03, iTween.Hash(
			"alpha", 0.0f,
			"time", 1.0f,
			"delay", 5.0f,
			"oncomplete", "ShowOther",
			"oncompletetarget", this.gameObject
		));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowOther(){
		Badge04.SetActive(true);
	}
}
