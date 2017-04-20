using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API.Net
{
    public interface INetworkWorker : IDisposable
    {
        /// <summary>
        ///     Connects the networker to a given port. For server use only.
        /// </summary>
        void Connect();
        /// <summary>
        ///     Connects the networker to a given server endpoint. For client use only.
        /// </summary>
        /// <param name="endpoint"></param>
        void Connect(IPEndPoint endpoint);
        /// <summary>
        ///     Reads and returns the packet pending on the connection, recycling afterwards.
        /// </summary>
        /// <returns></returns>
        IPacket ReadPacket(NetIncomingMessage message);
        /// <summary>
        ///     Creates and returns a packet to be sent on the connection.
        /// </summary>
        /// <returns></returns>
        NetOutgoingMessage CreatePacket(IPacket packet);
    }
}
