using UnityEngine;
using System.Collections;

public class TakePicture : MonoBehaviour {
	
	public string deviceName;
    WebCamTexture wct;
	
    // For saving to the _savepath
    private string _SavePath = "/Users/Shared/Unity/"; //Change the path here!
    int _CaptureCounter = 0;
	
	// Use this for initialization
	void Start () {
		wct = new WebCamTexture(WebCamTexture.devices[0].name);
		wct.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    void TakeSnapshot()
    {
       Texture2D snap = new Texture2D(wct.width, wct.height);
       snap.SetPixels(wct.GetPixels());
       snap.Apply();

       System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
       ++_CaptureCounter;
    }
}
