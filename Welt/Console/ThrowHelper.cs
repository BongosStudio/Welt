using System;

namespace Welt.Console
{
    public static class ThrowHelper
    {
        public static void Throw<TException>(object ex) where TException : Exception
        {
            Throw((TException) ex);
        }

        public static void Throw<TException>(object ex, ThrowType type) where TException : Exception
        {
            Throw((TException) ex, type);
        }

        public static void Throw(Exception ex)
        {
            Throw(ex, ThrowType.Info);
        }

        public static void Throw(Exception ex, ThrowType type)
        {
            // TODO: determine the console
            System.Console.WriteLine($"[{DateTime.Now.ToShortTimeString()} | {type}] - {ex.Message}");
        }
    }
}