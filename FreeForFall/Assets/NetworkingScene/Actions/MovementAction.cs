using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class MovementAction : Action
	{
		public int playerId;
		public Vector3 newPosition;

		public MovementAction (int playerId, Vector3 newPosition)
		{
			this.playerId = playerId;
			this.newPosition = newPosition;
		}

		public override void Execute(){
			GameObject.Find("OtherPlayer").transform.position = newPosition;
		}
	}
}

