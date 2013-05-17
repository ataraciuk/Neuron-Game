using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Move : MonoBehaviour {
	
	private float hipsZMultiplier = 0.15f;
	private bool jumping = false;
	private float jumpHeight = 7.0f;
	private float jumpTime = 0.2f;
	private float endJumpTime = 0.0f;
	private bool endedJump = false;
	
	public Transform BDNF; 
	
	private float pathCompletion = 0.0f;
	private IEnumerable<Vector3> thePath;
	
	private int BDNFAmount = 10;
	private float firstBDNFPath = 0.1f;
	private float lastBDNFPath = 0.95f;
	private float BDNFHeight = 11.0f;
	
	private float standingSpeed = 0.0005f;
	
	public IDictionary<int, UserMovement> users = new Dictionary<int, UserMovement>();
	
	public Transform tubeRing;
	public Transform ribosome;
	
	
	const int ringAmount = 30;
	private Transform[] rings = new Transform[ringAmount];
	private float ringSeparation = 0.01f;
	private float[] ringOffsets = new float[ringAmount];
	private int ringToMove = 0;
	private Transform[] ribosomes = new Transform[ringAmount];
	private float ribosomeDist = 10.0f;
	private float ringBeginning = 0.0001f;
	
	private List<Vector3> ringPositions = new List<Vector3>();
	private List<Vector3> ringAngles = new List<Vector3>();
	private int currentCache = 0;
	
	public GameObject PictureTaker;
	
	public GameObject Badge05;
	private bool fadedB5 = false;
	
	public GameObject Badge09;
	
	private float previousSpeed = 0.0f;
		
	// Use this for initialization
	void Start () {
		Vector3[] path = {
			new Vector3(976.656f, 1160.348f, 57.45f),
			new Vector3(20.833f,	972.301f,	430.565f),
			new Vector3(-930.696f,	0.0f,	176.947f),
			new Vector3(-383.768f,	0.0f,	-489.477f),
			new Vector3(-158.563f,	335.323f,	75.834f),
			new Vector3(714.683f,	274.965f,	-402.153f),
			new Vector3(-140.179f,	0.0f,	-843.371f),
			new Vector3(737.663f,	-176.377f,	-1022.617f),
			new Vector3(883.723f,	-33.156f,	-685.02f),
			new Vector3(1559.897f,  118.9f,	-608.974f),
			new Vector3(1541.968f,	0.0f,	158.764f),
			new Vector3(549.226f,	0.0f,	-80.431f),
			new Vector3(-282.656f,	-344.386f,	-719.279f)
		};
		path = path.Select(x => x * 0.2f).ToArray();
		thePath = Interpolate.NewCatmullRom(path, 1000, false);
		this.transform.parent.transform.position = path[0];
		Transform theBdnf;
		theBdnf = (Transform)PutOnPath(BDNF, firstBDNFPath, Vector3.up * BDNFHeight);
		theBdnf.gameObject.SendMessage("SetPictureTaker", PictureTaker);
		theBdnf.gameObject.SendMessage("SetMessageCaught", Badge09);
		theBdnf = (Transform)PutOnPath(BDNF, lastBDNFPath, Vector3.up * BDNFHeight);
		theBdnf.gameObject.SendMessage("SetPictureTaker", PictureTaker);
		theBdnf.gameObject.SendMessage("SetMessageCaught", Badge09);
		var spread = (lastBDNFPath - firstBDNFPath) / BDNFAmount;
		for(int i = 1; i <= BDNFAmount - 2; i++) {
			theBdnf = (Transform)PutOnPath(BDNF, firstBDNFPath + spread * i + (Random.value - 0.5f) * spread * 0.7f, Vector3.up * BDNFHeight);
			theBdnf.gameObject.SendMessage("SetPictureTaker", PictureTaker);
			theBdnf.gameObject.SendMessage("SetMessageCaught", Badge09);
		}
		
		for(float i = ringBeginning; i <= 0.99f; i += ringSeparation){
			var splinePos = CRSpline.InterpConstantSpeed(thePath.ToArray(), i);
			ringPositions.Add(splinePos);
			ringAngles.Add(splinePos - CRSpline.InterpConstantSpeed(thePath.ToArray(), i * 0.99f));
		}
		
		for(int i = 0; i < rings.Length; i++){
			rings[i] = (Transform)PutOnPathCache(tubeRing, i, Vector3.zero, ringPositions, ringAngles);
			rings[i].Rotate(90, 0, 0);
			ringOffsets[i] = i * ringSeparation + ringBeginning;
			ribosomes[i] = (Transform)PutOnPathCache(ribosome, i, ribosomeDist * new Vector3(Random.value, Random.value, Random.value), ringPositions, ringAngles);
		}
		currentCache = rings.Length;
		//MoveBall();
	}
	
	// Update is called once per frame
	void Update () {
		if(endedJump && Time.fixedTime > endJumpTime + 0.5f) {
			jumping = false;
			endedJump = false;
		}
		pathCompletion += standingSpeed;
		MoveBall();
		if(pathCompletion > ringOffsets[ringToMove] + 0.06f){
			var previous = (ringToMove - 1 + ringAmount) % ringAmount;
			var nextOffset = ringOffsets[previous] + ringSeparation;
			if(currentCache < ringPositions.Count) {
				SetOnPathCache(rings[ringToMove], currentCache, Vector3.zero, ringPositions, ringAngles);
				SetOnPathCache(ribosomes[ringToMove], currentCache, ribosomeDist * new Vector3(Random.value, Random.value, Random.value), ringPositions, ringAngles);
				rings[ringToMove].Rotate(90, 0, 0);
				ringOffsets[ringToMove] = nextOffset;
				ringToMove = (ringToMove + 1) % ringAmount;
				currentCache++;
			}
		}
		if(!fadedB5 && pathCompletion > 0.02f){
			fadedB5 = true;
			iTween.FadeTo(Badge05, 0.0f, 1.0f);
		}
	}
	
	void TorsoMoved(KeyValuePair<int, float> keypair) {
		var user = NewOrGet(keypair.Key);
		user.torsoPos.RemoveAt(0);
		user.torsoPos.Add(keypair.Value);
		if(user.HasToJump()) {
			OnJump();
		}
		//this.transform.position += new Vector3(0, val * torsoYMultiplier, 0);
	}
	
	void HipsMoved(KeyValuePair<int, float> keypair) {
		var user = NewOrGet(keypair.Key);
		if(!jumping) {
			var val = keypair.Value;
			var absx = Mathf.Abs(val);
			user.speeds.RemoveAt(0);
			user.velocities.RemoveAt(0);
			user.speeds.Add(absx);
			user.velocities.Add(val);
		}
		float mappedSpeed = Mathf.Log(1+ (user.speeds.Average() - Mathf.Abs(user.velocities.Average())));
		if(previousSpeed > 0.0f && mappedSpeed * hipsZMultiplier > 0.0f && !this.audio.isPlaying){
			this.audio.Play();
		}
		this.pathCompletion += mappedSpeed * hipsZMultiplier;
		previousSpeed = mappedSpeed * hipsZMultiplier;
		pathCompletion = Mathf.Min(pathCompletion, 1.0f);
		//iTween.PutOnPath(this.gameObject, thePath.ToArray(), pathCompletion); //TODO Cache this
		MoveBall();
	}
	
	void MoveBall() {
		if(pathCompletion < 1.0f) {
			var toMove = CRSpline.InterpConstantSpeed(thePath.ToArray(), pathCompletion) - this.transform.parent.transform.position;
			Quaternion rotation = new Quaternion();
			if(toMove.magnitude > 0.0f) {
				rotation.SetLookRotation(toMove);
				this.transform.parent.transform.localRotation = rotation;
			}
			this.transform.parent.transform.position += toMove;
		} else {
			TriggerEnd();
		}
	}
	
	void TriggerEnd() {
		Application.LoadLevel(3);
	}
	
	void UpdateJumpThreshold(KeyValuePair<int, float> keypair){
		var user = NewOrGet(keypair.Key);
		user.jumpThreshold = keypair.Value;
	}
	
	void OnJump() {
		if(!jumping) {
			Debug.Log("JUMP!");
			jumping = true;
			iTween.MoveAdd(this.gameObject, iTween.Hash(
				"amount", Vector3.up * jumpHeight, //Vector3.up
				"time", jumpTime,
				"easetype", iTween.EaseType.easeOutQuad,
				"oncomplete", "Fall",
				"oncompletetarget", this.gameObject
			));
		}
	}
	
	void Fall() {
		iTween.MoveAdd(this.gameObject, iTween.Hash(
			"amount", Vector3.down * jumpHeight,
			"time", jumpTime,
			"easetype", iTween.EaseType.easeInQuad,
			"oncomplete", "FallComplete",
			"oncompletetarget", this.gameObject
		));
	}
	
	void FallComplete(){
		endedJump = true;
		endJumpTime = Time.fixedTime;
	}
	
	UserMovement NewOrGet(int id){
		if(!users.ContainsKey(id)) {
			users.Add(id, new UserMovement());
		}
		return users[id];
	}
	
	Object PutOnPath(Transform obj, float pathPercentage, Vector3 offset) {
		pathPercentage = Mathf.Min( Mathf.Max(0.0f, pathPercentage), 0.999f);
		Quaternion rotation = new Quaternion();
		rotation.SetLookRotation(CRSpline.InterpConstantSpeed(thePath.ToArray(), pathPercentage) - CRSpline.InterpConstantSpeed(thePath.ToArray(), pathPercentage * 0.99f));
		return Instantiate(obj, CRSpline.InterpConstantSpeed(thePath.ToArray(), pathPercentage) + offset, rotation);
	}
	
	void SetOnPath(Transform obj, float pathPercentage, Vector3 offset) {
		pathPercentage = Mathf.Min( Mathf.Max(0.0f, pathPercentage), 0.999f);
		Quaternion rotation = new Quaternion();
		rotation.SetLookRotation(CRSpline.InterpConstantSpeed(thePath.ToArray(), pathPercentage) - CRSpline.InterpConstantSpeed(thePath.ToArray(), pathPercentage * 0.99f));
		obj.position = CRSpline.InterpConstantSpeed(thePath.ToArray(), pathPercentage) + offset;
		obj.rotation = rotation;
	}
	
	Object PutOnPathCache(Transform obj, int index, Vector3 offset, List<Vector3> positions, List<Vector3> angles) {
		Quaternion rotation = new Quaternion();
		rotation.SetLookRotation(angles.ElementAt(index));
		return Instantiate(obj, positions.ElementAt(index) + offset, rotation);
	}
	
	void SetOnPathCache(Transform obj, int index, Vector3 offset, List<Vector3> positions, List<Vector3> angles) {
		Quaternion rotation = new Quaternion();
		rotation.SetLookRotation(angles.ElementAt(index));
		obj.position = positions.ElementAt(index) + offset;
		obj.rotation = rotation;
	}
	
	public void RemoveUser(int id) {
		users.Remove(id);
	}
}