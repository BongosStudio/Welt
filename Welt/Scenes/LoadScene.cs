#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Threading;
using Microsoft.Xna.Framework;
using Welt.Cameras;
using Welt.Controllers;
using Welt.Forge.Renderers;
using Welt.API;
using EmptyKeys.UserInterface.Mvvm;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Welt.Core.Forge;
using Welt.Forge;
using GameUILibrary.Models;
using Welt.Core;
using Welt.Core.Net.Packets;
using Welt.Core.Net;
using Welt.Components;
using Microsoft.Xna.Framework.Content;

namespace Welt.Scenes
{
    public class LoadScene : Scene
    {
        private static readonly string[] m_Texts =
        {
            "Loading world...",
            "Building terrain...",
            "Processing system...",
            "Simulating world a bit...",
            "Spawning in"
        };
        private LoadingViewModel m_ViewModel = new LoadingViewModel();

        internal override ViewModelBase DataContext { get; set; }
        internal override UIRoot UI { get; set; } = new Loading();
        internal override Color BackColor => new Color(0.15f, 0.15f, 0.15f);

        public LoadScene(WeltGame game) : base(game)
        {
            // first, get world data from server
            DataContext = m_ViewModel;
        }

        public override void Initialize()
        {
            new Thread(() =>
            {
                var player = new PlayerRenderer(GraphicsDevice, Game.Client);
                var chunks = new ChunkComponent(Game, GraphicsDevice, player, Game.Client.World);
                var sky = new SkyComponent(Game, player);

                Game.Client.QueuePacket(new LoginRequestPacket(PacketReader.Version, Game.Client.User.Username));
                m_ViewModel.HintText = Constants.Tips[FastMath.NextRandom(Constants.Tips.Length)];
                m_ViewModel.UsernameText = Game.Client.User.Username;

                while (Game.Client.World.World == null) { }

                while (Game.Client.World.GetChunks().Length < 5)
                {
                    m_ViewModel.LoadingStatusText = "Loading world...";
                }

                m_ViewModel.LoadingStatus++;
                m_ViewModel.LoadingStatusText = "Processing system...";

                player.LoadContent(Game.Content);
                chunks.LoadContent(Game.Content);
                sky.LoadContent(Game.Content);

                m_ViewModel.LoadingStatus++;
                m_ViewModel.LoadingStatusText = "Building terrain...";
                chunks.LoadContent(Game.Content);
                chunks.Initialize();
                sky.Initialize();
                player.Initialize();

                m_ViewModel.LoadingStatus++;
                m_ViewModel.LoadingStatusText = "Simulating world...";
                
                m_ViewModel.LoadingStatus++;
                m_ViewModel.LoadingStatusText = "Spawning in.";

                //for (double i = 1; i >= 0; i -= 0.01)
                //{
                //    m_ViewModel.Opacity = i;
                //    Thread.Sleep(25);
                //}
                Next(new PlayScene(Game, chunks, sky, player));
            })
            { IsBackground = true }.Start();

        }

        public override void Update(GameTime time)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            
        }
    }
}