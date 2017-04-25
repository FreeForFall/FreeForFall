using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class SpawnCooldownRefresh : NetworkEvent
	{
		public SpawnCooldownRefresh (Vector3 postion) : base (0x32, postion)
		{

		}
	}

}
