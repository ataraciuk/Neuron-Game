using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour {
	
	public Material[] Movies;
	private int current = 0;
	

	// Use this for initialization
	void Start () {
		renderer.sharedMaterial = Movies[current];
		GetTexture().Play();
	}
	
	MovieTexture GetTexture() {
		return (MovieTexture)renderer.sharedMaterial.mainTexture;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GetTexture().isPlaying){
			GetTexture().Stop();
			current = (current + 1) % Movies.Length;
			renderer.sharedMaterial = Movies[current];
			GetTexture().Play();
		}
	}
}
