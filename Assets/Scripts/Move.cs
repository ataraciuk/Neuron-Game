using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Move : MonoBehaviour {
	
	private Vector3 Speed = Vector3.forward / 5;
	private Vector3 startPos;
	private float hipsZMultiplier = 200.0f;
	private float torsoYMultiplier = 50.0f;
	private float alpha = 0.4f;
	private List<float> speeds = new List<float>();
	private List<float> velocities = new List<float>();
	private int sampleBufferSize = 80;
	private List<float> torsoPos = new List<float>();
	private int jumpLongBufferSize = 5;
	private int jumpBufferSize = 5;
	private float jumpThreshold = -1.0f;
	private float jumpMult = 0.15f;
	private bool jumping = false;
	private float jumpHeight = 7.0f;
	private float jumpTime = 0.2f;
	private float jumpingSpeed = 0.0f;
	private float endJumpTime = 0.0f;
	private bool endedJump = false;
		
	// Use this for initialization
	void Start () {
		startPos = this.transform.position;
		for(int i = 0; i < sampleBufferSize; i++) {
			speeds.Add(0.0f);
			velocities.Add(0.0f);
		}
		for(int i = 0; i < jumpLongBufferSize; i++) {
			torsoPos.Add(0.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(endedJump && Time.fixedTime > endJumpTime + 0.5f) {
			jumping = false;
			endedJump = false;
		}
		//this.transform.position += Speed;
		//this.transform.position += Vector3.up * ( Mathf.Sin( Time.fixedTime * 1f ) * 0.3f);
	}
	
	void TorsoMoved(float val) {
		torsoPos.RemoveAt(0);
		torsoPos.Add(val);
		if(jumpThreshold >= 0.0f && torsoPos.Sum() - torsoPos.Skip(sampleBufferSize - jumpBufferSize).Sum() > jumpThreshold * jumpMult) {
			OnJump();
		}
		//this.transform.position += new Vector3(0, val * torsoYMultiplier, 0);
	}
	void HipsMoved(float val) {
		if(!jumping) {
			var absx = Mathf.Abs(val);
			//absx = alpha * absx + (1 - alpha) * backoff;
			speeds.RemoveAt(0);
			velocities.RemoveAt(0);
			speeds.Add(absx);
			velocities.Add(val);
		}
		float mappedSpeed = Mathf.Log(1+ (speeds.Average() - Mathf.Abs(velocities.Average())));
		this.transform.position += new Vector3(0, /*torsoPos.y * torsoYMultiplier*/ 0, mappedSpeed * hipsZMultiplier);
	}
	
	void UpdateJumpThreshold(float val){
		//Debug.Log(val);
		jumpThreshold = val;
	}
	
	void OnJump() {
		if(!jumping) {
			jumping = true;
			iTween.MoveAdd(this.gameObject, iTween.Hash(
				"amount", Vector3.up * jumpHeight,
				"time", jumpTime,
				"easetype", iTween.EaseType.easeOutQuad,
				"oncomplete", "Fall",
				"oncompletetarget", this.gameObject
			));
		}
	}
	
	void Fall() {
		iTween.MoveAdd(this.gameObject, iTween.Hash(
			"amount", Vector3.down * jumpHeight,
			"time", jumpTime,
			"easetype", iTween.EaseType.easeInQuad,
			"oncomplete", "FallComplete",
			"oncompletetarget", this.gameObject
		));
	}
	
	void FallComplete(){
		endedJump = true;
		endJumpTime = Time.fixedTime;
	}
}
