using UnityEngine;
using System.Collections;

public class CollisionLargeBdnf : MonoBehaviour {

	public Transform Other;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.Rotate(1,1,0);
	}
	
	void OnTriggerEnter(Collider other) {
		Debug.Log("collision with other");
	}
}
