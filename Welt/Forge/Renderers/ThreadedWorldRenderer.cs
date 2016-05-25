#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Cameras;
using Welt.Models;
using Welt.Processors;
using Welt.Types;

#endregion

namespace Welt.Forge.Renderers
{
    public class ThreadedWorldRenderer : IRenderer
    {
        public ThreadedWorldRenderer(GraphicsDevice graphicsDevice, FirstPersonCamera camera, World world)
        {
            _mGraphicsDevice = graphicsDevice;
            _mCamera = camera;
            _mWorld = world;
        }

        public void Stop()
        {
            _mRunning = false;
        }

        public event EventHandler LoadStepCompleted;

        #region Initialize

        public void Initialize()
        {
            _mVertexBuildChunkProcessor = new VertexBuildChunkProcessor(_mGraphicsDevice);
            _mLightingChunkProcessor = new LightingChunkProcessor();

            Debug.WriteLine("Generate initial chunks");
            _mWorld.VisitChunks(DoInitialGenerate, GENERATE_RANGE);
            Debug.WriteLine("Light initial chunks");
            _mWorld.VisitChunks(DoLighting, LIGHT_RANGE);
            Debug.WriteLine("Build initial chunks");
            _mWorld.VisitChunks(DoBuild, BUILD_RANGE);

            #region debugFont Rectangle

            _mDebugRectTexture = new Texture2D(_mGraphicsDevice, 1, 1);
            var texcol = new Color[1];
            _mDebugRectTexture.GetData(texcol);
            texcol[0] = Color.Black;
            _mDebugRectTexture.SetData(texcol);

            _mGenQVector2 = new Vector2(580, 0);
            _mLightQVector2 = new Vector2(580, 16);
            _mBuildQVector2 = new Vector2(580, 32);
            _mMemVector2 = new Vector2(580, 48);
            _mBackgroundRectangle = new Rectangle(580, 0, 100, 144);

            #endregion

            #region Thread creation

            _mWorkerQueueThread = new Thread(WorkerThread)
            {
                Priority = ThreadPriority.Highest,
                IsBackground = true
            };
            _mWorkerQueueThread.Start();

            _mWorkerRemoveThread = new Thread(WorkerRemoveThread)
            {
                Priority = ThreadPriority.Highest,
                IsBackground = true
            };
            _mWorkerRemoveThread.Start();

            //_workerGenerateQueueThread = new Thread(new ThreadStart(WorkerGenerateQueueThread));
            //_workerGenerateQueueThread.Priority = ThreadPriority.AboveNormal;
            //_workerGenerateQueueThread.IsBackground = true;
            //_workerGenerateQueueThread.Name = "WorkerGenerateQueueThread";
            //_workerGenerateQueueThread.Start();

            //_workerLightingQueueThread = new Thread(new ThreadStart(WorkerLightingQueueThread));
            //_workerLightingQueueThread.Priority = ThreadPriority.AboveNormal;
            //_workerLightingQueueThread.IsBackground = true;
            //_workerLightingQueueThread.Name = "WorkerLightingQueueThread";
            //_workerLightingQueueThread.Start();

            //_workerBuildQueueThread = new Thread(new ThreadStart(WorkerBuildQueueThread));
            //_workerBuildQueueThread.Priority = ThreadPriority.AboveNormal;
            //_workerBuildQueueThread.IsBackground = true;
            //_workerBuildQueueThread.Name = "WorkerBuildQueueThread";
            //_workerBuildQueueThread.Start();

            _mWorkerCheckThread = new Thread(WorkerCheckThread)
            {
                Priority = ThreadPriority.Highest,
                IsBackground = true,
                Name = "WorkerCheckThread"
            };
            _mWorkerCheckThread.Start();

            #endregion
        }

        #endregion

        public void LoadContent(ContentManager content)
        {
            TextureAtlas = content.Load<Texture2D>("Textures\\blocks_APR28_3");
            SolidBlockEffect = content.Load<Effect>("Effects\\SolidBlockEffect");
            WaterBlockEffect = content.Load<Effect>("Effects\\WaterBlockEffect");

            _mDebugSpriteBatch = new SpriteBatch(_mGraphicsDevice);
            _mDebugFont = content.Load<SpriteFont>("Fonts\\OSDdisplay");
        }

        #region Update

        public void Update(GameTime gameTime)
        {
            ////Debug.WriteLine("M:" + GC.GetTotalMemory(false));

            //uint cameraX = (uint)(_camera.Position.X / Chunk.SIZE.X);
            //uint cameraZ = (uint)(_camera.Position.Z / Chunk.SIZE.Z);

            //Vector3i currentChunkIndex = new Vector3i(cameraX, 0, cameraZ);

            ////if (_previousChunkIndex != currentChunkIndex)
            ////{
            ////_previousChunkIndex = currentChunkIndex;

            //for (uint ix = cameraX - REMOVE_RANGE; ix < cameraX + REMOVE_RANGE; ix++)
            //{
            //    for (uint iz = cameraZ - REMOVE_RANGE; iz < cameraZ + REMOVE_RANGE; iz++)
            //    {
            //        int distX = (int)(ix - cameraX);
            //        int distZ = (int)(iz - cameraZ);
            //        int xdir = 1, zdir = 1;
            //        if (distX < 0)
            //        {
            //            distX = 0 - distX;
            //            xdir = -1;
            //        }
            //        if (distZ < 0)
            //        {
            //            distZ = 0 - distZ;
            //            zdir = -1;
            //        }
            //        Vector3i chunkIndex = new Vector3i(ix, 0, iz);

            //        #region Remove
            //        if (distX > GENERATE_RANGE || distZ > GENERATE_RANGE)
            //        {
            //            if (_world.viewableChunks[ix, iz] != null)
            //            {
            //                Debug.WriteLine("Remove({0},{1}) ChunkCount = {2}", ix, iz, _world.viewableChunks.Count);
            //                _world.viewableChunks.Remove(ix, iz);
            //            }
            //            continue;
            //        }
            //        #endregion
            //        #region Generate
            //        if (distX > LIGHT_RANGE || distZ > LIGHT_RANGE)
            //        {
            //            if (_world.viewableChunks[ix, iz] == null)
            //            {
            //                uint removeX = ix, removeZ = iz;

            //                // find the opposite chunk
            //                if (distX > LIGHT_RANGE) removeX = (uint)(ix - distX * xdir * 2);
            //                if (distZ > LIGHT_RANGE) removeZ = (uint)(iz - distZ * zdir * 2);

            //                // now that we have the opposite chunk, we can check if it is in a Ready state.
            //                // any previously showing chunks, would have that state. If so, we only attempt to regenerate showing chunks
            //                Chunk chunkRemove = _world.viewableChunks[removeX, removeZ];

            //                if (chunkRemove != null)
            //                {
            //                    if (chunkRemove.State == ChunkState.Ready)
            //                    {
            //                        Debug.WriteLine("Remove({0},{1}), Assign ({2},{3}), Dist ({4},{5}), Dir ({6},{7}) ChunkCount = {8}", removeX, removeZ, ix, iz, distX, distZ, xdir, zdir, _world.viewableChunks.Count);

            //                        // remove chunk is in a ready state, so we can remove it
            //                        _world.viewableChunks.Remove(removeX, removeZ);

            //                        // now we can add the front facing chunk to the generate queue, once only.
            //                        Chunk chunkGenerate = new Chunk(_world, chunkIndex);
            //                        chunkGenerate.State = ChunkState.AwaitingGenerate;
            //                        _world.viewableChunks[ix, iz] = chunkGenerate;

            //                        //Debug.WriteLine("chunkGenerate at {0}", chunkIndex);
            //                        QueueGenerate(chunkIndex);

            //                        /*when it works, replace by toReAssign next commented block*/
            //                        /* Chunk toReAssign = _world.viewableChunks[removeX, removeZ];
            //                        if(toReAssign!=null) toReAssign.Assign(chunkIndex);
            //                        toReAssign.State = ChunkState.AwaitingGenerate; 
            //                        */
            //                    }
            //                    else if (chunkRemove.State != ChunkState.AwaitingLighting)
            //                    {
            //                        Debug.WriteLine("chunkGenerate at {0}, state = {1}", chunkIndex, chunkRemove.State);
            //                    }
            //                }
            //                //else if (chunkRemove == null)
            //                //{
            //                //    Debug.WriteLine("NULL Remove found at ({0},{1}), ChunkCount = {2}", removeX, removeZ, _world.viewableChunks.Count);
            //                //}

            //            }
            //            continue;
            //        }
            //        /*
            //        if (distX >= GENERATE_RANGE || distZ >= GENERATE_RANGE)
            //        {
            //            if (_world.viewableChunks[ix, iz] == null)
            //            {
            //                Debug.WriteLine("Add({0},{1})", ix, iz);

            //                Chunk chunk = new Chunk(_world, chunkIndex);
            //                chunk.State = ChunkState.AwaitingGenerate;
            //                _world.viewableChunks[ix, iz] = chunk;
            //                QueueGenerate(chunkIndex);
            //            }
            //            continue;
            //        }*/
            //        #endregion
            //        #region Light
            //        if (distX <= LIGHT_RANGE || distZ <= LIGHT_RANGE)
            //        {
            //            Chunk chunk = _world.viewableChunks[ix, iz];
            //            if (chunk != null && chunk.State == ChunkState.AwaitingLighting)
            //            {
            //                QueueLighting(chunkIndex);
            //            }

            //            if (chunk != null && chunk.State == ChunkState.AwaitingRelighting)
            //            //if (rebuildChunk != null && rebuildChunk.State == ChunkState.AwaitingRelighting)
            //            {
            //                QueueLighting(chunkIndex);
            //            }

            //            if (chunk != null && (chunk.State == ChunkState.AwaitingRebuild || chunk.State == ChunkState.AwaitingBuild))
            //            //if (rebuildChunk != null && (rebuildChunk.State == ChunkState.AwaitingRebuild || rebuildChunk.State == ChunkState.AwaitingBuild))
            //            {
            //                QueueBuild(chunkIndex);
            //            }

            //            continue;
            //        }
            //        #endregion
            //        //#region Rebuild
            //        //Chunk rebuildChunk = _world.viewableChunks[ix, iz];

            //        //if (rebuildChunk.State == ChunkState.AwaitingRelighting)
            //        ////if (rebuildChunk != null && rebuildChunk.State == ChunkState.AwaitingRelighting)
            //        //{
            //        //    QueueLighting(chunkIndex);
            //        //}

            //        //if (rebuildChunk.State == ChunkState.AwaitingRebuild || rebuildChunk.State == ChunkState.AwaitingBuild)
            //        ////if (rebuildChunk != null && (rebuildChunk.State == ChunkState.AwaitingRebuild || rebuildChunk.State == ChunkState.AwaitingBuild))
            //        //{
            //        //    QueueBuild(chunkIndex);
            //        //}
            //        //#endregion
            //    }
            //}
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime)
        {
            DrawSolid(gameTime);
            DrawWater(gameTime);

            #region OSD debug texts

            _mDebugSpriteBatch.Begin();

            if (_mDebugRectangle)
            {
                _mDebugSpriteBatch.Draw(_mDebugRectTexture, _mBackgroundRectangle, Color.Black);
            }
            //long workingSet = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
            _mDebugSpriteBatch.DrawString(_mDebugFont, "GenQ: " + _mGenerateQueue.Count, _mGenQVector2, Color.White);
            _mDebugSpriteBatch.DrawString(_mDebugFont, "LightQ: " + _mLightingQueue.Count, _mLightQVector2, Color.White);
            _mDebugSpriteBatch.DrawString(_mDebugFont, "BuildQ: " + _mBuildQueue.Count, _mBuildQVector2, Color.White);
            //debugSpriteBatch.DrawString(debugFont, (GC.GetTotalMemory(false) / (1024 * 1024)).ToString() + "MB/" + workingSet / (1024 * 1024) + "MB", memVector2, Color.White);
            _mDebugSpriteBatch.End();

            #endregion
        }

        #endregion

        #region DoInitialGenerate

        private Chunk DoInitialGenerate(Vector3I chunkIndex)
        {
            //Debug.WriteLine("DoGenerate " + chunkIndex);
            var chunk = new Chunk(_mWorld, chunkIndex);
            _mWorld.Chunks[chunkIndex.X, chunkIndex.Z] = chunk;
            if (chunk.State == ChunkState.AwaitingGenerate)
            {
                chunk.State = ChunkState.Generating;
                _mWorld.Generator.Generate(chunk);
                chunk.State = ChunkState.AwaitingLighting;
            }
            return chunk;
        }

        #endregion

        public void QueueGenerate(Vector3I chunkIndex)
        {
            lock (_mGenerateQueue)
            {
                _mGenerateQueue.Enqueue(chunkIndex);
            }
        }

        public void QueueLighting(Vector3I chunkIndex)
        {
            if (_mWorld.Chunks[chunkIndex.X, chunkIndex.Z] == null)
            {
                throw new ArgumentNullException("queuing lighting for a null chunk");
            }
            lock (_mLightingQueue)
            {
                _mLightingQueue.Enqueue(chunkIndex);
            }
        }

        public void QueueBuild(Vector3I chunkIndex)
        {
            lock (_mBuildQueue)
            {
                _mBuildQueue.Enqueue(chunkIndex);
            }
        }

        #region WorkerCheckThread

        public void WorkerCheckThread()
        {
            while (_mRunning)
            {
                var cameraX = (uint) (_mCamera.Position.X/Chunk.Size.X);
                var cameraZ = (uint) (_mCamera.Position.Z/Chunk.Size.Z);

                //Vector3i currentChunkIndex = new Vector3i(cameraX, 0, cameraZ); // GC.GetGeneration(0)

                //if (_previousChunkIndex != currentChunkIndex)
                //{
                //    _previousChunkIndex = currentChunkIndex;

                for (var ix = cameraX - REMOVE_RANGE; ix < cameraX + REMOVE_RANGE; ix++)
                {
                    for (var iz = cameraZ - REMOVE_RANGE; iz < cameraZ + REMOVE_RANGE; iz++)
                    {
                        var distX = (int) (ix - cameraX);
                        var distZ = (int) (iz - cameraZ);
                        int xdir = 1, zdir = 1;
                        if (distX < 0)
                        {
                            distX = 0 - distX;
                            xdir = -1;
                        }
                        if (distZ < 0)
                        {
                            distZ = 0 - distZ;
                            zdir = -1;
                        }

                        var chunkIndex = new Vector3I(ix, 0, iz); // GC.GetGeneration(0)

                        //Debug.WriteLine("currentChunkIndex = {0}, chunkIndex = {1}, distX = {2}, distZ = {3}", currentChunkIndex, chunkIndex, distX, distZ);

                        #region Remove

                        if (distX > GENERATE_RANGE || distZ > GENERATE_RANGE)
                        {
                            if (_mWorld.Chunks[ix, iz] != null)
                            {
                                //Debug.WriteLine("Remove({0},{1}) ChunkCount = {2}", ix, iz, _world.viewableChunks.Count);
                                _mWorld.Chunks.Remove(ix, iz);
                            }
                            continue;
                        }

                        #endregion

                        #region Generate

                        if ((distX > LIGHT_RANGE || distZ > LIGHT_RANGE) &&
                            (distX < REMOVE_RANGE || distZ < REMOVE_RANGE))
                        {
                            if (_mWorld.Chunks[ix, iz] == null)
                            {
                                uint removeX = ix, removeZ = iz;

                                if (distX > LIGHT_RANGE) removeX = (uint) (ix - distX*xdir*2);
                                if (distZ > LIGHT_RANGE) removeZ = (uint) (iz - distZ*zdir*2);

                                var toReAssign = _mWorld.Chunks[removeX, removeZ];
                                if (toReAssign != null)
                                {
                                    switch (toReAssign.State)
                                    {
                                        case ChunkState.Ready:
                                            lock (this)
                                            {
                                                var chunkGenerate = new Chunk(_mWorld, chunkIndex);
                                                chunkGenerate.State = ChunkState.AwaitingGenerate;
                                                _mWorld.Chunks[ix, iz] = chunkGenerate;
                                                _mWorld.Chunks.Remove(removeX, removeZ);
                                                //reassign is not ready, make the rest work first
                                                //toReAssign.Assign(chunkIndex);
                                                //toReAssign.State = ChunkState.AwaitingGenerate;

                                                QueueGenerate(chunkIndex);
                                            }
                                            break;
                                        case ChunkState.AwaitingGenerate:
                                            lock (this)
                                            {
                                                QueueGenerate(chunkIndex);
                                            }
                                            break;
                                        case ChunkState.AwaitingLighting:
                                            break;
                                        case ChunkState.AwaitingBuild:
                                            lock (this)
                                            {
                                                DoBuild(toReAssign);
                                            }
                                            break;
                                        case ChunkState.AwaitingRebuild:
                                            lock (this)
                                            {
                                                DoBuild(toReAssign);
                                            }
                                            break;
                                        default:
                                            //Debug.WriteLine("Generate: State = {0}", toReAssign.State);
                                            break;
                                    }
                                }
                                //else
                                //{
                                //    // for some reason we have identified a null chunk, therefore create one temporarily
                                //    lock (this)
                                //    {
                                //        Chunk chunkGenerate = new Chunk(_world, chunkIndex);
                                //        chunkGenerate.State = ChunkState.AwaitingGenerate;
                                //        _world.Chunks[ix, iz] = chunkGenerate;
                                //        QueueGenerate(chunkIndex);
                                //    }
                                //}
                            }
                            continue;
                        }

                        #endregion

                        #region Light

                        if (distX >= LIGHT_RANGE || distZ >= LIGHT_RANGE)
                        {
                            var chunk = _mWorld.Chunks[ix, iz];
                            if (chunk != null && chunk.State == ChunkState.AwaitingLighting)
                            {
                                QueueLighting(chunkIndex);
                            }
                            continue;
                        }

                        #endregion

                        #region Rebuild

                        var rebuildChunk = _mWorld.Chunks[ix, iz];
                        if (rebuildChunk != null)
                        {
                            if (rebuildChunk.State == ChunkState.AwaitingRelighting)
                            {
                                QueueLighting(chunkIndex);
                            }
                            if (rebuildChunk.State == ChunkState.AwaitingRebuild ||
                                rebuildChunk.State == ChunkState.AwaitingBuild)
                            {
                                QueueBuild(chunkIndex);
                            }
                        }

                        #endregion
                    }
                }

                //}
                Thread.Sleep(1);
            }
        }

        #endregion

        #region WorkerThread

        private void WorkerThread()
        {
            bool foundGenerate, foundLighting, foundBuild;
            var target = new Vector3I(0, 0, 0);
            while (_mRunning)
            {
                foundGenerate = false;
                foundLighting = false;
                foundBuild = false;

                #region Generate

                // LOOK FOR CHUNKS REQUIRING GENERATION
                lock (_mGenerateQueue)
                {
                    if (_mGenerateQueue.Count > 0)
                    {
                        target = _mGenerateQueue.Dequeue();
                        foundGenerate = true;
                    }
                }
                if (foundGenerate)
                {
                    try
                    {
                        var chunkGenerate = _mWorld.Chunks[target.X, target.Z];
                        if (chunkGenerate != null && chunkGenerate.State == ChunkState.AwaitingGenerate)
                        {
                            //Debug.WriteLine("DoGenerate target = {0}, state = {1}", target, chunkGenerate.State);
                            DoGenerate(chunkGenerate);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine("NullReferenceException DoGenerate target = {0}", target);
                        if (WeltGame.ThrowExceptions) throw e;
                        DoGenerate(target);
                    }
                    continue;
                }

                #endregion

                #region Light

                // LOOK FOR CHUNKS REQUIRING LIGHTING
                lock (_mLightingQueue)
                {
                    if (_mLightingQueue.Count > 0)
                    {
                        target = _mLightingQueue.Dequeue();
                        foundLighting = true;
                    }
                }
                if (foundLighting)
                {
                    try
                    {
                        var chunkLighting = _mWorld.Chunks[target.X, target.Z];
                        if (chunkLighting.State == ChunkState.AwaitingLighting ||
                            chunkLighting.State == ChunkState.AwaitingRelighting)
                        {
                            //Debug.WriteLine("DoLighting target = {0}, state = {1}", target, chunkLighting.State);
                            DoLighting(chunkLighting);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine("NullReferenceException DoLighting target = {0}", target);
                        if (WeltGame.ThrowExceptions) throw e;

                        DoGenerate(target);
                    }
                    continue;
                }

                #endregion

                #region Build

                // LOOK FOR CHUNKS REQUIRING BUILD
                lock (_mBuildQueue)
                {
                    if (_mBuildQueue.Count > 0)
                    {
                        target = _mBuildQueue.Dequeue();
                        foundBuild = true;
                    }
                }
                if (foundBuild)
                {
                    try
                    {
                        var chunkBuild = _mWorld.Chunks[target.X, target.Z];
                        if (chunkBuild.State == ChunkState.AwaitingBuild ||
                            chunkBuild.State == ChunkState.AwaitingRebuild)
                        {
                            //Debug.WriteLine("DoBuild target = {0}, state = {1}", target, chunkBuild.State);
                            DoBuild(chunkBuild);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine("NullReferenceException DoBuild target = {0}", target);
                        if (WeltGame.ThrowExceptions) throw e;
                        DoGenerate(target);
                    }
                    continue;
                }

                #endregion

                Thread.Sleep(10);
            }
        }

        #endregion

        #region WorkerGenerateQueueThread

        private void WorkerGenerateQueueThread()
        {
            var target = new Vector3I(0, 0, 0);
            bool foundGenerate;

            while (_mRunning)
            {
                foundGenerate = false;

                //if (_generateQueue.Count != 0 || _lightingQueue.Count != 0 || _buildQueue.Count != 0)
                //    Debug.WriteLine("_gQ = {0}, _lQ = {1}, _bQ = {2}", _generateQueue.Count, _lightingQueue.Count, _buildQueue.Count);

                // LOOK FOR CHUNKS REQUIRING GENERATION
                lock (_mGenerateQueue)
                {
                    if (_mGenerateQueue.Count > 0)
                    {
                        target = _mGenerateQueue.Dequeue();
                        foundGenerate = true;
                    }
                }
                if (foundGenerate)
                {
                    try
                    {
                        var chunkGenerate = _mWorld.Chunks[target.X, target.Z];
                        if (chunkGenerate != null && chunkGenerate.State == ChunkState.AwaitingGenerate)
                        {
                            //Debug.WriteLine("DoGenerate target = {0}, state = {1}", target, chunkGenerate.State);
                            DoGenerate(target);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine("NullReferenceException DoGenerate target = {0}", target);
                        if (WeltGame.ThrowExceptions) throw e;
                        DoGenerate(target);
                    }
                    continue;
                }
                Thread.Sleep(1);
            }
        }

        #endregion

        #region WorkerLightingQueueThread

        private void WorkerLightingQueueThread()
        {
            var target = new Vector3I(0, 0, 0);
            bool foundLighting;

            while (_mRunning)
            {
                foundLighting = false;

                //if (_generateQueue.Count != 0 || _lightingQueue.Count != 0 || _buildQueue.Count != 0)
                //    Debug.WriteLine("_gQ = {0}, _lQ = {1}, _bQ = {2}", _generateQueue.Count, _lightingQueue.Count, _buildQueue.Count);

                // LOOK FOR CHUNKS REQUIRING LIGHTING
                lock (_mLightingQueue)
                {
                    if (_mLightingQueue.Count > 0)
                    {
                        target = _mLightingQueue.Dequeue();
                        foundLighting = true;
                    }
                }
                if (foundLighting)
                {
                    try
                    {
                        var chunkLighting = _mWorld.Chunks[target.X, target.Z];
                        if (chunkLighting.State == ChunkState.AwaitingLighting ||
                            chunkLighting.State == ChunkState.AwaitingRelighting)
                        {
                            //Debug.WriteLine("DoLighting target = {0}, state = {1}", target, chunkLighting.State);
                            DoLighting(target);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine("NullReferenceException DoLighting target = {0}", target);
                        if (WeltGame.ThrowExceptions) throw e;
                        DoLighting(target);
                    }
                    continue;
                }
                Thread.Sleep(1);
            }
        }

        #endregion

        #region WorkerBuildQueueThread

        private void WorkerBuildQueueThread()
        {
            var target = new Vector3I(0, 0, 0);
            bool foundBuild;

            while (_mRunning)
            {
                foundBuild = false;

                //if (_generateQueue.Count != 0 || _lightingQueue.Count != 0 || _buildQueue.Count != 0)
                //    Debug.WriteLine("_gQ = {0}, _lQ = {1}, _bQ = {2}", _generateQueue.Count, _lightingQueue.Count, _buildQueue.Count);

                // LOOK FOR CHUNKS REQUIRING BUILD
                lock (_mBuildQueue)
                {
                    if (_mBuildQueue.Count > 0)
                    {
                        target = _mBuildQueue.Dequeue();
                        foundBuild = true;
                    }
                }

                if (foundBuild)
                {
                    try
                    {
                        var chunkBuild = _mWorld.Chunks[target.X, target.Z];
                        if (chunkBuild.State == ChunkState.AwaitingBuild ||
                            chunkBuild.State == ChunkState.AwaitingRebuild)
                        {
                            //Debug.WriteLine("DoBuild target = {0}, state = {1}", target, chunkBuild.State);
                            DoBuild(target);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine("NullReferenceException DoBuild target = {0}", target);
                        if (WeltGame.ThrowExceptions) throw e;
                        DoBuild(target);
                    }
                    continue;
                }
                Thread.Sleep(1);
            }
        }

        #endregion

        #region WorkerRemoveThread

        private void WorkerRemoveThread()
        {
            while (_mRunning)
            {
                var cameraX = (uint) (_mCamera.Position.X/Chunk.Size.X);
                var cameraZ = (uint) (_mCamera.Position.Z/Chunk.Size.Z);

                for (var ix = cameraX - REMOVE_RANGE*4; ix < cameraX + REMOVE_RANGE*4; ix++)
                {
                    for (var iz = cameraZ - REMOVE_RANGE*4; iz < cameraZ + REMOVE_RANGE*4; iz++)
                    {
                        var distX = (int) (ix - cameraX);
                        var distZ = (int) (iz - cameraZ);

                        if (distX < 0)
                        {
                            distX = 0 - distX;
                        }
                        if (distZ < 0)
                        {
                            distZ = 0 - distZ;
                        }

                        var chunkIndex = new Vector3I(ix, 0, iz); // GC.GetGeneration(0)

                        #region Remove

                        if (distX > GENERATE_RANGE || distZ > GENERATE_RANGE)
                        {
                            if (_mWorld.Chunks[ix, iz] != null)
                            {
                                //Debug.WriteLine("Remove({0},{1}) ChunkCount = {2}", ix, iz, _world.Chunks.Count);
                                _mWorld.Chunks.Remove(ix, iz);
                            }
                        }

                        #endregion
                    }
                }
                GC.Collect(9);
                Thread.Sleep(20);
            }
        }

        #endregion

        #region DrawSolid

        private void DrawSolid(GameTime gameTime)
        {
            _mTod = _mWorld.Tod;

            SolidBlockEffect.Parameters["World"].SetValue(Matrix.Identity);
            SolidBlockEffect.Parameters["View"].SetValue(_mCamera.View);
            SolidBlockEffect.Parameters["Projection"].SetValue(_mCamera.Projection);
            SolidBlockEffect.Parameters["CameraPosition"].SetValue(_mCamera.Position);
            SolidBlockEffect.Parameters["FogNear"].SetValue(Fognear);
            SolidBlockEffect.Parameters["FogFar"].SetValue(Fogfar);
            SolidBlockEffect.Parameters["Texture1"].SetValue(TextureAtlas);

            SolidBlockEffect.Parameters["HorizonColor"].SetValue(Horizoncolor);
            SolidBlockEffect.Parameters["NightColor"].SetValue(Nightcolor);

            SolidBlockEffect.Parameters["MorningTint"].SetValue(Morningtint);
            SolidBlockEffect.Parameters["EveningTint"].SetValue(Eveningtint);

            SolidBlockEffect.Parameters["SunColor"].SetValue(Suncolor);
            SolidBlockEffect.Parameters["timeOfDay"].SetValue(_mTod);

            var viewFrustum = new BoundingFrustum(_mCamera.View*_mCamera.Projection);

            _mGraphicsDevice.BlendState = BlendState.Opaque;
            _mGraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in SolidBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunk in _mWorld.Chunks.Values)
                {
                    if (chunk == null) continue;

                    if (chunk.BoundingBox.Intersects(viewFrustum) && chunk.IndexBuffer != null)
                    {
                        lock (chunk)
                        {
                            if (chunk.IndexBuffer.IndexCount > 0)
                            {
                                _mGraphicsDevice.SetVertexBuffer(chunk.VertexBuffer);
                                _mGraphicsDevice.Indices = chunk.IndexBuffer;
                                _mGraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                    chunk.VertexBuffer.VertexCount, 0, chunk.IndexBuffer.IndexCount/3);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Fields

        protected Effect SolidBlockEffect;
        protected Effect WaterBlockEffect;

        protected Texture2D TextureAtlas;
        private VertexBuildChunkProcessor _mVertexBuildChunkProcessor;
        private LightingChunkProcessor _mLightingChunkProcessor;

        #region Thread fields

        private readonly Queue<Vector3I> _mGenerateQueue = new Queue<Vector3I>();
        private readonly Queue<Vector3I> _mBuildQueue = new Queue<Vector3I>();
        private readonly Queue<Vector3I> _mLightingQueue = new Queue<Vector3I>();

        private Thread _mWorkerQueueThread;
        private Thread _mWorkerCheckThread;

        private Thread _mWorkerRemoveThread;

        //private Thread _workerGenerateQueueThread;
        //private Thread _workerLightingQueueThread;
        //private Thread _workerBuildQueueThread;

        private bool _mRunning = true;

        #endregion

        private readonly GraphicsDevice _mGraphicsDevice;
        private readonly FirstPersonCamera _mCamera;
        private readonly World _mWorld;

        private Vector3I _mPreviousChunkIndex;

        #region Atmospheric settings

        protected Vector4 Nightcolor = Color.Black.ToVector4();
        public Vector4 Suncolor = Color.White.ToVector4();
        protected Vector4 Horizoncolor = Color.White.ToVector4();

        protected Vector4 Eveningtint = Color.Red.ToVector4();
        protected Vector4 Morningtint = Color.Gold.ToVector4();

        private float _mTod;
        public bool DayMode = false;
        public bool NightMode = false;
        public const int Fognear = 14*16; //(BUILD_RANGE - 1) * 16;
        public const float Fogfar = 16*16; //(BUILD_RANGE + 1) * 16;

        #endregion

        #region Range fields

        private const byte BUILD_RANGE = 10;
        private const byte LIGHT_RANGE = BUILD_RANGE + 1;
        private const byte GENERATE_RANGE = LIGHT_RANGE + 1;
        private const byte REMOVE_RANGE = GENERATE_RANGE + 1;

        #endregion

        #region debugFont

        private SpriteBatch _mDebugSpriteBatch;
        private SpriteFont _mDebugFont;
        private Texture2D _mDebugRectTexture;
        private readonly bool _mDebugRectangle = true;
        private Vector2 _mGenQVector2;
        private Vector2 _mLightQVector2;
        private Vector2 _mBuildQVector2;
        private Vector2 _mMemVector2;
        private Rectangle _mBackgroundRectangle;

        #endregion

        #endregion

        #region DoGenerate

        //TODO try to avoid using this method in favor of the method taking a chunk in param
        private Chunk DoGenerate(Vector3I target)
        {
            return DoGenerate(_mWorld.Chunks.Get(target));
        }

        private Chunk DoGenerate(Chunk chunk)
        {
            lock (this)
            {
                //Debug.WriteLine("DoGenerate " + chunk);

                if (chunk == null)
                {
                    // Thread sync issue - requeue
                    //QueueGenerate(chunkIndex);
                    return null;
                }
                if (chunk.State == ChunkState.AwaitingGenerate)
                {
                    chunk.State = ChunkState.Generating;
                    _mWorld.Generator.Generate(chunk);
                    chunk.State = ChunkState.AwaitingLighting;
                }
                return chunk;
            }
        }

        #endregion

        #region DoLighting

        //TODO try to avoid using this method in favor of the method taking a chunk in param
        private Chunk DoLighting(Vector3I target)
        {
            return DoLighting(_mWorld.Chunks.Get(target));
        }

        private Chunk DoLighting(Chunk chunk)
        {
            lock (this)
            {
                //Debug.WriteLine("DoLighting " + chunk);

                //TODO chunk happens to be null here sometime : it was not null when enqueued , it became null after
                // => cancel this lighting
                if (chunk == null) return null;

                if (chunk.State == ChunkState.AwaitingLighting)
                {
                    chunk.State = ChunkState.Lighting;
                    _mLightingChunkProcessor.ProcessChunk(chunk);
                    chunk.State = ChunkState.AwaitingBuild;
                }
                else if (chunk.State == ChunkState.AwaitingRelighting)
                {
                    chunk.State = ChunkState.Lighting;
                    _mLightingChunkProcessor.ProcessChunk(chunk);
                    chunk.State = ChunkState.AwaitingBuild;
                    QueueBuild(chunk.Index);
                }
                return chunk;
            }
        }

        #endregion

        #region DoBuild

        //TODO try to avoid using this method in favor of the method taking a chunk in param
        private Chunk DoBuild(Vector3I target)
        {
            return DoBuild(_mWorld.Chunks.Get(target));
        }

        private Chunk DoBuild(Chunk chunk)
        {
            lock (this)
            {
                //Debug.WriteLine("DoBuild " + chunk);              
                if (chunk == null) return null;
                if (chunk.State == ChunkState.AwaitingBuild || chunk.State == ChunkState.AwaitingRebuild)
                {
                    chunk.State = ChunkState.Building;
                    _mVertexBuildChunkProcessor.ProcessChunk(chunk);
                    chunk.State = ChunkState.Ready;
                }
                return chunk;
            }
        }

        #endregion

        #region DrawWater

        private float _mRippleTime;

        private void DrawWater(GameTime gameTime)
        {
            _mRippleTime += 0.1f;

            _mTod = _mWorld.Tod;

            WaterBlockEffect.Parameters["World"].SetValue(Matrix.Identity);
            WaterBlockEffect.Parameters["View"].SetValue(_mCamera.View);
            WaterBlockEffect.Parameters["Projection"].SetValue(_mCamera.Projection);
            WaterBlockEffect.Parameters["CameraPosition"].SetValue(_mCamera.Position);
            WaterBlockEffect.Parameters["FogNear"].SetValue(Fognear);
            WaterBlockEffect.Parameters["FogFar"].SetValue(Fogfar);
            WaterBlockEffect.Parameters["Texture1"].SetValue(TextureAtlas);
            WaterBlockEffect.Parameters["SunColor"].SetValue(Suncolor);

            WaterBlockEffect.Parameters["HorizonColor"].SetValue(Horizoncolor);
            WaterBlockEffect.Parameters["NightColor"].SetValue(Nightcolor);

            WaterBlockEffect.Parameters["MorningTint"].SetValue(Morningtint);
            WaterBlockEffect.Parameters["EveningTint"].SetValue(Eveningtint);

            WaterBlockEffect.Parameters["timeOfDay"].SetValue(_mTod);
            WaterBlockEffect.Parameters["RippleTime"].SetValue(_mRippleTime);

            var viewFrustum = new BoundingFrustum(_mCamera.View*_mCamera.Projection);

            _mGraphicsDevice.BlendState = BlendState.NonPremultiplied;
            _mGraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in WaterBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunk in _mWorld.Chunks.Values)
                {
                    if (chunk == null) continue;

                    if (chunk.BoundingBox.Intersects(viewFrustum) && chunk.WaterVertexBuffer != null)
                    {
                        lock (chunk)
                        {
                            if (chunk.WaterIndexBuffer.IndexCount > 0)
                            {
                                _mGraphicsDevice.SetVertexBuffer(chunk.WaterVertexBuffer);
                                _mGraphicsDevice.Indices = chunk.WaterIndexBuffer;
                                _mGraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                    chunk.WaterVertexBuffer.VertexCount, 0, chunk.WaterIndexBuffer.IndexCount/3);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}