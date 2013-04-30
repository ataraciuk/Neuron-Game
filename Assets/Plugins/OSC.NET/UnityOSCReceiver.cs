using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using OSC.NET;

public class UnityOSCReceiver : MonoBehaviour {
	
	private bool connected = false;
	public int port = 12345;
	public int sendPort = 12346;
	private OSCReceiver receiver;
	private Thread thread;
	private Thread sendThread;
	private int skeletons = 0;
	
	private List<OSCMessage> processQueue = new List<OSCMessage>();		
	public delegate void OSCMessageReceivedHandler(OSCMessage msg);
	public static event OSCMessageReceivedHandler OSCMessageReceived;
	
	
	public UnityOSCReceiver() {}

	public int getPort() {
		return port;
	}
			
	public void Start() {
		connect();
	}
	
	public void OnGUI(){
		if(GUILayout.Button("Reconnect")){
			if(connected){
				disconnect();
				Invoke("connect", .1f); //wait a tick to sync threading before reconnecting
			}
			else{
				connect();
			}
		}
	}
	
	
	public void connect(){
		
		try {
//			print("connecting.");
			connected = true;
			receiver = new OSCReceiver(port, sendPort);
			//Debug.Log(port);
			thread = new Thread(new ThreadStart(listen));
			thread.Start();
			//sendThread = new Thread(new ThreadStart(send));
			//sendThread.Start();
		} catch (Exception e) {
			Debug.Log("failed to connect to port "+port);
			Debug.Log(e.Message);
		}
	}
	/**
	 * Call update every frame in order to dispatch all messages that have come
	 * in on the listener thread
	 */
	public void Update() {
		//processMessages has to be called on the main thread
		//so we used a shared proccessQueue full of OSC Messages
		lock(processQueue){
			foreach( OSCMessage message in processQueue){
				if(OSCMessageReceived != null){
					OSCMessageReceived(message); //uses events/delegates for speed, as opposed to BroadcastMessage. Clients should subscribe to this event.
				}
				//BroadcastMessage("OSCMessageReceived", message, SendMessageOptions.DontRequireReceiver);
			}
			processQueue.Clear();
		}
	}
	
	public void OnApplicationQuit(){
		disconnect();
	}
	
	public void disconnect() {
      	if (receiver!=null){
//		print("disconnecting.");
      		 receiver.Close();
      	}
      	
       	receiver = null;
		connected = false;
	}

	public bool isConnected() { return connected; }

	private void listen() {
		while(connected) {
			try {
				OSCPacket packet = receiver.Receive();
				if (packet!=null) {
					lock(processQueue){
						
						//Debug.Log( "adding  packets " + processQueue.Count );
						if (packet.IsBundle()) {
							ArrayList messages = packet.Values;
							for (int i=0; i<messages.Count; i++) {
								processQueue.Add( (OSCMessage)messages[i] );
							}
						} else{
							processQueue.Add( (OSCMessage)packet );
						}
					}
				} else Console.WriteLine("null packet");
			} catch (Exception e) { 
				Debug.Log( e.Message );
				Console.WriteLine(e.Message); 
			}
		}
	}
	
	private void send() {
		while(connected) {
			if(skeletons > 0) {
				try {
					receiver.Send("/torso_trackjointpos 1");
					Debug.Log ("sent, sleeping...");
					Thread.Sleep(1000);
				} catch (Exception e) { 
					Debug.Log( e.Message );
					Console.WriteLine(e.Message); 
				}
			}
		}
	}
	
	public void SkeletonDetected(){
		skeletons++;
	}
	
	public void SkeletonLeft(){
		skeletons--;
	}
}
