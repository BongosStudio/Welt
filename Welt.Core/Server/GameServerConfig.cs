#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
namespace Welt.Core.Server
{
    /// <summary>
    ///     The configuration model used for starting a <see cref="GameServer"/>.
    /// </summary>
    public struct GameServerConfig
    {
        /// <summary>
        ///     The name of the server that will be communicated to clients.
        /// </summary>
        public string Name;
        /// <summary>
        ///     The port that clients will request to.
        /// </summary>
        public ushort Port;
        /// <summary>
        ///     The local address of the server.
        /// </summary>
        public string Address;
        /// <summary>
        ///     Whether or not players not on a whitelist will be allowed to join.
        /// </summary>
        public bool IsWhitelisted;
        
    }
}