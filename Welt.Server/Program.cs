using Lidgren.Network;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Welt.API;
using Welt.API.Logging;
using Welt.API.Net;
using Welt.Core.Forge;
using Welt.Core.Net;
using Welt.Core.Net.Packets;
using Welt.Core.Server;
using Welt.Server.Properties;

namespace Welt.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Welt Server";
            if (!File.Exists("access_config.sec"))
            {
                using (var stream = File.Create("access_config.sec"))
                {
                    stream.Write(Resources.access_config, 0, Resources.access_config.Length);
                }
            }
            if (!File.Exists("config.sec"))
            {
                using (var stream = File.Create("config.sec"))
                {
                    stream.Write(Resources.config, 0, Resources.config.Length);
                }
            }
            using (var server = new MultiplayerServer())
            {
                var world = new World("test");
                for (var x = -8; x < 8; x++)
                {
                    for (var z = -8; z < 8; z++)
                    {
                        world.GetChunk(new Vector3I((uint)(x + world.SpawnPoint.X), 0, (uint)(z + world.SpawnPoint.Z)));
                    }
                }
                server.AddLogProvider(new DefaultLogProvider());
                server.RegisterPacketHandler(new ScreenshotResultPacket().Id, HandleScreenshot);
                server.AddWorld(world);
                server.Start(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3456));
                while (true)
                {
                    var input = Console.ReadLine().ToLower();
                    if (input == "quit")
                    {
                        server.Stop();
                        break;
                    }
                    if (input == "ss")
                    {
                        server.QueuePacket(new ScreenshotRequestPacket());
                        Console.WriteLine($"Requested screenshot of {server.Clients.Count} clients");
                    }
                }
            }
        }
        internal static void HandleScreenshot(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            var packet = (ScreenshotResultPacket)_packet;
            File.WriteAllBytes($"ss_{_client.Username}.png", packet.Data);
        }
    }
}
