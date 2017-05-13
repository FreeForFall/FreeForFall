using System;
using Photon;
using AssemblyCSharp;

namespace AssemblyCSharp
{
	public static class NetworkEventHandlers
	{
		public static void SendEvent (NetworkEvent e)
		{
			PhotonNetwork.RaiseEvent (e.NetworkEventCode, e.Content, e.Reliable, null);
		}

		public static void Broadcast (Constants.EVENT_IDS id, object[] content)
		{
			PhotonNetwork.RaiseEvent ((byte)id, content, true, null);
		}

		public static void Broadcast (Constants.EVENT_IDS id, object content = null)
		{
			Broadcast (id, new object[] { content });
		}
	}
}

