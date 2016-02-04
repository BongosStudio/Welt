#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;
using Welt.Controllers;
using Welt.Scenes;

namespace Welt
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class WeltGame : Game
    {
        #region Fields

        private SceneController _sceneController;
        private readonly GraphicsDeviceManager _graphics;
        
        public static bool ThrowExceptions = true;
        public static WeltGame Instance;

        #endregion

        public WeltGame()
        {          
            Instance = this;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1000,
                PreferredBackBufferHeight = 750
            };

            Content.RootDirectory = "Content";
            _graphics.SynchronizeWithVerticalRetrace = true; // press f3 to set it to false at runtime 
        }

        #region Initialize
        
        protected override void Initialize()
        {
            _sceneController = new SceneController(this, _graphics);
            _sceneController.Initialize(new MainMenuScene(this));
            
            IsMouseVisible = true;
            
            base.Initialize();
        }
        
        #endregion
        
        #region Update
      
        protected override void Update(GameTime gameTime)
        {
            _sceneController.Update(gameTime);
            base.Update(gameTime);
           
        }
       
        #endregion

        #region Draw
        
        protected override void Draw(GameTime gameTime)
        {
            _sceneController.Draw(gameTime);
            base.Draw(gameTime);
        }
        
        #endregion
    }
}
