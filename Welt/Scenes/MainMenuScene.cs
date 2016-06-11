#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Welt.Controllers;
using Welt.Forge;
using Welt.Forge.Renderers;
using Welt.MonoGame.Extended;
using Welt.MonoGame.Extended.Particles;
using Welt.MonoGame.Extended.Particles.Modifiers;
using Welt.MonoGame.Extended.Particles.Modifiers.Containers;
using Welt.MonoGame.Extended.Particles.Modifiers.Interpolators;
using Welt.MonoGame.Extended.Particles.Profiles;
using Welt.MonoGame.Extended.TextureAtlases;
using Welt.UI;
using Welt.UI.Components;
using ButtonState = System.Windows.Forms.ButtonState;
using Keys = System.Windows.Forms.Keys;

namespace Welt.Scenes
{
    public class MainMenuScene : Scene
    {
        protected override Color BackColor => Color.GhostWhite;
        private SpriteBatch _spriteBatch;
        private Camera2D _camera;
        private ParticleEffect _particleEffect;
        private TimeSpan _resetTime = TimeSpan.Zero;


        public MainMenuScene(Game game) : base(game)
        {
            //game.Window.AllowUserResizing = true;
            AddComponent(new ImageComponent("Images/welt", "background", GraphicsDevice)
            {
                Opacity = 0.8f
            });
            
            var button = new ButtonComponent("Singleplayer", "spbutton", 300, 100, GraphicsDevice)
            {
                TextHorizontalAlignment = HorizontalAlignment.Center,
                BorderWidth = new BoundsBox(2, 2, 2, 2),
                BackgroundActiveColor = Color.CadetBlue,
                BackgroundColor = Color.LightSteelBlue,
                ForegroundColor = Color.Black,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            button.MouseLeftDown += (sender, args) =>
            {
                SceneController.Load(new LoadScene(game, new World("DEMO WORLD"))); // TODO: fetch world data
            };

            AddComponent(button);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new Camera2D(GraphicsDevice);
            var particleTexture = Effects.CreateSolidColorTexture(GraphicsDevice, 1, 1, Color.White);
            var region = new TextureRegion2D(particleTexture);
            _particleEffect = new ParticleEffect
            {
                Emitters = new[]
                {
                    new ParticleEmitter(750, TimeSpan.FromSeconds(2.5), Profile.Point())
                    {
                        TextureRegion = region,
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(50, 100),
                            Quantity = 10,
                            Rotation = new Range<float>(-1, 1),
                            Scale = new Range<float>(3.0f, 4.0f)
                        },
                        Modifiers = new IModifier[]
                        {
                            new RotationModifier
                            {
                                RotationRate = -2.1f
                            },
                            new RectangleContainerModifier
                            {
                                Width = WeltGame.Width,
                                Height = WeltGame.Height,
                            },
                            new LinearGravityModifier
                            {
                                Direction = -Vector2.UnitY,
                                Strength = 30f
                            }, 
                            new AgeModifier
                            {
                                Interpolators = new IInterpolator[]
                                {
                                    new ColorInterpolator
                                    {
                                        InitialColor = new HslColor(0.33f, 0.5f, 0.5f),
                                        FinalColor = new HslColor(0.5f, 0.9f, 1.0f)
                                    }
                                }
                            }
                        }
                    },
                }
            };
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var mouseState = Mouse.GetState();
            var p = _camera.ScreenToWorld(mouseState.X, mouseState.Y);
            
            _particleEffect.Update(deltaTime);

            if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                _particleEffect.Trigger(new Vector2(p.X, p.Y));

            if (_resetTime > TimeSpan.FromSeconds(1))
            {
                _particleEffect.Trigger(new Vector2(400, 240));
                _resetTime -= gameTime.ElapsedGameTime;
            }
            else if (_resetTime <= TimeSpan.FromSeconds(1) && _resetTime > TimeSpan.Zero)
            {
                _resetTime -= gameTime.ElapsedGameTime;
            }
            else
            {
                _resetTime = TimeSpan.FromSeconds(1.2);
            }

            //_particleEffect.Trigger(new Vector2(400, 240));
            

            base.Update(gameTime);

        }

        public override void Draw(GameTime time)
        {
            base.Draw(time);
            _spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());
            _spriteBatch.Draw(_particleEffect);
            _spriteBatch.End();
        }
    }
}
