using UnityEngine;
using System.Collections;

public class CollisionSmallBdnf : MonoBehaviour {

	public GameObject Other;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject == Other) {
			Debug.Log("collision!");
		}
	}
}
