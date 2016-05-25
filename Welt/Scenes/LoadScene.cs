﻿#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

<<<<<<< HEAD
using System;
=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
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
<<<<<<< HEAD
using Welt.UI.Components;
=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5

namespace Welt.Scenes
{
    public class LoadScene : Scene
    {
        protected override Color BackColor => new Color(0.15f, 0.15f, 0.15f);

        private readonly World _world;
        private readonly IRenderer _renderer;
        private readonly SkyDomeRenderer _skyRenderer;
<<<<<<< HEAD
=======
        private readonly Player _player;
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
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
<<<<<<< HEAD
            Player.Current.AssignWorld(_world);
            _playerRenderer = new PlayerRenderer(game.GraphicsDevice, Player.Current);
=======

            _player = new Player(worldToLoad); // TODO: load from file or from server
            _playerRenderer = new PlayerRenderer(game.GraphicsDevice, _player);
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            _renderer = new SimpleRenderer(game.GraphicsDevice, _playerRenderer.Camera, _world);
            _skyRenderer = new SkyDomeRenderer(game.GraphicsDevice, _playerRenderer.Camera, _world);

            _sprite = new SpriteBatch(game.GraphicsDevice);
            _text = game.Content.Load<SpriteFont>("Fonts/console");
            _loadingText = _loadingTexts[0];
<<<<<<< HEAD
            AddComponent(new TextComponent(_loadingText, "status", GraphicsDevice)
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Foreground = Color.White,
                Margin = new BoundsBox(0, 100, 0, 40)
            });
            AddComponent(new TextComponent(worldToLoad.Name, "worldname", GraphicsDevice)
            {
                Foreground = Color.White,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new BoundsBox(40, 0, 0, 120)
            });
            AddComponent(new TextComponent(Player.Current.Username.ToUpper(), "playername", GraphicsDevice)
            {
                Foreground = Color.White,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new BoundsBox(40, 0, 0, 80)
            });
            AddComponent(new TextComponent("NO ADDITIONAL INFO", "info", GraphicsDevice)
            {
                Foreground = Color.White,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new BoundsBox(40, 0, 0, 40)
=======
            AddComponent(new TextComponent(_loadingText, "status", game.GraphicsDevice)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Color.White
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
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
<<<<<<< HEAD
            player = Player.Current;
=======
            player = _player;
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        }
    }
}