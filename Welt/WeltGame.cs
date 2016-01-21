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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _sceneController = new SceneController(this, _graphics);
            _sceneController.Initialize(new SplashScene(this));
            
            //TODO refactor WorldRenderer needs player position + view frustum 
            IsMouseVisible = true;
            
            base.Initialize();
        }
        
        #endregion
        
        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _sceneController.Update(gameTime);
            base.Update(gameTime);
           
        }
       
        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _sceneController.Draw(gameTime);
            base.Draw(gameTime);
        }
        
        #endregion
    }
}
