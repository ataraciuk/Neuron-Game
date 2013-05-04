using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttractLogic : MonoBehaviour {
	
	public GameObject Badge01;
	private int jumps = 0;
	private int required = 2;
	public TextMesh requiredTxt;
	public TextMesh jumpLbl;
	public IDictionary<int, UserMovement> users = new Dictionary<int, UserMovement>();
	private bool jumping = false;
	
	// Use this for initialization
	void Start () {
		Badge01.transform.localScale = Vector3.zero;
		requiredTxt.text = required.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void HipsMoved(KeyValuePair<int, float> keypair){
		NewOrGet(keypair.Key);
		iTween.ScaleTo(Badge01, Vector3.one, 3.0f);	
	}
	
	void TorsoMoved(KeyValuePair<int, float> keypair) {
		Debug.Log ("torso");
		var user = NewOrGet(keypair.Key);
		user.torsoPos.RemoveAt(0);
		user.torsoPos.Add(keypair.Value);
		if(user.HasToJump()) {
			OnJump();
		}
		//this.transform.position += new Vector3(0, val * torsoYMultiplier, 0);
	}
	
	void OnJump(){
		if(!jumping) {
			jumping = true;
			jumps++;
			var remaining = required - jumps;
			requiredTxt.text = remaining.ToString();
			jumpLbl.text = remaining == 1 ? "jump" : "jumps";
			if(remaining <= 0) {
				Application.LoadLevel(1);
			}
			iTween.MoveTo(this.gameObject, iTween.Hash(
				"position", Vector3.zero,
				"delay", 0.5f,
				"oncomplete", "EndedJump",
				"oncompletetarget", this.gameObject
			));
		}
	}
	
	void EndedJump(){
		jumping = false;
	}
	
	UserMovement NewOrGet(int id){
		if(!users.ContainsKey(id)) {
			users.Add(id, new UserMovement());
		}
		return users[id];
	}
	
	void UpdateJumpThreshold(KeyValuePair<int, float> keypair){
		var user = NewOrGet(keypair.Key);
		user.jumpThreshold = keypair.Value;
	}
}
