#region Copyright

// COPYRIGHT 2015 JUSTIN COX (CONJI)

#endregion Copyright

using Microsoft.Xna.Framework;
using System;
using Welt.Scenes;

namespace Welt.Controllers
{
    public class SceneController
    {
        public static WeltGame Game = WeltGame.Instance;

        public static GraphicsDeviceManager GraphicsManager;
        private static Scene m_Current;

        public SceneController(WeltGame game, GraphicsDeviceManager gdm)
        {
            Game = game;
            GraphicsManager = gdm;
            Scene.Controller = this;
            Game.Exiting += HandleExiting;
        }

        public static void Draw(GameTime gameTime)
        {
            GraphicsManager.GraphicsDevice.Clear(m_Current.BackColor);
            m_Current.I_Draw(gameTime);
            //if (m_Current.WillDrawUi) m_Ui.Draw(gameTime);
        }

        public static void Initialize(GraphicsDeviceManager manager, Scene scene)
        {
            GraphicsManager = manager;
            Load(scene);
            
        }

        public static void Load(Scene scene)
        {
            m_Current?.Dispose();
            Game.Audio?.Destroy();
            scene.I_Initialize();
            Game.Audio?.LoadContent(Game.Content);
            Game.Audio?.BeginWatch();
            m_Current = scene;
            //m_Ui = scene.UI;
            //m_Ui.DataContext = scene.DataContext;
        }

        public static void Update(GameTime gameTime)
        {

            m_Current.I_Update(gameTime);
            //m_Ui.Update(gameTime);
            //_ui.Update(gameTime);
        }

        public static void HandleExiting(object sender, EventArgs args)
        {
            m_Current.OnExiting(sender, args);
        }
    }
}