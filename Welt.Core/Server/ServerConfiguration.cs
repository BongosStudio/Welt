using Sec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API;

namespace Welt.Core.Server
{
    public class ServerConfiguration : IServerConfiguration
    {
        private SecFile m_File;

        public ServerConfiguration()
        {
            m_File = SecFile.Open("config");
        }

        public string Address => m_File["server"].Get<SecString>("address");

        public bool LoadPlugins => m_File["server"].Get<SecBool>("loadplugins");

        public int MaxPlayers => m_File["server"].Get<SecInt>("maxplayers");

        public string MessageOfTheDay => m_File["server"].Get<SecString>("motd");

        public string Name => m_File["server"].Get<SecString>("name");

        public int Port => m_File["server"].Get<SecInt>("port");

        public bool Query => m_File["server"].Get<SecBool>("query");
        public int QueryPort => m_File["server"].Get<SecInt>("queryport");
    }
}
