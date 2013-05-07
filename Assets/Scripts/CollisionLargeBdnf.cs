using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CollisionLargeBdnf : MonoBehaviour {

	public Transform Other;
	private GameObject PictureTaker;
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
			collided = true;
		}
	}
	
	void SetPictureTaker(GameObject pt){
		PictureTaker = pt;
	}
}
