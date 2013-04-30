using UnityEngine;
using System.Collections;

public class CollisionLargeBdnf : MonoBehaviour {

	public Transform Other;
	private GameObject PictureTaker;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.Rotate(1,1,0);
	}
	
	void OnTriggerEnter(Collider other) {
		PictureTaker.SendMessage("TakeSnapshot");
		Debug.Log("collision with other");
	}
	
	void SetPictureTaker(GameObject pt){
		PictureTaker = pt;
	}
}
