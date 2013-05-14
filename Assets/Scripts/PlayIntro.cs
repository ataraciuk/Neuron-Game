using UnityEngine;
using System.Collections;

public class PlayIntro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetTexture().Play();
	}
	
	// Update is called once per frame
	void Update () {
		if(!GetTexture().isPlaying){
			GetTexture().Stop();
			Application.LoadLevel(2);
		}
	}
	
	MovieTexture GetTexture() {
		return (MovieTexture)renderer.sharedMaterial.mainTexture;
	}
}
