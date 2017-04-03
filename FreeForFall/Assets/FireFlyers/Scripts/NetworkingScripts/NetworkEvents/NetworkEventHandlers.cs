using System;
using Photon;

namespace AssemblyCSharp
{
	public static class NetworkEventHandlers
	{
		public static void SendEvent (NetworkEvent e)
		{
			PhotonNetwork.RaiseEvent (e.NetworkEventCode, e.Content, e.Reliable, null);
		}
	}
}

