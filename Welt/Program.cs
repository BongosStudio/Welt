#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
<<<<<<< HEAD

using System;
using System.IO;

=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
namespace Welt
{
#if WINDOWS || XBOX
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
<<<<<<< HEAD
            if (args.Length < 2) return;
            Logger.Create();
            using (var game = new WeltGame(args[0], args[1]))
=======
            
            using (var game = new WeltGame())
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            {
                game.Run();
            }
        }
    }
#endif
<<<<<<< HEAD
        }
=======
}
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5

