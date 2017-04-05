using System;

namespace AssemblyCSharp
{
	public class PlayerLostEvent : NetworkEvent
	{
		public PlayerLostEvent () : base (0x19)
		{
		}
	}
}

