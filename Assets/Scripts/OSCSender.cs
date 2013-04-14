using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class OSCSender : MonoBehaviour {
	
	public int Port = 12346;
	private UdpClient udp;
	
	// Use this for initialization
	void Start () {
		udp = new UdpClient(Port);
	}
	
	// Update is called once per frame
	void Update () {
		Send();
	}
	
	public void Send() {
		byte[] msg = Encoding.ASCII.GetBytes("/torso_trackjointpos 1");
		IPEndPoint ip = null;
		//udp.Send(msg, msg.Length, ip); 
	}
}
