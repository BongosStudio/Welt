#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Welt
{
    public static class Logger
    {
        private static FileStream m_Stream;

        public static void Create()
        {
            m_Stream = File.OpenWrite("log.txt");
        }

        public static void WriteLine(string input)
        {
            var buffer = Encoding.UTF8.GetBytes(input + "\r\n");
            m_Stream.Write(buffer, 0, buffer.Length);
        }

        public static void Close()
        {
            m_Stream.Dispose();
        }
    }
}
