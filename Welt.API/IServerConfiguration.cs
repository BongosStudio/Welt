using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API
{
    public interface IServerConfiguration
    {
        string Name { get; }
        string Address { get; }
        int Port { get; }
        string MessageOfTheDay { get; }
        bool LoadPlugins { get; }
        int MaxPlayers { get; }
        bool Query { get; }
        int QueryPort { get; }
        bool IsRealtime { get; }
    }
}
