using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TSPS;

public class EmotionTrigger : MonoBehaviour {
	
	public Material	[] materials;
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Surprised(){
		this.gameObject.renderer.sharedMaterial = materials[2];
	}
	
	public void Happy(){
		this.gameObject.renderer.sharedMaterial = materials[3];
	}
	
	public void Sad(){
		this.gameObject.renderer.sharedMaterial = materials[4];
	}
	
	public void Neutral(){
		this.gameObject.renderer.sharedMaterial = materials[0];
	}
}
