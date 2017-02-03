using Sec;
using System;
using System.Collections.Generic;
using System.IO;
using Welt.API;
using Welt.Core.Properties;

namespace Welt.Core.Server
{
    public class AccessConfiguration : IAccessConfiguration
    {
        private SecFile m_ConfigFile;

        public AccessConfiguration() : this("access_config")
        {

        }

        public AccessConfiguration(string name)
        {
            m_ConfigFile = SecFile.Open(name);
        }

        public IList<string> Blacklist => m_ConfigFile["blacklist"].GetStrings();

        public IList<string> Oplist => m_ConfigFile["oplist"].GetStrings();

        public IList<string> Whitelist => m_ConfigFile["whitelist"].GetStrings();
    }
}