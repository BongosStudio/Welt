#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Cameras;
using Welt.Controllers;
using Welt.Forge;
using Welt.Forge.Renderers;
using Welt.Models;
using EmptyKeys.UserInterface.Mvvm;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using System.Collections.Generic;
using Welt.Types;

namespace Welt.Scenes
{
    public class LoadScene : Scene
    {
        private static readonly string[] m_Texts =
        {
            "Creating world...",
            "Building terrain...",
            "Simulating world for a bit...",
            "Preparing level..."
        };

        private static int m_TextStep;
        private readonly World m_World;
        private WorldRenderer m_WorldRenderer;
        private SkyRenderer m_SkyRenderer;
        private PlayerRenderer m_PlayerRenderer;
        private Player m_Player;

        internal override ViewModelBase DataContext { get; set; }
        internal override UIRoot UI { get; set; } = new Loading();
        internal override Color BackColor => new Color(0.15f, 0.15f, 0.15f);

        public LoadScene(WeltGame game, World worldToLoad) : base(game)
        {
            m_World = worldToLoad;
            Player.CreatePlayer("test", "token");
            Player.Current.AssignWorld(m_World);
            m_Player = Player.Current;
            m_PlayerRenderer = new PlayerRenderer(game.GraphicsDevice, m_Player);
            m_PlayerRenderer = new PlayerRenderer(game.GraphicsDevice, m_Player);
            m_WorldRenderer = new WorldRenderer(game.GraphicsDevice, m_PlayerRenderer.Camera, m_World);
            m_SkyRenderer = new SkyRenderer(game.GraphicsDevice, m_PlayerRenderer.Camera, m_World);
            
            
        }

        public override void Initialize()
        {
            new Thread(() =>
            {
                
                m_WorldRenderer.Initialize();
                m_SkyRenderer.Initialize();
                m_PlayerRenderer.Initialize();
                
                SceneController.Load(new PlayScene(Game, m_World, m_WorldRenderer, m_SkyRenderer, m_PlayerRenderer));
            })
            {IsBackground = true}.Start();

        }

        public override void Update(GameTime time)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            
        }

        protected override void LoadContent()
        {
            m_WorldRenderer.LoadContent(WeltGame.Instance.Content);
            m_SkyRenderer.LoadContent(WeltGame.Instance.Content);
            m_PlayerRenderer.LoadContent(WeltGame.Instance.Content);
        }

        public void Handoff(out World world, out IRenderer renderer, out SkyRenderer sky,
            out PlayerRenderer playerRenderer, out Player player)
        {
            world = m_World;
            renderer = m_WorldRenderer;
            sky = m_SkyRenderer;
            playerRenderer = m_PlayerRenderer;
            player = Player.Current;
        }
    }
}