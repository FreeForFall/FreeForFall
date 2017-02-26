using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;

public class NetworkScript : MonoBehaviour {


	private Thread _listener;
	private Thread _parser;
	private Thread _sender;
	private UdpClient _client;
	private UdpClient _server;
	private bool _done;
	private Queue<byte[]> _packets;
	private Queue<AssemblyCSharp.Action> _actions;

	public Queue<AssemblyCSharp.Action> FutureActions;

	// Use this for initialization
	void Start () {
		_client = new UdpClient(7777);
		_server = new UdpClient(5555);
		_done = false;
		_packets = new Queue<byte[]>();
		_actions = new Queue<AssemblyCSharp.Action>();
		FutureActions = new Queue<AssemblyCSharp.Action>();
		_listener = new Thread(new ThreadStart(_receive));
		_listener.IsBackground = true;

		_parser = new Thread(new ThreadStart(_parse));
		_parser.IsBackground = true;

		_sender = new Thread(new ThreadStart(_send));
		_sender.IsBackground = true;

		_parser.Start();
		_listener.Start();
		_sender.Start();
	}

	private void _send(){
		while(!_done){
			if(FutureActions.Count == 0){
				continue;
			}
			while(FutureActions.Count > 0){
				byte[] data = Encoding.UTF8.GetBytes(_createPacket(FutureActions.Dequeue()));
				_server.Send(data, data.Length, "127.0.0.1", 7777);
				//Debug.Log("Sent data");
			}
		}
	}

	private string _createPacket(AssemblyCSharp.Action a){
		if(a is AssemblyCSharp.MovementAction){
			var action = (AssemblyCSharp.MovementAction)a;
			return "0|" + action.playerId + "|" + EncodeData(action.newPosition);
		}
		return "";
	}

	// Can't use generics...
	private string EncodeData(Vector3 vec){
		return $"{vec.x}|{vec.y}|{vec.z}";
	}

	/*
	private string EncodeData<T>(T data){
		if(data is Vector3){
			Vector3 vec = (Vector3)data;
			return $"{vec.x}|{vec.y}|{vec.z}";
		}
		return "";
	}
	*/

	private void _parse(){
		while(!_done){
			if(_packets.Count == 0){
				continue;
			}
			lock(_packets){
				// TODO : Add a limit of packets parsed everytime, because this is blocking the receiving
				while(_packets.Count > 0){
					byte[] packet = _packets.Dequeue();
					string payload = Encoding.UTF8.GetString(packet);
					//Debug.Log(payload);
					_parsePacket(payload);
				}
			}
		}
	}

	private void _parsePacket(string payload){
		string[] split = payload.Split('|');
		switch(split[0]){
		case "0":
			//Debug.Log("Movement");
			int playerId = int.Parse(split[1]);
			float newx = float.Parse(split[2]);
			float newy = float.Parse(split[3]);
			float newz = float.Parse(split[4]);
			lock(_actions){
				_actions.Enqueue(new AssemblyCSharp.MovementAction(playerId, new Vector3(newx, newy, newz)));
				//Debug.Log("Added a new action to the execute queue");
			}
			break;
		default:
			//Debug.Log("No handler");
			break;
		}
	}


	private void _receive(){
		while(!_done){
			var ipE = new IPEndPoint(IPAddress.Any, 0);
			byte[] data = _client.Receive(ref ipE);
			lock(_packets){
				_packets.Enqueue(data);
			}
		}
	}

	void Update(){
		while(_actions.Count > 0){
			//Debug.Log(_actions.Count + " actions left, executing one");
			_actions.Dequeue().Execute();
		}
	}

	void OnApplicationQuit(){
		_done = true;
	}
}
