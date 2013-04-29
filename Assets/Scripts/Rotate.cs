using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	
	Vector3 rotation;
	
	// Use this for initialization
	void Start () {
		float[] vals = new float[3];
		for(int i = 0; i < 3; i++){
			var rand = Random.value * 2;
			float dir = Random.value <= 0.5f ? 1.0f : -1.0f;
			vals[i] = rand * dir;
		}
		rotation = new Vector3(vals[0], vals[1], vals[2]);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(rotation);
	}
}
