#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Welt
{
    public static class Logger
    {
        private static readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

        private static string _toSend;
        public static void Create()
        {
            // TODO: because I can't think of how to do this shit
        }

        public static void WriteLine(string input)
        {
            _toSend = input;
        }
    }
}