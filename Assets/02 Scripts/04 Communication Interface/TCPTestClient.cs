using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using UnityEngine;

public class TCPTestClient : MonoBehaviour
{
	#region private members 	
	private TcpClient socketConnection;
	private Thread clientReceiveThread;
	#endregion
	public int portNumber = 5000;
	public string serverIP = "192.0.2.1";

	private string _message;
	private bool _connected;

	[HideInInspector] public bool enableSync;
	[HideInInspector] public bool openGripper;
	[HideInInspector] public bool closeGripper;
	[HideInInspector] public string rapidPos;


	private void Awake()
    {
		//needs time to connect
		SetMessage("");
		SetConnectionStatus(false);
		enableSync = false;
		openGripper = false;
		closeGripper = false; ;
	}
   

	public void SetMessage(string message){ _message = message; }
	public void SetConnectionStatus(bool connected){ _connected = connected; }
	public string GetMessage() { return _message; }
	public bool IsConnected() { return _connected; }

    public void ConnectToTcpServer()
	{

			clientReceiveThread = new Thread(new ThreadStart(ListenForData));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
			StartCoroutine(WaitToSendMessage());

	}
	private IEnumerator WaitToSendMessage()
	{

		yield return new WaitUntil(() => IsConnected() == true);
		SendMessage();
		yield break;
	}
	private void DisonnectFromTcpServer()
	{
		//It will mean that the socket is closed, the comunication will be stopped
		SetConnectionStatus(false);
		socketConnection.Close();
		clientReceiveThread.IsBackground = false;
		clientReceiveThread.Abort();
		
	}

    private void ListenForData()
	{
		socketConnection = new TcpClient(serverIP, portNumber);
		SetConnectionStatus(true);

		Byte[] bytes = new Byte[1024];
		while (IsConnected())
		{
			// Get a stream object for reading 				
			using (NetworkStream stream = socketConnection.GetStream())
			{
				int length;
				// Read incomming stream into byte arrary. 					
				while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
				{
					var incommingData = new byte[length];
					Array.Copy(bytes, 0, incommingData, 0, length);
					// Convert byte array to string message. 						
					string serverMessage = Encoding.ASCII.GetString(incommingData);
					//Debug.Log("server message received as: " + serverMessage);
					//Sends back the ack message;
					SetMessage("ack");
					SendMessage();

					switch (serverMessage)
					{
						case "done":
							SetMessage("done");
							DisonnectFromTcpServer();
							break;
						case "Open":
							openGripper = true;
							break;
						case "Close":
							closeGripper = true;
							break;
						case "ack":
							break;
						default:
							rapidPos = serverMessage;
							enableSync = true;
							break;

					}
				}
			}
		}
	}
	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	public void SendMessage()
	{
		// Get a stream object for writing. 			
		NetworkStream stream = socketConnection.GetStream();

		if (stream.CanWrite)
		{
			string message = GetMessage();
			// Convert string message to byte array.                 
			byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message);
			// Write byte array to socketConnection stream.                 
			stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
			//Debug.Log("Sent: " + message);
		}
	}


}

