using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Move : MonoBehaviour {
	
	private Vector3 Speed = Vector3.forward / 5;
	private Vector3 startPos;
	private float hipsZMultiplier = 0.2f;
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
	
	public Transform BDNF; 
	
	private float pathCompletion = 0.0f;
	private IEnumerable<Vector3> thePath;
	
	private int BDNFAmount = 10;
	private float firstBDNFPath = 0.05f;
	private float lastBDNFPath = 0.95f;
		
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
		Vector3[] path = {
			new Vector3(976.656f, 1160.348f, 57.45f),
			new Vector3(20.833f,	972.301f,	430.565f),
			new Vector3(-930.696f,	0.0f,	176.947f),
			new Vector3(-383.768f,	0.0f,	-489.477f),
			new Vector3(-158.563f,	335.323f,	75.834f),
			new Vector3(714.683f,	274.965f,	-402.153f),
			new Vector3(-140.179f,	0.0f,	-843.371f),
			new Vector3(737.663f,	-176.377f,	-1022.617f)/*
			new Vector3(-930.696f,	0.0f,	176.947f),
			new Vector3(-930.696f,	0.0f,	176.947f),
			new Vector3(-930.696f,	0.0f,	176.947f),
			new Vector3(-930.696f,	0.0f,	176.947f),
			new Vector3(-930.696f,	0.0f,	176.947f),
			new Vector3(-930.696f,	0.0f,	176.947f)*/		
		};
		path = path.Select(x => x * 0.1f).ToArray();
		thePath = Interpolate.NewCatmullRom(path, 1000, false);
		this.transform.position = path[0];
		Instantiate(BDNF, CRSpline.InterpConstantSpeed(thePath.ToArray(), firstBDNFPath),Quaternion.identity);
		Instantiate(BDNF, CRSpline.InterpConstantSpeed(thePath.ToArray(), lastBDNFPath),Quaternion.identity);
		var spread = (lastBDNFPath - firstBDNFPath) / BDNFAmount;
		for(int i = 1; i <= BDNFAmount - 2; i++) {
			Instantiate(BDNF, CRSpline.InterpConstantSpeed(thePath.ToArray(), firstBDNFPath + spread * i + (Random.value - 0.5f) * spread * 0.6f), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(endedJump && Time.fixedTime > endJumpTime + 0.5f) {
			jumping = false;
			endedJump = false;
		}
		//pathCompletion += 0.0001f;
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
		this.pathCompletion += mappedSpeed * hipsZMultiplier;
		pathCompletion = Mathf.Min(pathCompletion, 1.0f);
		Debug.Log(pathCompletion);
		this.transform.position += CRSpline.InterpConstantSpeed(thePath.ToArray(), pathCompletion) - this.transform.position;
		//this.transform.position += new Vector3(0, /*torsoPos.y * torsoYMultiplier*/ 0, mappedSpeed * hipsZMultiplier);
	}
	
	void UpdateJumpThreshold(float val){
		//Debug.Log(val);
		jumpThreshold = val;
	}
	
	void OnJump() {
		if(!jumping) {
			Debug.Log("JUMP!");
			jumping = true;
			iTween.MoveBy(this.gameObject, iTween.Hash(
				"amount", Vector3.up * jumpHeight,
				"time", jumpTime,
				"easetype", iTween.EaseType.easeOutQuad,
				"oncomplete", "Fall",
				"oncompletetarget", this.gameObject
			));
		}
	}
	
	void Fall() {
		iTween.MoveBy(this.gameObject, iTween.Hash(
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
