using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class BazookaEvent : NetworkEvent
	{
		public BazookaEvent (Vector3 start, Quaternion angle, Vector3 force) : base (0x51, new object[] {
				start, angle, force
			})
		{
		}
	}
}

