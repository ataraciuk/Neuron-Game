using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TakePicture : MonoBehaviour {
	
	public string deviceName;
    WebCamTexture wct;
	
    // For saving to the _savepath
    //private string _SavePath = "/Users/Shared/Unity/"; //Change the path here!
	private List<Texture2D> pics = new List<Texture2D>();
	private const int xSize = 400;
	private const int ySize = 300;
	private const float xTumb = 80.0f;
	private const float yTumb = (float) ySize * xTumb / (float) xSize;
	private const string dropbox = "/Users/ataraciuk/Dropbox/Public/neuron-test/";
	private const string jsname = "js/lastImage.js";
	private int lastImage;
	
	// Use this for initialization
	void Start () {
		Debug.Log(WebCamTexture.devices.Length);
		Debug.Log(WebCamTexture.devices[0].name);
		wct = new WebCamTexture(WebCamTexture.devices[0].name, xSize, ySize);
		wct.Play();
		string ln = System.IO.File.ReadAllLines(dropbox + jsname)[0];
		string shouldBeAnInt = ln.Replace("var lastImage = ", "").Replace(";", "");
		Debug.Log (shouldBeAnInt);
		lastImage = int.Parse(shouldBeAnInt);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    void TakeSnapshot()
    {
		Texture2D snap = new Texture2D(wct.width, wct.height);
		snap.SetPixels(wct.GetPixels());
		snap.Apply();
		int newLast = pics.Count + lastImage + 1;
		System.IO.File.WriteAllBytes(dropbox + "images/" + newLast.ToString() + ".png", snap.EncodeToPNG() );
		System.IO.File.WriteAllText(dropbox + jsname, "var lastImage = "+newLast.ToString()+";");
		pics.Add(snap);
    }
	
	void OnGUI(){
		float starting = 8.0f, yPos = 0.0f;
		pics.ForEach(delegate(Texture2D pic){
			GUI.DrawTexture(new Rect(Screen.width - xTumb - starting, yPos + starting, xTumb, yTumb), pic);
			yPos += yTumb + starting;
		});
	}
}
