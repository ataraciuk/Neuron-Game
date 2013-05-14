using UnityEngine;
using System.Collections;

public class PlayWin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetTexture().Play();
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
