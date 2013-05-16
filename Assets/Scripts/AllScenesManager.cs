using UnityEngine;
using System.Collections;

public class AllScenesManager : MonoBehaviour {
	
	public string OSCeletonPath;
	
	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R) && Application.loadedLevel > 0) Application.LoadLevel(0);
		if (Input.GetKeyDown(KeyCode.L)) Screen.lockCursor = !Screen.lockCursor;
		if (Input.GetKeyDown(KeyCode.S)) Application.LoadLevel((Application.loadedLevel + 1) % Application.levelCount);
	}
}
