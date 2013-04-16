using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Move : MonoBehaviour {
	
	private Vector3 Speed = Vector3.forward / 5;
	private Vector3 startPos;
	private float torsoZMultiplier = 200.0f;
	private float torsoYMultiplier = 50.0f;
	private float alpha = 0.4f;
	private List<float> speeds = new List<float>();
	private List<float> velocities = new List<float>();
	private int sampleBufferSize = 40;
		
	// Use this for initialization
	void Start () {
		startPos = this.transform.position;
		for(int i = 0; i < sampleBufferSize; i++) {
			speeds.Add(0.0f);
			velocities.Add(0.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.position += Speed;
		//this.transform.position += Vector3.up * ( Mathf.Sin( Time.fixedTime * 1f ) * 0.3f);
	}
	
	void TorsoMoved(Vector3 torsoPos) {
//		var absx = Mathf.Abs(torsoPos.x);
//		//absx = alpha * absx + (1 - alpha) * backoff;
//		speeds.RemoveAt(0);
//		velocities.RemoveAt(0);
//		speeds.Add(absx);
//		velocities.Add(torsoPos.x);
//		float mappedSpeed = Mathf.Log(1+ (speeds.Average() - Mathf.Abs(velocities.Average())));
//		this.transform.position += new Vector3(0, /*torsoPos.y * torsoYMultiplier*/ 0, mappedSpeed * torsoZMultiplier);
	}
	void HipsMoved(float val) {
		var absx = Mathf.Abs(val);
		//absx = alpha * absx + (1 - alpha) * backoff;
		speeds.RemoveAt(0);
		velocities.RemoveAt(0);
		speeds.Add(absx);
		velocities.Add(val);
		float mappedSpeed = Mathf.Log(1+ (speeds.Average() - Mathf.Abs(velocities.Average())));
		this.transform.position += new Vector3(0, /*torsoPos.y * torsoYMultiplier*/ 0, mappedSpeed * torsoZMultiplier);		
	}
}
