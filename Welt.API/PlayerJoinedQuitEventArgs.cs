using System;
using Welt.API.Net;

namespace Welt.API
{
    public class PlayerJoinedQuitEventArgs : EventArgs
    {
        public IRemoteClient Client { get; set; }

        public PlayerJoinedQuitEventArgs(IRemoteClient client)
        {
            Client = client;
        }
    }
}