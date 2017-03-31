using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Events
{
    public class ServerDiscoveredEventArgs : EventArgs
    {
        public string ServerName;
        public IPEndPoint EndPoint;
        public string MessageOfTheDay;
        public int CurrentPlayers;
        public int MaxPlayers;

        public ServerDiscoveredEventArgs(string serverName, IPEndPoint endPoint, string motd, int current, int max)
        {
            ServerName = serverName;
            EndPoint = endPoint;
            MessageOfTheDay = motd;
            CurrentPlayers = current;
            MaxPlayers = max;
        }
    }
}
