using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class jsOSCReceiver {}

namespace OSC.NET
{
	/// <summary>
	/// OSCReceiver
	/// </summary>
	public class OSCReceiver
	{
		protected UdpClient udpClient;
		protected int localPort;
		protected int sendPort;

		public OSCReceiver(int localPort, int sendPort)
		{
			this.localPort = localPort;
			this.sendPort = sendPort;
			Connect();
		}

		public void Connect()
		{
			if(this.udpClient != null) Close();
			this.udpClient = new UdpClient( this.localPort);
		}

		public void Close()
		{
			if (this.udpClient!=null) this.udpClient.Close();
			this.udpClient = null;
		}

		public OSCPacket Receive()
		{
            try
            {
                IPEndPoint ip = null;
                byte[] bytes = this.udpClient.Receive(ref ip);
				Debug.Log(ip);
                if (bytes != null && bytes.Length > 0)
                    return OSCPacket.Unpack(bytes);

            } catch (Exception e) { 
                Console.WriteLine(e.Message);
                return null;
            }

			return null;
		}
		public void Send(string msg) {
			byte[] bytes = Encoding.ASCII.GetBytes(msg);
			Debug.Log(sendPort);
			this.udpClient.Send(bytes, bytes.Length, "127.0.0.1", sendPort);
		}
	}
}
