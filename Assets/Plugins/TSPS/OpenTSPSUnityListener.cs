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
	
	private float torsoXMultiplier = 30.0f;
	private float torsoYMultiplier = 50.0f;
	private float torsoXBeginning = -1.0f;
	private float torsoYBeginning = -1.0f;
	
	public void OnEnable(){
		UnityOSCReceiver.OSCMessageReceived += new UnityOSCReceiver.OSCMessageReceivedHandler(OSCMessageReceived);
		
	}
	public void OnDisable(){
		UnityOSCReceiver.OSCMessageReceived -= new UnityOSCReceiver.OSCMessageReceivedHandler(OSCMessageReceived);
		
	}
	
	public void OSCMessageReceived(OSC.NET.OSCMessage message){
		
		string address = message.Address;
		ArrayList args = message.Values;
		if(address.StartsWith("/joint") && args[0].ToString() == "torso") {
			if (torsoXBeginning == -1.0f) {
				torsoXBeginning = (float)args[2];
			}
			if (torsoYBeginning == -1.0f) {
				torsoYBeginning = (float)args[3];
			}
			BroadcastMessage("TorsoMoved", new Vector3(
				((float)args[2] - torsoXBeginning) * torsoXMultiplier,
				(torsoYBeginning - (float)args[3]) * torsoYMultiplier,
				0), SendMessageOptions.DontRequireReceiver);
			torsoXBeginning = (float)args[2];
			torsoYBeginning = (float)args[3];
		}
		//Debug.Log("mouth width: "+mouthWidth+"    mouth height: "+mouthHeight);
	}
}
