using UnityEngine;
using System.Collections;
 
public class CameraRotation : MonoBehaviour {
  
  public Material SkyBoxMat;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		SkyBoxMat.SetVector("Rotation",transform.localEulerAngles);
	}
}