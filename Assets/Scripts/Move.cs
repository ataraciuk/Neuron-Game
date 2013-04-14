using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	
	private Vector3 Speed = Vector3.forward / 5;
	private Vector3 startPos;
		
	// Use this for initialization
	void Start () {
		startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += Speed;
		//this.transform.position += Vector3.up * ( Mathf.Sin( Time.fixedTime * 1f ) * 0.3f);
	}
	
	void TorsoMoved(Vector3 torsoPos) {
		this.transform.position += torsoPos;
	}
}
