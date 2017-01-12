#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.IO;
using Lidgren.Network;
using Welt.Core.Net;

namespace Welt.Core.Server
{
    public class GameServer
    {
        /// <summary>
        ///     Gets the configuration applied to the server.
        /// </summary>
        public GameServerConfig Config { get; }
        /// <summary>
        ///     Gets or sets whether or not the server should run on a timed loop or wait for
        ///     update calls from a host.
        /// </summary>
        public bool ShouldWaitForUpdateCalls { get; set; }

        private NetPeer _netServer;

        /// <summary>
        ///     Creates a new instance of GameServer with the specified <see cref="GameServerConfig"/>
        /// </summary>
        /// <param name="config"></param>
        public GameServer(GameServerConfig config)
        {
            Config = config;
        }

        public void Start()
        {
            MessageHandler.Initialize();
            _netServer = new NetServer(new NetPeerConfiguration(Config.Name)
            {
                Port = Config.Port
            });
            _netServer.Start();
        }

        public void Update(double time)
        {
            NetIncomingMessage message;
            while ((message = _netServer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        var id = message.ReadByte();
                        var m = MessageHandler.GetMessage(id);
                        message.WriteAllFields(m);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch (message.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:
                                break;
                            case NetConnectionStatus.Disconnected:
                                break;
                            case NetConnectionStatus.Disconnecting:
                                break;
                            case NetConnectionStatus.InitiatedConnect:
                                break;
                            case NetConnectionStatus.ReceivedInitiation:
                                break;
                            case NetConnectionStatus.RespondedAwaitingApproval:
                                break;
                            case NetConnectionStatus.RespondedConnect:
                                break;
                        }
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        break;
                }
            }
        }
    }
}