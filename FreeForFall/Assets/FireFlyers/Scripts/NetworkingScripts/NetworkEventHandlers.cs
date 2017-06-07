using System;
using Photon;
using AssemblyCSharp;
using UnityEngine;

namespace AssemblyCSharp
{
    public static class NetworkEventHandlers
    {
        public static void Broadcast(Constants.EVENT_IDS id, object[] content)
        {
            Debug.Log("SENT " + id.ToString());
            PhotonNetwork.RaiseEvent((byte)id, content, true, RaiseEventOptions.Default);
        }

        public static void Broadcast(Constants.EVENT_IDS id, object content = null)
        {
            Broadcast(id, new object[] { content });
        }
    }
}
