#region Copyright

// COPYRIGHT 2015 JUSTIN COX (CONJI)

#endregion Copyright

using EmptyKeys.UserInterface.Generated;
using GameUILibrary.Models;
using Microsoft.Xna.Framework;
using System;
using Welt.Extensions;
using Welt.Scenes;

namespace Welt.Controllers
{
    public class SceneController
    {
        public static WeltGame Game = WeltGame.Instance;

        static SceneController()
        {
            Game.Exiting += HandleExiting;
            m_ErrorModel = new ErrorViewModel
            {
                ReturnAction = new Action(() =>
                {
                    m_IsErrorShown = false;
                })
            };
        }

        private static Scene m_Current;
        private static bool m_IsErrorShown;
        private static ErrorViewModel m_ErrorModel;
        private static Error ErrorPage => new Error { DataContext = m_ErrorModel };
        
        public static void ShowError(string message)
        {
            m_IsErrorShown = true;
            m_ErrorModel.Result = message;
        }

        public static void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(m_Current.BackColor);
            if (!m_IsErrorShown) m_Current.I_Draw(gameTime);
            else ErrorPage.Draw(gameTime);
            //if (m_Current.WillDrawUi) m_Ui.Draw(gameTime);
        }

        public static void Initialize(Scene scene)
        {
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

            if (!m_IsErrorShown) m_Current.I_Update(gameTime);
            else ErrorPage.Update(gameTime);
            //m_Ui.Update(gameTime);
            //_ui.Update(gameTime);
        }

        public static void HandleExiting(object sender, EventArgs args)
        {
            m_Current.OnExiting(sender, args);
        }
    }
}