using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    public sealed class NetworkWorker : INetworkWorker
    {
        private IPacketReader m_PacketReader;
        private NetPeerConfiguration m_Config;
        private NetPeer m_NetPeer;
        private bool m_IsServer;

        public static NetworkWorker CreateClient(NetPeerConfiguration config, IPacketReader packetReader)
        {
            var networker = new NetworkWorker(config, false, packetReader);
            return networker;
        }

        public static NetworkWorker CreateServer(NetPeerConfiguration config, IPacketReader packetReader)
        {
            var networker = new NetworkWorker(config, true, packetReader);
            return networker;
        }

        private NetworkWorker(NetPeerConfiguration config, bool isServer, IPacketReader packetReader)
        {
            m_IsServer = isServer;
            m_PacketReader = packetReader;
            m_Config = config;
            m_Config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            m_Config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            m_Config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            m_Config.EnableMessageType(NetIncomingMessageType.Error);
            m_Config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            m_Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            if (isServer)
                m_NetPeer = new NetServer(m_Config);
            else
                m_NetPeer = new NetClient(m_Config);
        }

        public void Connect()
        {
            if (m_NetPeer is NetClient)
                throw new NetException("Connect() cannot be called on a client");
            m_NetPeer.Start();
        }

        public void Connect(IPEndPoint endpoint)
        {
            if (m_NetPeer is NetServer)
                throw new NetException("Connect(IPEndPoint) cannot be called on a server");
            m_NetPeer.Start();
            m_NetPeer.Connect(endpoint);
        }

        public NetOutgoingMessage CreatePacket(IPacket packet)
        {
            var message = m_NetPeer.CreateMessage();
            m_PacketReader.WritePacket(message, packet);
            return message;
        }

        public void Dispose()
        {
            
        }

        public IPacket ReadPacket(NetIncomingMessage message)
        {
            if (message.MessageType != NetIncomingMessageType.Data) return null;
            var packet = m_PacketReader.ReadPacket(message, m_IsServer);
            return packet;
        }

        private void ProcessNetworkMessages()
        {
            NetIncomingMessage msg;
            while((msg = m_NetPeer.ReadMessage()) != null)
            {

            }
        }
    }
}
