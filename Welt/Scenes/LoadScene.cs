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
using Welt.UI;

namespace Welt.Scenes
{
    public class LoadScene : Scene
    {
        protected override Color BackColor => new Color(0.15f, 0.15f, 0.15f);

        private readonly World _world;
        private readonly IRenderer _renderer;
        private readonly SkyDomeRenderer _skyRenderer;
        private readonly Player _player;
        private readonly PlayerRenderer _playerRenderer;

        private int _loadingPercent;
        private string _loadingText;

        private readonly string[] _loadingTexts =
        {
            "Loading terrain...",
            "Generating terrain...",
            "Building terrain...",
            "Preparing level..."
        };

        private SpriteBatch _sprite;
        private SpriteFont _text;

        public LoadScene(Game game, World worldToLoad) : base(game)
        {
            _world = worldToLoad;

            _player = new Player(worldToLoad); // TODO: load from file or from server
            _playerRenderer = new PlayerRenderer(game.GraphicsDevice, _player);
            _renderer = new SimpleRenderer(game.GraphicsDevice, _playerRenderer.Camera, _world);
            _skyRenderer = new SkyDomeRenderer(game.GraphicsDevice, _playerRenderer.Camera, _world);

            _sprite = new SpriteBatch(game.GraphicsDevice);
            _text = game.Content.Load<SpriteFont>("Fonts/console");
            _loadingText = _loadingTexts[0];
            AddComponent(new TextComponent(_loadingText, "status", game.GraphicsDevice)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Color.White
            });
        }

        public override void Initialize()
        {
            base.Initialize();
            _renderer.LoadStepCompleted += (sender, args) =>
            {
                _loadingPercent++;
                GetComponent("status")
                    .Value.SetPropertyValue(TextComponent.TextProperty, _loadingTexts[_loadingPercent]);
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
            player = _player;
        }
    }
}