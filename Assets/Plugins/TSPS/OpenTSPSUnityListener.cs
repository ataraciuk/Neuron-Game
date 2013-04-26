/**
 * OpenTSPS + Unity3d Extension
 * Created by James George on 11/24/2010
 * 
 * This example is distributed under The MIT License
 *
 * Copyright (c) 2010 James George
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TSPS;
using System.Linq;

public class OpenTSPSUnityListener : MonoBehaviour  {
	
	private int jumpThreshold = 10;
	private IDictionary<int,UserSkeleton> Users = new Dictionary<int,UserSkeleton>();
	
	public void OnEnable(){
		UnityOSCReceiver.OSCMessageReceived += new UnityOSCReceiver.OSCMessageReceivedHandler(OSCMessageReceived);
		
	}
	public void OnDisable(){
		UnityOSCReceiver.OSCMessageReceived -= new UnityOSCReceiver.OSCMessageReceivedHandler(OSCMessageReceived);
		
	}
	
	public void Update(){
		foreach(var keypair in Users){
			var user = keypair.Value;
			var id = keypair.Key;
			if(user.l_hip >= 0.0f && user.r_hip >= 0.0f) {
				var diff = (user.l_hip + user.r_hip) / 2;
				if(user.hipCalibrated < 0.0f) {
					user.hipCalibrated = diff;
				}
				BroadcastMessage("HipsMoved", new KeyValuePair<int,float>(id, diff - user.hipCalibrated), SendMessageOptions.DontRequireReceiver);
				user.hipCalibrated = diff;
			}
			if(user.torso >= 0.0f) {
				if(user.jumpCalibrated < 0.0f) {
					user.jumpCalibrated = user.torso;
				}
				BroadcastMessage("TorsoMoved", new KeyValuePair<int,float>(id, user.jumpCalibrated - user.torso), SendMessageOptions.DontRequireReceiver);
				user.jumpCalibrated = user.torso;
			}
			if(user.l_knee >= 0.0f && user.r_knee >= 0.0f && user.neck >= 0.0f) {
				BroadcastMessage("UpdateJumpThreshold", new KeyValuePair<int,float>(id,(user.l_knee + user.r_knee) / 2 - user.neck), SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	public void OSCMessageReceived(OSC.NET.OSCMessage message){
		
		string address = message.Address;
		ArrayList args = message.Values;
		if(address.StartsWith("/joint")) {
			var joint = args[0].ToString();
			if(!Users.ContainsKey((int)args[1])){
				this.Users.Add((int)args[1], new UserSkeleton());
			}
			var user = this.Users[(int)args[1]];
			if(joint == "torso") user.torso = (float)args[3];
			else if(joint == "l_hip") user.l_hip = (float)args[2];
			else if(joint == "r_hip") user.r_hip = (float)args[2];
			else if(joint == "l_knee") user.l_knee = (float)args[3];
			else if(joint == "r_knee") user.r_knee = (float)args[3];
			else if(joint == "neck") user.neck = (float)args[3];
		}
		else if(address.StartsWith("/new_skel")) {
			this.Users.Add((int)args[0], new UserSkeleton());
		}
		else if(address.StartsWith("/lost_user")) {
			this.Users.Remove((int)args[0]);
			BroadcastMessage("RemoveUser", (int)args[0], SendMessageOptions.DontRequireReceiver);
		}
	}
}

public class UserSkeleton {
	public float torsoXBeginning = -1.0f;
	public float torsoYBeginning = -1.0f;
	public float l_hip = -1.0f;
	public float r_hip = -1.0f;
	public float l_knee = -1.0f;
	public float r_knee = -1.0f;
	public float torso = -1.0f;
	public float jumpCalibrated = -1.0f;
	public float hipCalibrated = -1.0f;
	public float neck = -1.0f;
}
