using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UserMovement {
	public List<float> speeds = new List<float>();
	public List<float> velocities = new List<float>();
	public List<float> torsoPos = new List<float>();
	public float jumpThreshold = -1.0f;
				
	public int sampleBufferSize = 80;
	public int jumpLongBufferSize = 5;
	
	private float jumpMult = 0.15f;
	
	public UserMovement(){
		for(int i = 0; i < sampleBufferSize; i++) {
			speeds.Add(0.0f);
			velocities.Add(0.0f);
		}
		for(int i = 0; i < jumpLongBufferSize; i++) {
			torsoPos.Add(0.0f);
		}
	}
	
	public float JumpSum() {
		return this.torsoPos.Sum() - this.torsoPos.Skip(this.sampleBufferSize - this.jumpLongBufferSize).Sum();
	}
	
	public bool HasToJump(){
		return jumpThreshold >= 0.0f && JumpSum() > jumpThreshold * jumpMult;
	}
}
