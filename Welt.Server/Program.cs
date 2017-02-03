using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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
                server.AddLogProvider(new DefaultLogProvider());
                server.Start(new IPEndPoint(IPAddress.Any, 3456));
                while (true)
                {
                    Console.Write('>');
                    var input = Console.ReadLine().ToLower();
                    if (input == "quit")
                    {
                        server.Stop();
                        break;
                    }
                }
            }
        }
    }
}
