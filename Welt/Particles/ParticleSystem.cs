using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Welt.Annotations;
using Welt.Cameras;

namespace Welt.Particles
{
    /// <summary>
	/// The main component in charge of displaying particles.
	/// </summary>
	public class ParticleSystem
    {
        #region Fields

        public WeltGame Game;
        public GraphicsDevice GraphicsDevice;

        string m_SettingsName;
        ParticleSettings m_Settings;
        ContentManager m_Content;
        Effect m_ParticleEffect;
        Camera m_Camera;

        EffectParameter m_EffectViewParameter;
        EffectParameter m_EffectProjectionParameter;
        EffectParameter m_EffectViewportScaleParameter;
        EffectParameter m_EffectTimeParameter;
        
        ParticleVertex[] m_Particles;
        
        DynamicVertexBuffer m_VertexBuffer;
        
        IndexBuffer m_IndexBuffer;

        int m_FirstActiveParticle;
        int m_FirstNewParticle;
        int m_FirstFreeParticle;
        int m_FirstRetiredParticle;

        float m_CurrentTime;
        
        int m_DrawCounter;
        
        static Random m_Random = new Random();

        #endregion

        #region Initialization

        public ParticleSystem(WeltGame game, ContentManager content, Camera camera, [NotNull] ParticleSettings settings)
        {
            Game = game;
            GraphicsDevice = game.GraphicsDevice;
            m_Content = content;
            m_Settings = settings;
            m_Camera = camera;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParticleSystem(WeltGame game, ContentManager content, Camera camera, [NotNull] string settingsName)
        {
            Game = game;
            GraphicsDevice = game.GraphicsDevice;
            this.m_Content = content;
            this.m_SettingsName = settingsName;
            m_Camera = camera;
        }

        /// <summary>
        /// Loads graphics for the particle system.
        /// </summary>
        public void LoadContent()
        {
            if (m_SettingsName != null)
            {
                if (m_Settings == null)
                    m_Settings = m_Content.Load<ParticleSettings>(m_SettingsName);  
            }
            else
            {
                if (m_Settings == null)
                    throw new MissingMemberException("Settings object not passed in constructor");
            }

            // Allocate the particle array, and fill in the corner fields (which never change).
            m_Particles = new ParticleVertex[m_Settings.MaxParticles * 4];

            for (int i = 0; i < m_Settings.MaxParticles; i++)
            {
                m_Particles[i * 4 + 0].Corner = new Short2(-1, -1);
                m_Particles[i * 4 + 1].Corner = new Short2(1, -1);
                m_Particles[i * 4 + 2].Corner = new Short2(1, 1);
                m_Particles[i * 4 + 3].Corner = new Short2(-1, 1);
            }

            LoadParticleEffect();

            // Create a dynamic vertex buffer.
            m_VertexBuffer = new DynamicVertexBuffer(GraphicsDevice, ParticleVertex.VertexDeclaration,
                        m_Settings.MaxParticles * 4, BufferUsage.WriteOnly);

            // Create and populate the index buffer.
            ushort[] indices = new ushort[m_Settings.MaxParticles * 6];

            for (int i = 0; i < m_Settings.MaxParticles; i++)
            {
                indices[i * 6 + 0] = (ushort)(i * 4 + 0);
                indices[i * 6 + 1] = (ushort)(i * 4 + 1);
                indices[i * 6 + 2] = (ushort)(i * 4 + 2);

                indices[i * 6 + 3] = (ushort)(i * 4 + 0);
                indices[i * 6 + 4] = (ushort)(i * 4 + 2);
                indices[i * 6 + 5] = (ushort)(i * 4 + 3);
            }

            m_IndexBuffer = new IndexBuffer(GraphicsDevice, typeof(ushort), indices.Length, BufferUsage.WriteOnly);

            m_IndexBuffer.SetData(indices);
        }


        /// <summary>
        /// Helper for loading and initializing the particle effect.
        /// </summary>
        void LoadParticleEffect()
        {
            Effect effect = m_Content.Load<Effect>("Effects\\ParticleEffect");

            // If we have several particle systems, the content manager will return
            // a single shared effect instance to them all. But we want to preconfigure
            // the effect with parameters that are specific to this particular
            // particle system. By cloning the effect, we prevent one particle system
            // from stomping over the parameter settings of another.

            //particleEffect = effect.Clone ();
            // No cloning for now so we will just create a new effect for now
            m_ParticleEffect = effect;

            EffectParameterCollection parameters = m_ParticleEffect.Parameters;

            // Look up shortcuts for parameters that change every frame.
            m_EffectViewParameter = parameters["View"];
            m_EffectProjectionParameter = parameters["Projection"];
            m_EffectViewportScaleParameter = parameters["ViewportScale"];
            m_EffectTimeParameter = parameters["CurrentTime"];

            // Set the values of parameters that do not change.
            parameters["Duration"].SetValue((float)m_Settings.Duration.TotalSeconds);
            parameters["DurationRandomness"].SetValue(m_Settings.DurationRandomness);
            parameters["Gravity"].SetValue(m_Settings.Gravity);
            parameters["EndVelocity"].SetValue(m_Settings.EndVelocity);
            parameters["MinColor"].SetValue(m_Settings.MinColor.ToVector4());
            parameters["MaxColor"].SetValue(m_Settings.MaxColor.ToVector4());

            parameters["RotateSpeed"].SetValue(
                new Vector2(m_Settings.MinRotateSpeed, m_Settings.MaxRotateSpeed));

            parameters["StartSize"].SetValue(
                new Vector2(m_Settings.MinStartSize, m_Settings.MaxStartSize));

            parameters["EndSize"].SetValue(
                new Vector2(m_Settings.MinEndSize, m_Settings.MaxEndSize));

            // Load the particle texture, and set it onto the effect.
            Texture2D texture = m_Content.Load<Texture2D>(m_Settings.TextureName);

            parameters["Texture"].SetValue(texture);
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the particle system.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (gameTime == null)
                throw new ArgumentNullException("gameTime");

            m_CurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            RetireActiveParticles();
            FreeRetiredParticles();

            // If we let our timer go on increasing for ever, it would eventually
            // run out of floating point precision, at which point the particles
            // would render incorrectly. An easy way to prevent this is to notice
            // that the time value doesn't matter when no particles are being drawn,
            // so we can reset it back to zero any time the active queue is empty.

            if (m_FirstActiveParticle == m_FirstFreeParticle)
                m_CurrentTime = 0;

            if (m_FirstRetiredParticle == m_FirstActiveParticle)
                m_DrawCounter = 0;
        }


        /// <summary>
        /// Helper for checking when active particles have reached the end of
        /// their life. It moves old particles from the active area of the queue
        /// to the retired section.
        /// </summary>
        void RetireActiveParticles()
        {
            float particleDuration = (float)m_Settings.Duration.TotalSeconds;

            while (m_FirstActiveParticle != m_FirstNewParticle)
            {
                // Is this particle old enough to retire?
                // We multiply the active particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                float particleAge = m_CurrentTime - m_Particles[m_FirstActiveParticle * 4].Time;

                if (particleAge < particleDuration)
                    break;

                // Remember the time at which we retired this particle.
                m_Particles[m_FirstActiveParticle * 4].Time = m_DrawCounter;

                // Move the particle from the active to the retired queue.
                m_FirstActiveParticle++;

                if (m_FirstActiveParticle >= m_Settings.MaxParticles)
                    m_FirstActiveParticle = 0;
            }
        }


        /// <summary>
        /// Helper for checking when retired particles have been kept around long
        /// enough that we can be sure the GPU is no longer using them. It moves
        /// old particles from the retired area of the queue to the free section.
        /// </summary>
        void FreeRetiredParticles()
        {
            while (m_FirstRetiredParticle != m_FirstActiveParticle)
            {
                // Has this particle been unused long enough that
                // the GPU is sure to be finished with it?
                // We multiply the retired particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                int age = m_DrawCounter - (int)m_Particles[m_FirstRetiredParticle * 4].Time;

                // The GPU is never supposed to get more than 2 frames behind the CPU.
                // We add 1 to that, just to be safe in case of buggy drivers that
                // might bend the rules and let the GPU get further behind.
                if (age < 3)
                    break;

                // Move the particle from the retired to the free queue.
                m_FirstRetiredParticle++;

                if (m_FirstRetiredParticle >= m_Settings.MaxParticles)
                    m_FirstRetiredParticle = 0;
            }
        }


        /// <summary>
        /// Draws the particle system.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            GraphicsDevice device = GraphicsDevice;

            // Restore the vertex buffer contents if the graphics device was lost.
            if (m_VertexBuffer.IsContentLost)
            {
                m_VertexBuffer.SetData(m_Particles);
            }

            // If there are any particles waiting in the newly added queue,
            // we'd better upload them to the GPU ready for drawing.
            if (m_FirstNewParticle != m_FirstFreeParticle)
            {
                AddNewParticlesToVertexBuffer();
            }

            // If there are any active particles, draw them now!
            if (m_FirstActiveParticle != m_FirstFreeParticle)
            {
                SetCamera(m_Camera.View, m_Camera.Projection);
                device.BlendState = m_Settings.BlendState;
                device.DepthStencilState = DepthStencilState.DepthRead;
                
                m_EffectViewportScaleParameter.SetValue(new Vector2(0.5f / device.Viewport.AspectRatio, -0.5f));
                m_EffectTimeParameter.SetValue(m_CurrentTime);
                
                device.SetVertexBuffer(m_VertexBuffer);
                device.Indices = m_IndexBuffer;

                // Activate the particle effect.
                foreach (EffectPass pass in m_ParticleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    if (m_FirstActiveParticle < m_FirstFreeParticle)
                    {
                        // If the active particles are all in one consecutive range,
                        // we can draw them all in a single call.
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 
                            m_FirstActiveParticle * 6, (m_FirstFreeParticle - m_FirstActiveParticle) * 2);
                    }
                    else
                    {
                        // If the active particle range wraps past the end of the queue
                        // back to the start, we must split them over two draw calls.
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                        m_FirstActiveParticle * 6, (m_Settings.MaxParticles - m_FirstActiveParticle) * 2);

                        if (m_FirstFreeParticle > 0)
                        {
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                            0, m_FirstFreeParticle * 2);
                        }
                    }
                }

                // Reset some of the renderstates that we changed,
                // so as not to mess up any other subsequent drawing.
                device.DepthStencilState = DepthStencilState.Default;
            }

            m_DrawCounter++;
        }


        /// <summary>
        /// Helper for uploading new particles from our managed
        /// array to the GPU vertex buffer.
        /// </summary>
        void AddNewParticlesToVertexBuffer()
        {
            int stride = ParticleVertex.SizeInBytes;

            if (m_FirstNewParticle < m_FirstFreeParticle)
            {
                // If the new particles are all in one consecutive range,
                // we can upload them all in a single call.
                //				vertexBuffer.SetData (firstNewParticle * stride * 4, particles, 
                //					firstNewParticle * 4, 
                //					(firstFreeParticle - firstNewParticle) * 4, 
                //					stride, SetDataOptions.NoOverwrite);			} else {
                // If the new particle range wraps past the end of the queue
                // back to the start, we must split them over two upload calls.
                //				vertexBuffer.SetData (firstNewParticle * stride * 4, particles, 
                //					firstNewParticle * 4, 
                //					(settings.MaxParticles - firstNewParticle) * 4, 
                //					stride, SetDataOptions.NoOverwrite);

                if (m_FirstFreeParticle > 0)
                {
                    m_VertexBuffer.SetData(0, m_Particles,
                        0, m_FirstFreeParticle * 4,
                        stride, SetDataOptions.NoOverwrite);
                }
            }

            // Move the particles we just uploaded from the new to the active queue.
            m_FirstNewParticle = m_FirstFreeParticle;
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Sets the camera view and projection matrices
        /// that will be used to draw this particle system.
        /// </summary>
        public void SetCamera(Matrix view, Matrix projection)
        {
            m_EffectViewParameter.SetValue(view);
            m_EffectProjectionParameter.SetValue(projection);
        }


        /// <summary>
        /// Adds a new particle to the system.
        /// </summary>
        public void AddParticle(Vector3 position, Vector3 velocity)
        {
            // Figure out where in the circular queue to allocate the new particle.
            int nextFreeParticle = m_FirstFreeParticle + 1;

            if (nextFreeParticle >= m_Settings.MaxParticles)
                nextFreeParticle = 0;

            // If there are no free particles, we just have to give up.
            if (nextFreeParticle == m_FirstRetiredParticle)
                return;

            // Adjust the input velocity based on how much
            // this particle system wants to be affected by it.
            velocity *= m_Settings.EmitterVelocitySensitivity;

            // Add in some random amount of horizontal velocity.
            float horizontalVelocity = MathHelper.Lerp(m_Settings.MinHorizontalVelocity,
                            m_Settings.MaxHorizontalVelocity,
                            (float)m_Random.NextDouble());

            double horizontalAngle = m_Random.NextDouble() * MathHelper.TwoPi;

            velocity.X += horizontalVelocity * (float)Math.Cos(horizontalAngle);
            velocity.Z += horizontalVelocity * (float)Math.Sin(horizontalAngle);

            // Add in some random amount of vertical velocity.
            velocity.Y += MathHelper.Lerp(m_Settings.MinVerticalVelocity,
                    m_Settings.MaxVerticalVelocity,
                    (float)m_Random.NextDouble());

            // Choose four random control values. These will be used by the vertex
            // shader to give each particle a different size, rotation, and color.
            Color randomValues = new Color((byte)m_Random.Next(255),
                    (byte)m_Random.Next(255),
                    (byte)m_Random.Next(255),
                    (byte)m_Random.Next(255));

            // Fill in the particle vertex structure.
            for (int i = 0; i < 4; i++)
            {
                m_Particles[m_FirstFreeParticle * 4 + i].Position = position;
                m_Particles[m_FirstFreeParticle * 4 + i].Velocity = velocity;
                m_Particles[m_FirstFreeParticle * 4 + i].Random = randomValues;
                m_Particles[m_FirstFreeParticle * 4 + i].Time = m_CurrentTime;
            }

            m_FirstFreeParticle = nextFreeParticle;
        }


        #endregion
    }

}
