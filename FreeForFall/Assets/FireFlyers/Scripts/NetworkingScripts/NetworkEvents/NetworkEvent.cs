using System;

namespace AssemblyCSharp
{
	public abstract class NetworkEvent
	{
		protected byte eventCode;
		protected byte[] content;
		protected bool reliable;

		public byte NetworkEventCode
		{
			get
			{
				return this.eventCode;
			}
		}

		public object Content
		{
			get
			{
				return this.content;
			}
		}

		public bool Reliable
		{
			get
			{
				return reliable;
			}
		}

		public NetworkEvent (byte eventCode, byte[] content = null, bool reliable = true)
		{
			this.eventCode = eventCode;
			this.content = content;
			this.reliable = reliable;
		}

		public NetworkEvent (byte eventCode, byte content, bool reliable = true)
		{
			this.eventCode = eventCode;
			this.content = new byte[]{ content };
			this.reliable = reliable;
		}
	}
}

