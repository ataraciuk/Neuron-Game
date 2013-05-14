using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CollisionLargeBdnf : MonoBehaviour {

	public Transform Other;
	private GameObject PictureTaker;
	private GameObject MessageCaught;
	private bool collided = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.Rotate(1,1,0);
	}
	
	void OnTriggerEnter(Collider other) {
		if(!collided) {
			PictureTaker.SendMessage("TakeSnapshot");
			this.audio.Play();
			this.GetComponentsInChildren<Renderer>().ToList().ForEach(delegate(Renderer r){
				r.enabled = false;
			});
			this.MessageCaught.SetActive(true);
			iTween.FadeTo(this.MessageCaught, 1.0f, 0.1f);
			iTween.FadeTo(this.MessageCaught, iTween.Hash(
				"alpha", 0.0f,
				"time", 0.4f,
				"delay", 2.0f
			));
			collided = true;
		}
	}
	
	void SetPictureTaker(GameObject pt){
		PictureTaker = pt;
	}
	
	void SetMessageCaught(GameObject m) {
		MessageCaught = m;
	}
}
