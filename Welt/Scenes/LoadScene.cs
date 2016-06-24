#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Mvvm;
using GameUILibrary.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Cameras;
using Welt.Controllers;
using Welt.Forge;
using Welt.Forge.Renderers;
using Welt.Models;

namespace Welt.Scenes
{
    public class LoadScene : Scene
    {
        protected override Color BackColor => new Color(0.15f, 0.15f, 0.15f);
        internal override UIRoot UI => new Loading();
        internal override ViewModelBase DataContext { get; set; }
        private readonly World _world;
        private readonly IRenderer _renderer;
        private readonly SkyDomeRenderer _skyRenderer;
        private readonly PlayerRenderer _playerRenderer;

        private static readonly string[] _texts =
        {
            "Creating world...",
            "Building terrain...",
            "Simulating world for a bit...",
            "Preparing level..."
        };

        private static int _textStep;

        public LoadScene(Game game, World worldToLoad) : base(game)
        {
            _world = worldToLoad;
            Player.Current.AssignWorld(_world);
            _playerRenderer = new PlayerRenderer(game.GraphicsDevice, Player.Current);
            _playerRenderer = new PlayerRenderer(game.GraphicsDevice, Player.Current);
            _renderer = new SimpleRenderer(game.GraphicsDevice, _playerRenderer.Camera, _world);
            _skyRenderer = new SkyDomeRenderer(game.GraphicsDevice, _playerRenderer.Camera, _world);
            var viewModel = new LoadingViewModel {LoadingStatusText = _texts[0]};
            _renderer.LoadStepCompleted += (sender, args) =>
            {
                _textStep++;
                viewModel.LoadingStatusText = _texts[_textStep];
            };
            DataContext = viewModel;
        }

        public override void Initialize()
        {
            base.Initialize();
            _renderer.LoadStepCompleted += (sender, args) =>
            {

            };
            new Thread(() =>
            {
                _renderer.Initialize();
                _skyRenderer.Initialize();
                _playerRenderer.Initialize();
                SceneController.Load(new PlayScene(Game, _world, _renderer, _skyRenderer, _playerRenderer));
            })
            {IsBackground = true}.Start();

        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            _renderer.LoadContent(WeltGame.Instance.Content);
            _skyRenderer.LoadContent(WeltGame.Instance.Content);
            _playerRenderer.LoadContent(WeltGame.Instance.Content);
        }

        public void Handoff(out World world, out IRenderer renderer, out SkyDomeRenderer sky,
            out PlayerRenderer playerRenderer, out Player player)
        {
            world = _world;
            renderer = _renderer;
            sky = _skyRenderer;
            playerRenderer = _playerRenderer;
            player = Player.Current;
        }
    }
}