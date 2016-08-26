#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.API.Forge;
using Welt.Core.Forge;
using Welt.Forge;
using Welt.Forge.Builders;
using Welt.Logic.Forge;
using Welt.Rendering;
using Welt.Types;

// ReSharper disable PossibleLossOfFraction

#endregion

namespace Welt.Processors
{
    public class VertexBuildChunkProcessor : IChunkProcessor
    {
        private readonly GraphicsDevice _mGraphicsDevice;
        private const int MAX_SUN_VALUE = 16;

        public VertexBuildChunkProcessor(GraphicsDevice graphicsDevice)
        {
            _mGraphicsDevice = graphicsDevice;
        }

        #region BuildVertexList

        private void BuildVertexList(ChunkBuilder chunk)
        {
            //lowestNoneBlock and highestNoneBlock come from the terrain gen (Eventually, if the terraingen did not set them you gain nothing)
            //and digging is handled correctly too 
            //TODO generalize highest/lowest None to non-solid

            var yLow = (byte) (chunk.LowestNoneBlock.Y == 0 ? 0 : chunk.LowestNoneBlock.Y - 1);
            var yHigh =
                (byte) (chunk.HighestSolidBlock.Y == Chunk.Height ? Chunk.Height : chunk.HighestSolidBlock.Y + 1);

            for (byte x = 0; x < Chunk.Width; x++)
            {
                for (byte z = 0; z < Chunk.Depth; z++)
                {
                    #region ylow and yhigh on ChunkObject borders

                    if (x == 0)
                    {
                        if (chunk.OwnedChunk.E == null)
                        {
                            yHigh = byte.MaxValue/2;
                            yLow = 0;
                        }
                        else
                        {
                            yHigh = Math.Max(yHigh, chunk.OwnedChunk.E.HighestSolidBlock.Y);
                            yLow = Math.Min(yLow, chunk.OwnedChunk.E.LowestNoneBlock.Y);
                        }
                    }
                    else if (x == Chunk.Width - 1)
                    {
                        if (chunk.W == null)
                        {
                            yHigh = ChunkObject.Size.Y;
                            yLow = 0;
                        }
                        else
                        {
                            yHigh = Math.Max(yHigh, chunk.W.HighestSolidBlock.Y);
                            yLow = Math.Min(yLow, chunk.W.LowestNoneBlock.Y);
                        }
                    }

                    if (z == 0)
                    {
                        if (chunk.S == null)
                        {
                            yHigh = ChunkObject.Size.Y;
                            yLow = 0;
                        }
                        else
                        {
                            yHigh = Math.Max(yHigh, chunk.S.HighestSolidBlock.Y);
                            yLow = Math.Min(yLow, chunk.S.LowestNoneBlock.Y);
                        }
                    }
                    else if (z == ChunkObject.Max.Z)
                    {
                        if (chunk.N == null)
                        {
                            yHigh = ChunkObject.Size.Y;
                            yLow = 0;
                        }
                        else
                        {
                            yHigh = Math.Max(yHigh, chunk.N.HighestSolidBlock.Y);
                            yLow = Math.Min(yLow, chunk.N.LowestNoneBlock.Y);
                        }
                    }

                    #endregion

                    // TODO: change these to meshes instead of having to build each time there's a new one
                    for (var y = yLow; y < yHigh; y++)
                    {
                        var b = chunk.GetBlock(x, y, z);
                        if (b.Id == BlockType.None) continue;
                        if (BlockLogic.IsPlantBlock(b.Id))
                        {
                            BuildPlantVertexList(b, chunk, new Vector3I(x, y, z));
                        }
                        else if (BlockLogic.IsGrassBlock(b.Id))
                        {
                            BuildGrassVertexList(b, chunk, new Vector3I(x, y, z));
                        }
                        else
                        {
                            BuildBlockVertexList(b, chunk, new Vector3I(x, y, z));
                        }
                    }
                }
            }

            var v = chunk.VertexList.ToArray();
            var i = chunk.IndexList.ToArray();

            var water = chunk.WaterVertexList.ToArray();
            var iWater = chunk.WaterIndexList.ToArray();

            lock (chunk)
            {
                if (v.Length > 0)
                {
                    chunk.VertexBuffer = new VertexBuffer(_mGraphicsDevice, typeof (VertexPositionTextureLight), v.Length,
                        BufferUsage.WriteOnly);
                    chunk.VertexBuffer.SetData(v);
                    chunk.IndexBuffer = new IndexBuffer(_mGraphicsDevice, IndexElementSize.SixteenBits, i.Length,
                        BufferUsage.WriteOnly);
                    chunk.IndexBuffer.SetData(i);
                }

                if (water.Length > 0)
                {
                    chunk.WaterVertexBuffer = new VertexBuffer(_mGraphicsDevice, typeof (VertexPositionTextureLight),
                        water.Length, BufferUsage.WriteOnly);
                    chunk.WaterVertexBuffer.SetData(water);
                    chunk.WaterIndexBuffer = new IndexBuffer(_mGraphicsDevice, IndexElementSize.SixteenBits,
                        iWater.Length, BufferUsage.WriteOnly);
                    chunk.WaterIndexBuffer.SetData(iWater);
                }
            }

            chunk.IsModified = false;
        }

        #endregion

        #region BuildBlockVertexList

        private void BuildBlockVertexList(Block block, ChunkBuilder chunk, Vector3I chunkRelativePosition)
        {

            var blockPosition = chunk.Position + chunkRelativePosition;

            //get signed bytes from these to be able to remove 1 without further casts
            var x = (sbyte) chunkRelativePosition.X;
            var y = (sbyte) chunkRelativePosition.Y;
            var z = (sbyte) chunkRelativePosition.Z;


            var solidBlock = new Block(BlockType.Rock);

            var blockTopNw = chunk.GetBlock(x - 1, y + 1, z + 1);
            var blockTopN = chunk.GetBlock(x, y + 1, z + 1);
            var blockTopNe = chunk.GetBlock(x + 1, y + 1, z + 1);
            var blockTopW = chunk.GetBlock(x - 1, y + 1, z);
            var blockTopM = chunk.GetBlock(x, y + 1, z);
            var blockTopE = chunk.GetBlock(x + 1, y + 1, z);
            var blockTopSw = chunk.GetBlock(x - 1, y + 1, z - 1);
            var blockTopS = chunk.GetBlock(x, y + 1, z - 1);
            var blockTopSe = chunk.GetBlock(x + 1, y + 1, z - 1);

            var blockMidNw = chunk.GetBlock(x - 1, y, z + 1);
            var blockMidN = chunk.GetBlock(x, y, z + 1);
            var blockMidNe = chunk.GetBlock(x + 1, y, z + 1);
            var blockMidW = chunk.GetBlock(x - 1, y, z);
            var blockMidM = chunk.GetBlock(x, y, z);
            var blockMidE = chunk.GetBlock(x + 1, y, z);
            var blockMidSw = chunk.GetBlock(x - 1, y, z - 1);
            var blockMidS = chunk.GetBlock(x, y, z - 1);
            var blockMidSe = chunk.GetBlock(x + 1, y, z - 1);

            var blockBotNw = chunk.GetBlock(x - 1, y - 1, z + 1);
            var blockBotN = chunk.GetBlock(x, y - 1, z + 1);
            var blockBotNe = chunk.GetBlock(x + 1, y - 1, z + 1);
            var blockBotW = chunk.GetBlock(x - 1, y - 1, z);
            var blockBotM = chunk.GetBlock(x, y - 1, z);
            var blockBotE = chunk.GetBlock(x + 1, y - 1, z);
            var blockBotSw = chunk.GetBlock(x - 1, y - 1, z - 1);
            var blockBotS = chunk.GetBlock(x, y - 1, z - 1);
            var blockBotSe = chunk.GetBlock(x + 1, y - 1, z - 1);

            float sunTr, sunTl, sunBr, sunBl;
            float redTr, redTl, redBr, redBl;
            float grnTr, grnTl, grnBr, grnBl;
            float bluTr, bluTl, bluBr, bluBl;
            Color localTr, localTl, localBr, localBl;


            // XDecreasing
            if (BlockLogic.IsTransparentBlock(blockMidW.Id) && block.Id != blockMidW.Id)
            {
                sunTl = (1f/MAX_SUN_VALUE)*((blockTopNw.Sun + blockTopW.Sun + blockMidNw.Sun + blockMidW.Sun)/4);
                sunTr = (1f/MAX_SUN_VALUE)*((blockTopSw.Sun + blockTopW.Sun + blockMidSw.Sun + blockMidW.Sun)/4);
                sunBl = (1f/MAX_SUN_VALUE)*((blockBotNw.Sun + blockBotW.Sun + blockMidNw.Sun + blockMidW.Sun)/4);
                sunBr = (1f/MAX_SUN_VALUE)*((blockBotSw.Sun + blockBotW.Sun + blockMidSw.Sun + blockMidW.Sun)/4);

                redTl = (1f/MAX_SUN_VALUE)*((blockTopNw.R + blockTopW.R + blockMidNw.R + blockMidW.R)/4);
                redTr = (1f/MAX_SUN_VALUE)*((blockTopSw.R + blockTopW.R + blockMidSw.R + blockMidW.R)/4);
                redBl = (1f/MAX_SUN_VALUE)*((blockBotNw.R + blockBotW.R + blockMidNw.R + blockMidW.R)/4);
                redBr = (1f/MAX_SUN_VALUE)*((blockBotSw.R + blockBotW.R + blockMidSw.R + blockMidW.R)/4);

                grnTl = (1f/MAX_SUN_VALUE)*((blockTopNw.G + blockTopW.G + blockMidNw.G + blockMidW.G)/4);
                grnTr = (1f/MAX_SUN_VALUE)*((blockTopSw.G + blockTopW.G + blockMidSw.G + blockMidW.G)/4);
                grnBl = (1f/MAX_SUN_VALUE)*((blockBotNw.G + blockBotW.G + blockMidNw.G + blockMidW.G)/4);
                grnBr = (1f/MAX_SUN_VALUE)*((blockBotSw.G + blockBotW.G + blockMidSw.G + blockMidW.G)/4);

                bluTl = (1f/MAX_SUN_VALUE)*((blockTopNw.B + blockTopW.B + blockMidNw.B + blockMidW.B)/4);
                bluTr = (1f/MAX_SUN_VALUE)*((blockTopSw.B + blockTopW.B + blockMidSw.B + blockMidW.B)/4);
                bluBl = (1f/MAX_SUN_VALUE)*((blockBotNw.B + blockBotW.B + blockMidNw.B + blockMidW.B)/4);
                bluBr = (1f/MAX_SUN_VALUE)*((blockBotSw.B + blockBotW.B + blockMidSw.B + blockMidW.B)/4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.XDecreasing,
                    block.Id, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr);
            }
            if (BlockLogic.IsTransparentBlock(blockMidE.Id) && block.Id != blockMidE.Id)
            {
                sunTl = (1f/MAX_SUN_VALUE)*((blockTopSe.Sun + blockTopE.Sun + blockMidSe.Sun + blockMidE.Sun)/4);
                sunTr = (1f/MAX_SUN_VALUE)*((blockTopNe.Sun + blockTopE.Sun + blockMidNe.Sun + blockMidE.Sun)/4);
                sunBl = (1f/MAX_SUN_VALUE)*((blockBotSe.Sun + blockBotE.Sun + blockMidSe.Sun + blockMidE.Sun)/4);
                sunBr = (1f/MAX_SUN_VALUE)*((blockBotNe.Sun + blockBotE.Sun + blockMidNe.Sun + blockMidE.Sun)/4);

                redTl = (1f/MAX_SUN_VALUE)*((blockTopSe.R + blockTopE.R + blockMidSe.R + blockMidE.R)/4);
                redTr = (1f/MAX_SUN_VALUE)*((blockTopNe.R + blockTopE.R + blockMidNe.R + blockMidE.R)/4);
                redBl = (1f/MAX_SUN_VALUE)*((blockBotSe.R + blockBotE.R + blockMidSe.R + blockMidE.R)/4);
                redBr = (1f/MAX_SUN_VALUE)*((blockBotNe.R + blockBotE.R + blockMidNe.R + blockMidE.R)/4);

                grnTl = (1f/MAX_SUN_VALUE)*((blockTopSe.G + blockTopE.G + blockMidSe.G + blockMidE.G)/4);
                grnTr = (1f/MAX_SUN_VALUE)*((blockTopNe.G + blockTopE.G + blockMidNe.G + blockMidE.G)/4);
                grnBl = (1f/MAX_SUN_VALUE)*((blockBotSe.G + blockBotE.G + blockMidSe.G + blockMidE.G)/4);
                grnBr = (1f/MAX_SUN_VALUE)*((blockBotNe.G + blockBotE.G + blockMidNe.G + blockMidE.G)/4);

                bluTl = (1f/MAX_SUN_VALUE)*((blockTopSe.B + blockTopE.B + blockMidSe.B + blockMidE.B)/4);
                bluTr = (1f/MAX_SUN_VALUE)*((blockTopNe.B + blockTopE.B + blockMidNe.B + blockMidE.B)/4);
                bluBl = (1f/MAX_SUN_VALUE)*((blockBotSe.B + blockBotE.B + blockMidSe.B + blockMidE.B)/4);
                bluBr = (1f/MAX_SUN_VALUE)*((blockBotNe.B + blockBotE.B + blockMidNe.B + blockMidE.B)/4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.XIncreasing,
                    block.Id, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr);
            }
            if (BlockLogic.IsTransparentBlock(blockBotM.Id) && block.Id != blockBotM.Id)
            {
                sunBl = (1f/MAX_SUN_VALUE)*((blockBotSw.Sun + blockBotS.Sun + blockBotM.Sun + blockTopW.Sun)/4);
                sunBr = (1f/MAX_SUN_VALUE)*((blockBotSe.Sun + blockBotS.Sun + blockBotM.Sun + blockTopE.Sun)/4);
                sunTl = (1f/MAX_SUN_VALUE)*((blockBotNw.Sun + blockBotN.Sun + blockBotM.Sun + blockTopW.Sun)/4);
                sunTr = (1f/MAX_SUN_VALUE)*((blockBotNe.Sun + blockBotN.Sun + blockBotM.Sun + blockTopE.Sun)/4);

                redBl = (1f/MAX_SUN_VALUE)*((blockBotSw.R + blockBotS.R + blockBotM.R + blockTopW.R)/4);
                redBr = (1f/MAX_SUN_VALUE)*((blockBotSe.R + blockBotS.R + blockBotM.R + blockTopE.R)/4);
                redTl = (1f/MAX_SUN_VALUE)*((blockBotNw.R + blockBotN.R + blockBotM.R + blockTopW.R)/4);
                redTr = (1f/MAX_SUN_VALUE)*((blockBotNe.R + blockBotN.R + blockBotM.R + blockTopE.R)/4);

                grnBl = (1f/MAX_SUN_VALUE)*((blockBotSw.G + blockBotS.G + blockBotM.G + blockTopW.G)/4);
                grnBr = (1f/MAX_SUN_VALUE)*((blockBotSe.G + blockBotS.G + blockBotM.G + blockTopE.G)/4);
                grnTl = (1f/MAX_SUN_VALUE)*((blockBotNw.G + blockBotN.G + blockBotM.G + blockTopW.G)/4);
                grnTr = (1f/MAX_SUN_VALUE)*((blockBotNe.G + blockBotN.G + blockBotM.G + blockTopE.G)/4);

                bluBl = (1f/MAX_SUN_VALUE)*((blockBotSw.B + blockBotS.B + blockBotM.B + blockTopW.B)/4);
                bluBr = (1f/MAX_SUN_VALUE)*((blockBotSe.B + blockBotS.B + blockBotM.B + blockTopE.B)/4);
                bluTl = (1f/MAX_SUN_VALUE)*((blockBotNw.B + blockBotN.B + blockBotM.B + blockTopW.B)/4);
                bluTr = (1f/MAX_SUN_VALUE)*((blockBotNe.B + blockBotN.B + blockBotM.B + blockTopE.B)/4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.YDecreasing,
                    block.Id, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr);
            }
            if (BlockLogic.IsTransparentBlock(blockTopM.Id) && block.Id != blockTopM.Id)
            {
                sunTl = (1f/MAX_SUN_VALUE)*((blockTopNw.Sun + blockTopN.Sun + blockTopW.Sun + blockTopM.Sun)/4);
                sunTr = (1f/MAX_SUN_VALUE)*((blockTopNe.Sun + blockTopN.Sun + blockTopE.Sun + blockTopM.Sun)/4);
                sunBl = (1f/MAX_SUN_VALUE)*((blockTopSw.Sun + blockTopS.Sun + blockTopW.Sun + blockTopM.Sun)/4);
                sunBr = (1f/MAX_SUN_VALUE)*((blockTopSe.Sun + blockTopS.Sun + blockTopE.Sun + blockTopM.Sun)/4);

                redTl = (1f/MAX_SUN_VALUE)*((blockTopNw.R + blockTopN.R + blockTopW.R + blockTopM.R)/4);
                redTr = (1f/MAX_SUN_VALUE)*((blockTopNe.R + blockTopN.R + blockTopE.R + blockTopM.R)/4);
                redBl = (1f/MAX_SUN_VALUE)*((blockTopSw.R + blockTopS.R + blockTopW.R + blockTopM.R)/4);
                redBr = (1f/MAX_SUN_VALUE)*((blockTopSe.R + blockTopS.R + blockTopE.R + blockTopM.R)/4);

                grnTl = (1f/MAX_SUN_VALUE)*((blockTopNw.G + blockTopN.G + blockTopW.G + blockTopM.G)/4);
                grnTr = (1f/MAX_SUN_VALUE)*((blockTopNe.G + blockTopN.G + blockTopE.G + blockTopM.G)/4);
                grnBl = (1f/MAX_SUN_VALUE)*((blockTopSw.G + blockTopS.G + blockTopW.G + blockTopM.G)/4);
                grnBr = (1f/MAX_SUN_VALUE)*((blockTopSe.G + blockTopS.G + blockTopE.G + blockTopM.G)/4);

                bluTl = (1f/MAX_SUN_VALUE)*((blockTopNw.B + blockTopN.B + blockTopW.B + blockTopM.B)/4);
                bluTr = (1f/MAX_SUN_VALUE)*((blockTopNe.B + blockTopN.B + blockTopE.B + blockTopM.B)/4);
                bluBl = (1f/MAX_SUN_VALUE)*((blockTopSw.B + blockTopS.B + blockTopW.B + blockTopM.B)/4);
                bluBr = (1f/MAX_SUN_VALUE)*((blockTopSe.B + blockTopS.B + blockTopE.B + blockTopM.B)/4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.YIncreasing,
                    block.Id, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr);
            }
            if (BlockLogic.IsTransparentBlock(blockMidS.Id) && block.Id != blockMidS.Id)
            {
                sunTl = (1f/MAX_SUN_VALUE)*((blockTopSw.Sun + blockTopS.Sun + blockMidSw.Sun + blockMidS.Sun)/4);
                sunTr = (1f/MAX_SUN_VALUE)*((blockTopSe.Sun + blockTopS.Sun + blockMidSe.Sun + blockMidS.Sun)/4);
                sunBl = (1f/MAX_SUN_VALUE)*((blockBotSw.Sun + blockBotS.Sun + blockMidSw.Sun + blockMidS.Sun)/4);
                sunBr = (1f/MAX_SUN_VALUE)*((blockBotSe.Sun + blockBotS.Sun + blockMidSe.Sun + blockMidS.Sun)/4);

                redTl = (1f/MAX_SUN_VALUE)*((blockTopSw.R + blockTopS.R + blockMidSw.R + blockMidS.R)/4);
                redTr = (1f/MAX_SUN_VALUE)*((blockTopSe.R + blockTopS.R + blockMidSe.R + blockMidS.R)/4);
                redBl = (1f/MAX_SUN_VALUE)*((blockBotSw.R + blockBotS.R + blockMidSw.R + blockMidS.R)/4);
                redBr = (1f/MAX_SUN_VALUE)*((blockBotSe.R + blockBotS.R + blockMidSe.R + blockMidS.R)/4);

                grnTl = (1f/MAX_SUN_VALUE)*((blockTopSw.G + blockTopS.G + blockMidSw.G + blockMidS.G)/4);
                grnTr = (1f/MAX_SUN_VALUE)*((blockTopSe.G + blockTopS.G + blockMidSe.G + blockMidS.G)/4);
                grnBl = (1f/MAX_SUN_VALUE)*((blockBotSw.G + blockBotS.G + blockMidSw.G + blockMidS.G)/4);
                grnBr = (1f/MAX_SUN_VALUE)*((blockBotSe.G + blockBotS.G + blockMidSe.G + blockMidS.G)/4);

                bluTl = (1f/MAX_SUN_VALUE)*((blockTopSw.B + blockTopS.B + blockMidSw.B + blockMidS.B)/4);
                bluTr = (1f/MAX_SUN_VALUE)*((blockTopSe.B + blockTopS.B + blockMidSe.B + blockMidS.B)/4);
                bluBl = (1f/MAX_SUN_VALUE)*((blockBotSw.B + blockBotS.B + blockMidSw.B + blockMidS.B)/4);
                bluBr = (1f/MAX_SUN_VALUE)*((blockBotSe.B + blockBotS.B + blockMidSe.B + blockMidS.B)/4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.ZDecreasing,
                    block.Id, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr);
            }
            if (BlockLogic  .IsTransparentBlock(blockMidN.Id) && block.Id != blockMidN.Id)
            {
                sunTl = (1f/MAX_SUN_VALUE)*((blockTopNe.Sun + blockTopN.Sun + blockMidNe.Sun + blockMidN.Sun)/4);
                sunTr = (1f/MAX_SUN_VALUE)*((blockTopNw.Sun + blockTopN.Sun + blockMidNw.Sun + blockMidN.Sun)/4);
                sunBl = (1f/MAX_SUN_VALUE)*((blockBotNe.Sun + blockBotN.Sun + blockMidNe.Sun + blockMidN.Sun)/4);
                sunBr = (1f/MAX_SUN_VALUE)*((blockBotNw.Sun + blockBotN.Sun + blockMidNw.Sun + blockMidN.Sun)/4);

                redTl = (1f/MAX_SUN_VALUE)*((blockTopNe.R + blockTopN.R + blockMidNe.R + blockMidN.R)/4);
                redTr = (1f/MAX_SUN_VALUE)*((blockTopNw.R + blockTopN.R + blockMidNw.R + blockMidN.R)/4);
                redBl = (1f/MAX_SUN_VALUE)*((blockBotNe.R + blockBotN.R + blockMidNe.R + blockMidN.R)/4);
                redBr = (1f/MAX_SUN_VALUE)*((blockBotNw.R + blockBotN.R + blockMidNw.R + blockMidN.R)/4);

                grnTl = (1f/MAX_SUN_VALUE)*((blockTopNe.G + blockTopN.G + blockMidNe.G + blockMidN.G)/4);
                grnTr = (1f/MAX_SUN_VALUE)*((blockTopNw.G + blockTopN.G + blockMidNw.G + blockMidN.G)/4);
                grnBl = (1f/MAX_SUN_VALUE)*((blockBotNe.G + blockBotN.G + blockMidNe.G + blockMidN.G)/4);
                grnBr = (1f/MAX_SUN_VALUE)*((blockBotNw.G + blockBotN.G + blockMidNw.G + blockMidN.G)/4);

                bluTl = (1f/MAX_SUN_VALUE)*((blockTopNe.B + blockTopN.B + blockMidNe.B + blockMidN.B)/4);
                bluTr = (1f/MAX_SUN_VALUE)*((blockTopNw.B + blockTopN.B + blockMidNw.B + blockMidN.B)/4);
                bluBl = (1f/MAX_SUN_VALUE)*((blockBotNe.B + blockBotN.B + blockMidNe.B + blockMidN.B)/4);
                bluBr = (1f/MAX_SUN_VALUE)*((blockBotNw.B + blockBotN.B + blockMidNw.B + blockMidN.B)/4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.ZIncreasing,
                    block.Id, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr);
            }
        }

        #endregion

        #region BuildPlantVertexList

        private void BuildPlantVertexList(Block block, ChunkBuilder chunk, Vector3I chunkRelativePosition)
        {

            var blockPosition = chunk.Position + chunkRelativePosition;

            //get signed bytes from these to be able to remove 1 without further casts
            var x = (sbyte) chunkRelativePosition.X;
            var y = (sbyte) chunkRelativePosition.Y;
            var z = (sbyte) chunkRelativePosition.Z;


            //Block blockTopNW, blockTopN, blockTopNE, blockTopW, blockTopM, blockTopE, blockTopSW, blockTopS, blockTopSE;
            //Block blockMidNW, blockMidN, blockMidNE, blockMidW, blockMidM, blockMidE, blockMidSW, blockMidS, blockMidSE;
            //Block blockBotNW, blockBotN, blockBotNE, blockBotW, blockBotM, blockBotE, blockBotSW, blockBotS, blockBotSE;

            //Block solidBlock = new Block(BlockType.Rock);

            //blockTopNW = ChunkObject.GetBlock(X - 1, Y + 1, Z + 1);
            //blockTopN = ChunkObject.GetBlock(X, Y + 1, Z + 1);
            //blockTopNE = ChunkObject.GetBlock(X + 1, Y + 1, Z + 1);
            //blockTopW = ChunkObject.GetBlock(X - 1, Y + 1, Z);
            //blockTopM = ChunkObject.GetBlock(X, Y + 1, Z);
            //blockTopE = ChunkObject.GetBlock(X + 1, Y + 1, Z);
            //blockTopSW = ChunkObject.GetBlock(X - 1, Y + 1, Z - 1);
            //blockTopS = ChunkObject.GetBlock(X, Y + 1, Z - 1);
            //blockTopSE = ChunkObject.GetBlock(X + 1, Y + 1, Z - 1);

            //blockMidNW = ChunkObject.GetBlock(X - 1, Y, Z + 1);
            //blockMidN = ChunkObject.GetBlock(X, Y, Z + 1);
            //blockMidNE = ChunkObject.GetBlock(X + 1, Y, Z + 1);
            //blockMidW = ChunkObject.GetBlock(X - 1, Y, Z);
            //blockMidM = ChunkObject.GetBlock(X, Y, Z);
            //blockMidE = ChunkObject.GetBlock(X + 1, Y, Z);
            //blockMidSW = ChunkObject.GetBlock(X - 1, Y, Z - 1);
            //blockMidS = ChunkObject.GetBlock(X, Y, Z - 1);
            //blockMidSE = ChunkObject.GetBlock(X + 1, Y, Z - 1);

            //blockBotNW = ChunkObject.GetBlock(X - 1, Y - 1, Z + 1);
            //blockBotN = ChunkObject.GetBlock(X, Y - 1, Z + 1);
            //blockBotNE = ChunkObject.GetBlock(X + 1, Y - 1, Z + 1);
            //blockBotW = ChunkObject.GetBlock(X - 1, Y - 1, Z);
            //blockBotM = ChunkObject.GetBlock(X, Y - 1, Z);
            //blockBotE = ChunkObject.GetBlock(X + 1, Y - 1, Z);
            //blockBotSW = ChunkObject.GetBlock(X - 1, Y - 1, Z - 1);
            //blockBotS = ChunkObject.GetBlock(X, Y - 1, Z - 1);
            //blockBotSE = ChunkObject.GetBlock(X + 1, Y - 1, Z - 1);

            //float sunTR, sunTL, sunBR, sunBL;
            //float redTR, redTL, redBR, redBL;
            //float grnTR, grnTL, grnBR, grnBL;
            //float bluTR, bluTL, bluBR, bluBL;
            //Color localTR, localTL, localBR, localBL;

            //localTR = Color.White; localTL = Color.White; localBR = Color.White; localBL = Color.White;

            //sunTL = (1f / MAX_SUN_VALUE) * ((blockTopNW.Sun + blockTopW.Sun + blockMidNW.Sun + blockMidW.Sun) / 4);
            //sunTR = (1f / MAX_SUN_VALUE) * ((blockTopSW.Sun + blockTopW.Sun + blockMidSW.Sun + blockMidW.Sun) / 4);
            //sunBL = (1f / MAX_SUN_VALUE) * ((blockBotNW.Sun + blockBotW.Sun + blockMidNW.Sun + blockMidW.Sun) / 4);
            //sunBR = (1f / MAX_SUN_VALUE) * ((blockBotSW.Sun + blockBotW.Sun + blockMidSW.Sun + blockMidW.Sun) / 4);

            //redTL = (1f / MAX_SUN_VALUE) * ((blockTopNW.R + blockTopW.R + blockMidNW.R + blockMidW.R) / 4);
            //redTR = (1f / MAX_SUN_VALUE) * ((blockTopSW.R + blockTopW.R + blockMidSW.R + blockMidW.R) / 4);
            //redBL = (1f / MAX_SUN_VALUE) * ((blockBotNW.R + blockBotW.R + blockMidNW.R + blockMidW.R) / 4);
            //redBR = (1f / MAX_SUN_VALUE) * ((blockBotSW.R + blockBotW.R + blockMidSW.R + blockMidW.R) / 4);

            //grnTL = (1f / MAX_SUN_VALUE) * ((blockTopNW.G + blockTopW.G + blockMidNW.G + blockMidW.G) / 4);
            //grnTR = (1f / MAX_SUN_VALUE) * ((blockTopSW.G + blockTopW.G + blockMidSW.G + blockMidW.G) / 4);
            //grnBL = (1f / MAX_SUN_VALUE) * ((blockBotNW.G + blockBotW.G + blockMidNW.G + blockMidW.G) / 4);
            //grnBR = (1f / MAX_SUN_VALUE) * ((blockBotSW.G + blockBotW.G + blockMidSW.G + blockMidW.G) / 4);

            //bluTL = (1f / MAX_SUN_VALUE) * ((blockTopNW.B + blockTopW.B + blockMidNW.B + blockMidW.B) / 4);
            //bluTR = (1f / MAX_SUN_VALUE) * ((blockTopSW.B + blockTopW.B + blockMidSW.B + blockMidW.B) / 4);
            //bluBL = (1f / MAX_SUN_VALUE) * ((blockBotNW.B + blockBotW.B + blockMidNW.B + blockMidW.B) / 4);
            //bluBR = (1f / MAX_SUN_VALUE) * ((blockBotSW.B + blockBotW.B + blockMidSW.B + blockMidW.B) / 4);

            //localTL = new Color(redTL, grnTL, bluTL);
            //localTR = new Color(redTR, grnTR, bluTR);
            //localBL = new Color(redBL, grnBL, bluBL);
            //localBR = new Color(redBR, grnBR, bluBR);

            //_blockRenderer.BuildFaceVertices(blockPosition, chunkRelativePosition, BlockFaceDirection.XDecreasing, block.Id, sunTL, sunTR, sunBL, sunBR, localTL, localTR, localBL, localBR);

            BuildPlantVertices(chunk, blockPosition, chunkRelativePosition, block.Id, 0.6f, Color.LightGray);
        }

        #endregion

        #region BuildGrassVertexList

        private void BuildGrassVertexList(Block block, ChunkBuilder chunk, Vector3I chunkRelativePosition)
        {

            var blockPosition = chunk.Position + chunkRelativePosition;

            //get signed bytes from these to be able to remove 1 without further casts
            var x = (sbyte) chunkRelativePosition.X;
            var y = (sbyte) chunkRelativePosition.Y;
            var z = (sbyte) chunkRelativePosition.Z;


            //Block blockTopNW, blockTopN, blockTopNE, blockTopW, blockTopM, blockTopE, blockTopSW, blockTopS, blockTopSE;
            //Block blockMidNW, blockMidN, blockMidNE, blockMidW, blockMidM, blockMidE, blockMidSW, blockMidS, blockMidSE;
            //Block blockBotNW, blockBotN, blockBotNE, blockBotW, blockBotM, blockBotE, blockBotSW, blockBotS, blockBotSE;

            //Block solidBlock = new Block(BlockType.Rock);

            //blockTopNW = ChunkObject.GetBlock(X - 1, Y + 1, Z + 1);
            //blockTopN = ChunkObject.GetBlock(X, Y + 1, Z + 1);
            //blockTopNE = ChunkObject.GetBlock(X + 1, Y + 1, Z + 1);
            //blockTopW = ChunkObject.GetBlock(X - 1, Y + 1, Z);
            //blockTopM = ChunkObject.GetBlock(X, Y + 1, Z);
            //blockTopE = ChunkObject.GetBlock(X + 1, Y + 1, Z);
            //blockTopSW = ChunkObject.GetBlock(X - 1, Y + 1, Z - 1);
            //blockTopS = ChunkObject.GetBlock(X, Y + 1, Z - 1);
            //blockTopSE = ChunkObject.GetBlock(X + 1, Y + 1, Z - 1);

            //blockMidNW = ChunkObject.GetBlock(X - 1, Y, Z + 1);
            //blockMidN = ChunkObject.GetBlock(X, Y, Z + 1);
            //blockMidNE = ChunkObject.GetBlock(X + 1, Y, Z + 1);
            //blockMidW = ChunkObject.GetBlock(X - 1, Y, Z);
            //blockMidM = ChunkObject.GetBlock(X, Y, Z);
            //blockMidE = ChunkObject.GetBlock(X + 1, Y, Z);
            //blockMidSW = ChunkObject.GetBlock(X - 1, Y, Z - 1);
            //blockMidS = ChunkObject.GetBlock(X, Y, Z - 1);
            //blockMidSE = ChunkObject.GetBlock(X + 1, Y, Z - 1);

            //blockBotNW = ChunkObject.GetBlock(X - 1, Y - 1, Z + 1);
            //blockBotN = ChunkObject.GetBlock(X, Y - 1, Z + 1);
            //blockBotNE = ChunkObject.GetBlock(X + 1, Y - 1, Z + 1);
            //blockBotW = ChunkObject.GetBlock(X - 1, Y - 1, Z);
            //blockBotM = ChunkObject.GetBlock(X, Y - 1, Z);
            //blockBotE = ChunkObject.GetBlock(X + 1, Y - 1, Z);
            //blockBotSW = ChunkObject.GetBlock(X - 1, Y - 1, Z - 1);
            //blockBotS = ChunkObject.GetBlock(X, Y - 1, Z - 1);
            //blockBotSE = ChunkObject.GetBlock(X + 1, Y - 1, Z - 1);

            //float sunTR, sunTL, sunBR, sunBL;
            //float redTR, redTL, redBR, redBL;
            //float grnTR, grnTL, grnBR, grnBL;
            //float bluTR, bluTL, bluBR, bluBL;
            //Color localTR, localTL, localBR, localBL;

            //localTR = Color.White; localTL = Color.White; localBR = Color.White; localBL = Color.White;

            //sunTL = (1f / MAX_SUN_VALUE) * ((blockTopNW.Sun + blockTopW.Sun + blockMidNW.Sun + blockMidW.Sun) / 4);
            //sunTR = (1f / MAX_SUN_VALUE) * ((blockTopSW.Sun + blockTopW.Sun + blockMidSW.Sun + blockMidW.Sun) / 4);
            //sunBL = (1f / MAX_SUN_VALUE) * ((blockBotNW.Sun + blockBotW.Sun + blockMidNW.Sun + blockMidW.Sun) / 4);
            //sunBR = (1f / MAX_SUN_VALUE) * ((blockBotSW.Sun + blockBotW.Sun + blockMidSW.Sun + blockMidW.Sun) / 4);

            //redTL = (1f / MAX_SUN_VALUE) * ((blockTopNW.R + blockTopW.R + blockMidNW.R + blockMidW.R) / 4);
            //redTR = (1f / MAX_SUN_VALUE) * ((blockTopSW.R + blockTopW.R + blockMidSW.R + blockMidW.R) / 4);
            //redBL = (1f / MAX_SUN_VALUE) * ((blockBotNW.R + blockBotW.R + blockMidNW.R + blockMidW.R) / 4);
            //redBR = (1f / MAX_SUN_VALUE) * ((blockBotSW.R + blockBotW.R + blockMidSW.R + blockMidW.R) / 4);

            //grnTL = (1f / MAX_SUN_VALUE) * ((blockTopNW.G + blockTopW.G + blockMidNW.G + blockMidW.G) / 4);
            //grnTR = (1f / MAX_SUN_VALUE) * ((blockTopSW.G + blockTopW.G + blockMidSW.G + blockMidW.G) / 4);
            //grnBL = (1f / MAX_SUN_VALUE) * ((blockBotNW.G + blockBotW.G + blockMidNW.G + blockMidW.G) / 4);
            //grnBR = (1f / MAX_SUN_VALUE) * ((blockBotSW.G + blockBotW.G + blockMidSW.G + blockMidW.G) / 4);

            //bluTL = (1f / MAX_SUN_VALUE) * ((blockTopNW.B + blockTopW.B + blockMidNW.B + blockMidW.B) / 4);
            //bluTR = (1f / MAX_SUN_VALUE) * ((blockTopSW.B + blockTopW.B + blockMidSW.B + blockMidW.B) / 4);
            //bluBL = (1f / MAX_SUN_VALUE) * ((blockBotNW.B + blockBotW.B + blockMidNW.B + blockMidW.B) / 4);
            //bluBR = (1f / MAX_SUN_VALUE) * ((blockBotSW.B + blockBotW.B + blockMidSW.B + blockMidW.B) / 4);

            //localTL = new Color(redTL, grnTL, bluTL);
            //localTR = new Color(redTR, grnTR, bluTR);
            //localBL = new Color(redBL, grnBL, bluBL);
            //localBR = new Color(redBR, grnBR, bluBR);

            //_blockRenderer.BuildFaceVertices(blockPosition, chunkRelativePosition, BlockFaceDirection.XDecreasing, block.Id, sunTL, sunTR, sunBL, sunBR, localTL, localTR, localBL, localBR);

            BuildGrassVertices(chunk, blockPosition, chunkRelativePosition, block.Id, 0.6f, Color.LightGray);
        }

        #endregion

        public void BuildGrassVertices(ChunkBuilder chunk, Vector3I blockPosition, Vector3I chunkRelativePosition,
            ushort blockType, float sunLight, Color localLight)
        {
            var texture = BlockLogic.GetTexture(blockType);

            var uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.XIncreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 1, 1),
                new Vector3(1, 0, 0), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 1, 0),
                new Vector3(1, 0, 0), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 0, 1),
                new Vector3(1, 0, 0), uvList[2], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 0, 0),
                new Vector3(1, 0, 0), uvList[5], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);

            uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.XDecreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 1, 0),
                new Vector3(-1, 0, 0), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 1, 1),
                new Vector3(-1, 0, 0), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 0, 0),
                new Vector3(-1, 0, 0), uvList[5], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 0, 1),
                new Vector3(-1, 0, 0), uvList[2], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);

            uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.XIncreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 1, 1),
                new Vector3(1, 0, 0), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 1, 0),
                new Vector3(1, 0, 0), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 0, 1),
                new Vector3(1, 0, 0), uvList[2], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 0, 0),
                new Vector3(1, 0, 0), uvList[5], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);

            uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.XDecreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 1, 0),
                new Vector3(-1, 0, 0), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 1, 1),
                new Vector3(-1, 0, 0), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 0, 0),
                new Vector3(-1, 0, 0), uvList[5], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 0, 1),
                new Vector3(-1, 0, 0), uvList[2], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);

            uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.ZIncreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 1, 0.3f),
                new Vector3(0, 0, 1), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 1, 0.3f),
                new Vector3(0, 0, 1), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0.3f),
                new Vector3(0, 0, 1), uvList[5], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0.3f),
                new Vector3(0, 0, 1), uvList[2], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);

            uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.ZDecreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 1, 0.3f),
                new Vector3(0, 0, -1), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 1, 0.3f),
                new Vector3(0, 0, -1), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0.3f),
                new Vector3(0, 0, -1), uvList[2], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0.3f),
                new Vector3(0, 0, -1), uvList[5], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);

            uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.ZIncreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 1, 0.7f),
                new Vector3(0, 0, 1), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 1, 0.7f),
                new Vector3(0, 0, 1), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0.7f),
                new Vector3(0, 0, 1), uvList[5], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0.7f),
                new Vector3(0, 0, 1), uvList[2], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);

            uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.ZDecreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 1, 0.7f),
                new Vector3(0, 0, -1), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 1, 0.7f),
                new Vector3(0, 0, -1), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0.7f),
                new Vector3(0, 0, -1), uvList[2], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0.7f),
                new Vector3(0, 0, -1), uvList[5], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);
        }

        #region BuildPlantVertices

        public void BuildPlantVertices(ChunkBuilder chunk, Vector3I blockPosition, Vector3I chunkRelativePosition,
            ushort blockType, float sunLight, Color localLight)
        {
            var texture = BlockLogic.GetTexture(blockType);

            var uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.XIncreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.5f, 1, 1),
                new Vector3(1, 0, 0), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.5f, 1, 0),
                new Vector3(1, 0, 0), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.5f, 0, 1),
                new Vector3(1, 0, 0), uvList[2], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.5f, 0, 0),
                new Vector3(1, 0, 0), uvList[5], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);

            uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.XDecreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.5f, 1, 0),
                new Vector3(-1, 0, 0), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.5f, 1, 1),
                new Vector3(-1, 0, 0), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.5f, 0, 0),
                new Vector3(-1, 0, 0), uvList[5], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.5f, 0, 1),
                new Vector3(-1, 0, 0), uvList[2], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);

            uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.ZIncreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 1, 0.5f),
                new Vector3(0, 0, 1), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 1, 0.5f),
                new Vector3(0, 0, 1), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0.5f),
                new Vector3(0, 0, 1), uvList[5], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0.5f),
                new Vector3(0, 0, 1), uvList[2], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);

            uvList = TextureHelper.UvMappings[(int) texture*6 + (int) BlockFaceDirection.ZDecreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 1, 0.5f),
                new Vector3(0, 0, -1), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 1, 0.5f),
                new Vector3(0, 0, -1), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0.5f),
                new Vector3(0, 0, -1), uvList[2], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0.5f),
                new Vector3(0, 0, -1), uvList[5], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);
        }

        #endregion

        #region BuildFaceVertices

        public void BuildFaceVertices(ChunkBuilder chunk, Vector3I blockPosition, Vector3I chunkRelativePosition,
            BlockFaceDirection faceDir, ushort blockType, float sunLightTl, float sunLightTr, float sunLightBl,
            float sunLightBr, Color localLightTl, Color localLightTr, Color localLightBl, Color localLightBr)
        {
            var texture = BlockLogic.GetTexture(blockType, faceDir);

            var faceIndex = (int) faceDir;

            var uvList = TextureHelper.UvMappings[(int) texture*6 + faceIndex];

            float height = 1;
            if (BlockLogic.IsCapBlock(blockType)) height = 0.1f;
            if (BlockLogic.IsHalfBlock(blockType)) height = 0.5f;
            switch (faceDir)
            {
                case BlockFaceDirection.XIncreasing:
                {
                    //TR,TL,BR,BR,TL,BL
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, height, 1),
                        new Vector3(1, 0, 0), uvList[0], sunLightTr, localLightTr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, height, 0),
                        new Vector3(1, 0, 0), uvList[1], sunLightTl, localLightTl);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 1),
                        new Vector3(1, 0, 0), uvList[2], sunLightBr, localLightBr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0),
                        new Vector3(1, 0, 0), uvList[5], sunLightBl, localLightBl);
                    AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);
                }
                    break;

                case BlockFaceDirection.XDecreasing:
                {
                    //TR,TL,BL,TR,BL,BR
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, height, 0),
                        new Vector3(-1, 0, 0), uvList[0], sunLightTr, localLightTr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, height, 1),
                        new Vector3(-1, 0, 0), uvList[1], sunLightTl, localLightTl);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0),
                        new Vector3(-1, 0, 0), uvList[5], sunLightBr, localLightBr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 1),
                        new Vector3(-1, 0, 0), uvList[2], sunLightBl, localLightBl);
                    AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);
                }
                    break;

                case BlockFaceDirection.YIncreasing:
                {
                    //BL,BR,TR,BL,TR,TL
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, height, 1),
                        new Vector3(0, 1, 0), uvList[4], sunLightTr, localLightTr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, height, 1),
                        new Vector3(0, 1, 0), uvList[5], sunLightTl, localLightTl);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, height, 0),
                        new Vector3(0, 1, 0), uvList[1], sunLightBr, localLightBr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, height, 0),
                        new Vector3(0, 1, 0), uvList[3], sunLightBl, localLightBl);
                    AddIndex(chunk, blockType, 3, 2, 0, 3, 0, 1);
                }
                    break;

                case BlockFaceDirection.YDecreasing:
                {
                    //TR,BR,TL,TL,BR,BL
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 1),
                        new Vector3(0, -1, 0), uvList[0], sunLightTr, localLightTr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 1),
                        new Vector3(0, -1, 0), uvList[2], sunLightTl, localLightTl);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0),
                        new Vector3(0, -1, 0), uvList[4], sunLightBr, localLightBr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0),
                        new Vector3(0, -1, 0), uvList[5], sunLightBl, localLightBl);
                    AddIndex(chunk, blockType, 0, 2, 1, 1, 2, 3);
                }
                    break;

                case BlockFaceDirection.ZIncreasing:
                {
                    //TR,TL,BL,TR,BL,BR
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, height, 1),
                        new Vector3(0, 0, 1), uvList[0], sunLightTr, localLightTr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, height, 1),
                        new Vector3(0, 0, 1), uvList[1], sunLightTl, localLightTl);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 1),
                        new Vector3(0, 0, 1), uvList[5], sunLightBr, localLightBr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 1),
                        new Vector3(0, 0, 1), uvList[2], sunLightBl, localLightBl);
                    AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);
                }
                    break;

                case BlockFaceDirection.ZDecreasing:
                {
                    //TR,TL,BR,BR,TL,BL
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, height, 0),
                        new Vector3(0, 0, -1), uvList[0], sunLightTr, localLightTr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, height, 0),
                        new Vector3(0, 0, -1), uvList[1], sunLightTl, localLightTl);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0),
                        new Vector3(0, 0, -1), uvList[2], sunLightBr, localLightBr);
                    AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0),
                        new Vector3(0, 0, -1), uvList[5], sunLightBl, localLightBl);
                    AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);
                }
                    break;
            }
        }

        #endregion

        private void AddVertex(ChunkBuilder chunk, ushort blockType, Vector3I blockPosition, Vector3I chunkRelativePosition,
            Vector3 vertexAdd, Vector3 normal, Vector2 uv1, float sunLight, Color localLight)
        {
            if (blockType != BlockType.Water)
            {
                chunk.VertexList.Add(new VertexPositionTextureLight((Vector3) blockPosition + vertexAdd, uv1, sunLight,
                    localLight.ToVector3()));
            }
            else
            {
                chunk.WaterVertexList.Add(new VertexPositionTextureLight((Vector3) blockPosition + vertexAdd, uv1,
                    sunLight, localLight.ToVector3()));
            }
        }

        #region AddIndex

        private void AddIndex(ChunkBuilder chunk, ushort blockType, short i1, short i2, short i3, short i4, short i5,
            short i6)
        {
            if (blockType != BlockType.Water)
            {
                chunk.IndexList.Add((short) (chunk.VertexCount + i1));
                chunk.IndexList.Add((short) (chunk.VertexCount + i2));
                chunk.IndexList.Add((short) (chunk.VertexCount + i3));
                chunk.IndexList.Add((short) (chunk.VertexCount + i4));
                chunk.IndexList.Add((short) (chunk.VertexCount + i5));
                chunk.IndexList.Add((short) (chunk.VertexCount + i6));
                chunk.VertexCount += 4;
            }
            else
            {
                chunk.WaterIndexList.Add((short) (chunk.WaterVertexCount + i1));
                chunk.WaterIndexList.Add((short) (chunk.WaterVertexCount + i2));
                chunk.WaterIndexList.Add((short) (chunk.WaterVertexCount + i3));
                chunk.WaterIndexList.Add((short) (chunk.WaterVertexCount + i4));
                chunk.WaterIndexList.Add((short) (chunk.WaterVertexCount + i5));
                chunk.WaterIndexList.Add((short) (chunk.WaterVertexCount + i6));
                chunk.WaterVertexCount += 4;
            }
        }

        #endregion

        public void ProcessChunk(ChunkBuilder chunk)
        {
            chunk.Clear();
            BuildVertexList(chunk);
        }

    }
}
