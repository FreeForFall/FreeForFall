using System;

namespace AssemblyCSharp
{
	public class EndGameEvent : NetworkEvent
	{
		public EndGameEvent () : base (0x99)
		{
		}
	}
}

