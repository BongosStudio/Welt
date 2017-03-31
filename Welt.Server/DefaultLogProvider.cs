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
            Console.CursorLeft = 0;
            switch (category)
            {
                case LogCategory.Debug:
#if DEBUG
                    Console.WriteLine($"[DEBUG][{DateTime.Now.ToShortTimeString()}]: {text}", parameters);
#endif
                    break;
                case LogCategory.Notice:
                    Console.WriteLine($"[INFO][{DateTime.Now.ToShortTimeString()}]: {text}", parameters);
                    break;
                case LogCategory.Packets:
                    m_PacketFileLog.WriteLine($"{DateTime.Now.ToLongTimeString()}: {text}", parameters);
                    break;
                case LogCategory.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Debug.WriteLine($"[WARNING][{DateTime.Now.ToShortTimeString()}]: {text}", parameters);
                    Console.ResetColor();
                    break;
                case LogCategory.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Debug.WriteLine($"[ERROR][{DateTime.Now.ToShortTimeString()}]: {text}", parameters);
                    Console.ResetColor();
                    break;
            }
            Console.Write('>');
        }
    }
}
