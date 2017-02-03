using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Logging;

namespace Welt.Server
{
    public class DefaultLogProvider : ILogProvider
    {
        private StreamWriter m_PacketFileLog;

        public DefaultLogProvider()
        {
            m_PacketFileLog = File.CreateText("packet_debug.txt");
        }

        public void Log(LogCategory category, string text, params object[] parameters)
        {
            switch (category)
            {
                case LogCategory.Debug:
                    Debug.WriteLine($"[DEBUG][{DateTime.UtcNow.ToShortTimeString()}]: {text}", parameters);
                    break;
                case LogCategory.Notice:
                    Console.WriteLine($"[INFO][{DateTime.UtcNow.ToShortTimeString()}]: {text}", parameters);
                    break;
                case LogCategory.Packets:
                    m_PacketFileLog.WriteLine($"{DateTime.UtcNow.ToLongTimeString()}: {text}", parameters);
                    break;
                case LogCategory.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Debug.WriteLine($"[WARNING][{DateTime.UtcNow.ToShortTimeString()}]: {text}", parameters);
                    Console.ResetColor();
                    break;
                case LogCategory.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Debug.WriteLine($"[ERROR][{DateTime.UtcNow.ToShortTimeString()}]: {text}", parameters);
                    Console.ResetColor();
                    break;
            }
        }
    }
}
