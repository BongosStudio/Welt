using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Events.Forge;
using Welt.Core.Forge;
using Welt.API;
using System.Collections.Concurrent;

namespace Welt.Components
{
    /// <summary>
    ///     Main component in charge of creating lights (both point and directional) so that
    ///     the chunk renderer can process them during .Draw calls.
    /// </summary>
    public class LightingComponent : ILogicComponent
    {
        private delegate void LightingOperation();

        public GraphicsDevice Graphics { get; private set; }

        public WeltGame Game { get; private set; }

        protected MultiplayerClient Client;
       

        public LightingComponent(WeltGame game, GraphicsDevice graphics, MultiplayerClient client)
        {
            Graphics = graphics;
            Game = game;
            Client = client;
            client.BlockChanged += HandleBlockChanged;
            client.ChunkUnloaded += HandleChunkUnloaded;
        }

        public void Dispose()
        {
            Graphics = null;
            Game = null;
            Client = null;

        }

        public void Initialize()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        private void HandleBlockChanged(object sender, BlockChangedEventArgs args)
        {

        }

        private void HandleChunkUnloaded(object sender, ChunkEventArgs args)
        {
            
        }

        private void RemoveLightAt(Vector3 position)
        {

        }

        private void RemoveLightsIn(Chunk chunk)
        {

        }

        internal class Light
        {
            public Vector3 Position;
            public bool IsPointLight;
            public Vector3? Direction;
            public Vector3 Color;
            public float Intensity;

            public Light(Vector3 position, bool isPointLight, Vector3 color, float intensity, Vector3? direction = null)
            {
                Position = position;
                IsPointLight = isPointLight;
                Color = color;
                Intensity = intensity;
                Direction = direction;
            }
            // TODO: allow update so we can have changing lights
        }
    }
}
