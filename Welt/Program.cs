#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion


namespace Welt
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            if (args.Length < 2) return;
            Logger.Create();
            using (var game = new WeltGame(args[0], args[1]))
            {
                game.Run();
            }
        }
    }
}

