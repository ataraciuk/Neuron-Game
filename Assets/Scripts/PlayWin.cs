using UnityEngine;
using System.Collections;

public class PlayWin : MonoBehaviour {
	
	public GameObject Badge10;
	
	// Use this for initialization
	void Start () {
		GetTexture().Play();
		iTween.ScaleTo(Badge10, iTween.Hash(
			"scale", Vector3.one,
			"delay", 10.0f,
			"time", 0.4f
		));
	}
	
	// Update is called once per frame
	void Update () {
		if(!GetTexture().isPlaying){
			Application.LoadLevel(0);
		}
	}
	
	MovieTexture GetTexture() {
		return (MovieTexture)renderer.sharedMaterial.mainTexture;
	}
}
