using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class SpawnPowerupEvent : NetworkEvent
	{
		public SpawnPowerupEvent (Vector3 position, int id) : base (0x40, new object[] { position, id })
		{

		}
	}

}
