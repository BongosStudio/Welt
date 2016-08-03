#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Welt.API.Forge;
using Welt.Core.Forge;
using Welt.Forge;
using Welt.Logic.Forge;
using Welt.Models;

#endregion

namespace Welt.Processors
{
    public class LightingChunkProcessor : IChunkProcessor
    {
        private const int MAX_SUN_VALUE = 16;
        private byte _r, _g, _b;

        public void ProcessChunk(ChunkObject chunk)
        {
            
            ClearLighting(chunk);
            FillLighting(chunk);
        }

        #region ClearLighting

        private void ClearLighting(ChunkObject chunk)
        {
            try
            {
                byte sunValue = MAX_SUN_VALUE;
                Block block;

                for (byte x = 0; x < ChunkObject.Size.X; x++)
                {
                    for (byte z = 0; z < ChunkObject.Size.Z; z++)
                    {
                            // we don't want this x-z value to be calculated each in in y-loop!
                        var inShade = false;
                        //for (byte y = ChunkObject.MAX.Y; y > 0; y--)
                        for (var y = ChunkObject.Max.Y; y > chunk.LowestNoneBlock.Y; y--)
                        {
                            block = chunk.GetBlock(x, y, z);
                            if (block.Id != BlockType.None) inShade = true;
                            block.Sun = (byte) (!inShade ? sunValue : 0);
                            
                            BlockLogic.GetLightLevel(block.Id, 
                                out block.R,
                                out block.G, 
                                out block.B);
                            chunk.SetBlockUnsafe(x, y, z, block);

                            //if (ChunkObject.Blocks[offset + y].Id == BlockType.RedFlower)
                            //{
                            //    ChunkObject.Blocks[offset + y].R = (byte) m_r.Next(17);
                            //    ChunkObject.Blocks[offset + y].G = (byte) m_r.Next(17);
                            //    ChunkObject.Blocks[offset + y].B = (byte) m_r.Next(17);
                            //}
                            //else
                            //{
                            //    ChunkObject.Blocks[offset + y].R = 0;
                            //    ChunkObject.Blocks[offset + y].G = 0;
                            //    ChunkObject.Blocks[offset + y].B = 0;
                            //}
                        }
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("ClearLighting Exception");
            }
        }

        #endregion

        #region PropogateLight

        private byte Attenuate(byte light)
        {
            return (byte)((light * 9) / 10);
        }

        private void PropogateLightSun(ChunkObject chunk, byte x, byte y, byte z, byte light)
        {
            var block = chunk.GetBlock(x, y, z);
            if (block.Id != BlockType.None && block.Id != BlockType.Water) return;
            if (block.Sun >= light) return;
            block.Sun = light;

            if (light <= 1) return;
            light = Attenuate(light);

            // Propogate light within this ChunkObject
            if (x > 0) PropogateLightSun(chunk, (byte) (x - 1), y, z, light);
            if (x < ChunkObject.Max.X) PropogateLightSun(chunk, (byte) (x + 1), y, z, light);
            if (y > 0) PropogateLightSun(chunk, x, (byte) (y - 1), z, light);
            if (y < ChunkObject.Max.Y) PropogateLightSun(chunk, x, (byte) (y + 1), z, light);
            if (z > 0) PropogateLightSun(chunk, x, y, (byte) (z - 1), light);
            if (z < ChunkObject.Max.Z) PropogateLightSun(chunk, x, y, (byte) (z + 1), light);

            //if (ChunkObject.E == null || ChunkObject.W == null || ChunkObject.S == null || ChunkObject.N == null)
            //{
            //    throw new Exception("LIGHTING ISSUE");
            //}

            if (chunk.E != null && x == 0) PropogateLightSun((ChunkObject) chunk.E, (ChunkObject.Max.X), y, z, light);
            if (chunk.W != null && (x == ChunkObject.Max.X)) PropogateLightSun((ChunkObject) chunk.W, 0, y, z, light);
            if (chunk.S != null && z == 0) PropogateLightSun((ChunkObject) chunk.S, x, y, (ChunkObject.Max.Z), light);
            if (chunk.N != null && (z == ChunkObject.Max.Z)) PropogateLightSun((ChunkObject) chunk.N, x, y, 0, light);

            chunk.SetBlockUnsafe(x, y, z, block);
        }

        private void PropogateLightR(ChunkObject chunk, byte x, byte y, byte z, byte lightR)
        {
            try
            {
                var block = chunk.GetBlock(x, y, z);
                if (block.Id != BlockType.None && block.Id != BlockType.Water) return;
                if (block.R >= lightR) return;
                block.R = lightR;
                if (chunk.State > ChunkState.Lighting)
                {
                    chunk.State = ChunkState.AwaitingBuild;
                }
                if (lightR <= 1) return;
                lightR = Attenuate(lightR);

                if (x > 0) PropogateLightR(chunk, (byte)(x - 1), y, z, lightR);
                if (x < ChunkObject.Max.X) PropogateLightR(chunk, (byte)(x + 1), y, z, lightR);
                if (y > 0) PropogateLightR(chunk, x, (byte)(y - 1), z, lightR);
                if (y < ChunkObject.Max.Y) PropogateLightR(chunk, x, (byte)(y + 1), z, lightR);
                if (z > 0) PropogateLightR(chunk, x, y, (byte)(z - 1), lightR);
                if (z < ChunkObject.Max.Z) PropogateLightR(chunk, x, y, (byte)(z + 1), lightR);

                if (chunk.E != null && x == 0) PropogateLightR((ChunkObject) chunk.E, (ChunkObject.Max.X), y, z, lightR);
                if (chunk.W != null && (x == ChunkObject.Max.X)) PropogateLightR((ChunkObject) chunk.W, 0, y, z, lightR);
                if (chunk.S != null && z == 0) PropogateLightR((ChunkObject) chunk.S, x, y, (ChunkObject.Max.Z), lightR);
                if (chunk.N != null && (z == ChunkObject.Max.Z)) PropogateLightR((ChunkObject) chunk.N, x, y, 0, lightR);

                chunk.SetBlockUnsafe(x, y, z, block);
            }
            catch (Exception)
            {
                Debug.WriteLine("PropogateLightR Exception");
            }
        }

        private void PropogateLightG(ChunkObject chunk, byte x, byte y, byte z, byte lightG)
        {
            try
            {
                var block = chunk.GetBlock(x, y, z);
                if (block.Id != BlockType.None && block.Id != BlockType.Water) return;
                if (chunk.GetBlock(x, y, z).G >= lightG) return;
                block.G = lightG;

                if (lightG <= 1) return;
                lightG = Attenuate(lightG);
                if (x > 0) PropogateLightG(chunk, (byte)(x - 1), y, z, lightG);
                if (x < ChunkObject.Max.X) PropogateLightG(chunk, (byte)(x + 1), y, z, lightG);
                if (y > 0) PropogateLightG(chunk, x, (byte)(y - 1), z, lightG);
                if (y < ChunkObject.Max.Y) PropogateLightG(chunk, x, (byte)(y + 1), z, lightG);
                if (z > 0) PropogateLightG(chunk, x, y, (byte)(z - 1), lightG);
                if (z < ChunkObject.Max.Z) PropogateLightG(chunk, x, y, (byte)(z + 1), lightG);

                if (chunk.E != null && x == 0) PropogateLightG((ChunkObject) chunk.E, (ChunkObject.Max.X), y, z, lightG);
                if (chunk.W != null && (x == ChunkObject.Max.X)) PropogateLightG((ChunkObject) chunk.W, 0, y, z, lightG);
                if (chunk.S != null && z == 0) PropogateLightG((ChunkObject) chunk.S, x, y, (ChunkObject.Max.Z), lightG);
                if (chunk.N != null && (z == ChunkObject.Max.Z)) PropogateLightG((ChunkObject) chunk.N, x, y, 0, lightG);

                chunk.SetBlockUnsafe(x, y, z, block);
            }
            catch (Exception)
            {
                Debug.WriteLine("PropogateLightG Exception");
            }
        }

        private void PropogateLightB(ChunkObject chunk, byte x, byte y, byte z, byte lightB)
        {
            try
            {
                var block = chunk.GetBlock(x, y, z);
                if (block.Id != BlockType.None && block.Id != BlockType.Water) return;
                if (block.B >= lightB) return;
                block.B = lightB;

                if (lightB <= 1) return;
                lightB = Attenuate(lightB);

                if (x > 0) PropogateLightB(chunk, (byte)(x - 1), y, z, lightB);
                if (x < ChunkObject.Max.X) PropogateLightB(chunk, (byte)(x + 1), y, z, lightB);
                if (y > 0) PropogateLightB(chunk, x, (byte)(y - 1), z, lightB);
                if (y < ChunkObject.Max.Y) PropogateLightB(chunk, x, (byte)(y + 1), z, lightB);
                if (z > 0) PropogateLightB(chunk, x, y, (byte)(z - 1), lightB);
                if (z < ChunkObject.Max.Z) PropogateLightB(chunk, x, y, (byte)(z + 1), lightB);

                if (chunk.E != null && x == 0) PropogateLightB((ChunkObject) chunk.E, (ChunkObject.Max.X), y, z, lightB);
                if (chunk.W != null && (x == ChunkObject.Max.X)) PropogateLightB((ChunkObject) chunk.W, 0, y, z, lightB);
                if (chunk.S != null && z == 0) PropogateLightB((ChunkObject) chunk.S, x, y, (ChunkObject.Max.Z), lightB);
                if (chunk.N != null && (z == ChunkObject.Max.Z)) PropogateLightB((ChunkObject) chunk.N, x, y, 0, lightB);

                chunk.SetBlockUnsafe(x, y, z, block);
            }
            catch (Exception)
            {
                Debug.WriteLine("PropogateLightB Exception");
            }
        }
        #endregion

        #region FillLighting
        private void FillLighting(ChunkObject chunk)
        {
            FillLightingSun(chunk);
            FillLightingR(chunk);
            FillLightingG(chunk);
            FillLightingB(chunk);
        }

        private void FillLightingSun(ChunkObject chunk)
        {

            for (byte x = 0; x < ChunkObject.Size.X; x++)
            {
                for (byte z = 0; z < ChunkObject.Size.Z; z++)
                {
                    //for (byte y = 0; y < ChunkObject.SIZE.Y; y++)
                    for (var y = chunk.LowestNoneBlock.Y; y < ChunkObject.Size.Y; y++)
                    {
                        var block = chunk.GetBlock(x, y, z);
                        if (block.Id != BlockType.None) continue;
                        // Sunlight
                        if (block.Sun > 1)
                        {
                            var light = Attenuate(block.Sun);

                            if (x > 0) PropogateLightSun(chunk, (byte)(x - 1), y, z, light);
                            if (x < ChunkObject.Max.X) PropogateLightSun(chunk, (byte)(x + 1), y, z, light);
                            if (y > 0) PropogateLightSun(chunk, x, (byte)(y - 1), z, light);
                            if (y < ChunkObject.Max.Y) PropogateLightSun(chunk, x, (byte)(y + 1), z, light);
                            if (z > 0) PropogateLightSun(chunk, x, y, (byte)(z - 1), light);
                            if (z < ChunkObject.Max.Z) PropogateLightSun(chunk, x, y, (byte)(z + 1), light);
                        }

                        // Pull in light from neighbours
                        if (chunk.E!=null && x == 0) PropogateLightSun(chunk, x, y, z, chunk.E.GetBlock(ChunkObject.Max.X, y, z).Sun);
                        if (chunk.W!=null && x == ChunkObject.Max.X) PropogateLightSun(chunk, x, y, z, chunk.W.GetBlock(0, y, z).Sun);
                        if (chunk.S!=null && z == 0) PropogateLightSun(chunk, x, y, z, chunk.S.GetBlock(x, y, ChunkObject.Max.Z).Sun);
                        if (chunk.N!=null && z == ChunkObject.Max.Z) PropogateLightSun(chunk, x, y, z, chunk.N.GetBlock(x, y, 0).Sun);
                    }
                }
            }

        }

        private void FillLightingR(ChunkObject chunk)
        {
            try
            {
                for (byte x = 0; x < ChunkObject.Size.X; x++)
                {
                    for (byte z = 0; z < ChunkObject.Size.Z; z++)
                    {
                        for (var y = chunk.LowestNoneBlock.Y; y < ChunkObject.Size.Y; y++)
                        {
                            BlockLogic.GetLightLevel(chunk.GetBlock(x, y, z).Id, out _r, out _g, out _b);
              
                            // Local light R
                            if (_r > 1)
                            {
                                var light = Attenuate(_r);

                                if (x > 0) PropogateLightR(chunk, (byte)(x - 1), y, z, light);
                                if (x < ChunkObject.Max.X) PropogateLightR(chunk, (byte)(x + 1), y, z, light);
                                if (y > 0) PropogateLightR(chunk, x, (byte)(y - 1), z, light);
                                if (y < ChunkObject.Max.Y) PropogateLightR(chunk, x, (byte)(y + 1), z, light);
                                if (z > 0) PropogateLightR(chunk, x, y, (byte)(z - 1), light);
                                if (z < ChunkObject.Max.Z) PropogateLightR(chunk, x, y, (byte)(z + 1), light);
                            }

                            // Pull in light from neighbours
                            if (chunk.E!=null && x == 0) PropogateLightR(chunk, x, y, z, chunk.E.GetBlock(ChunkObject.Max.X, y, z).R);
                            if (chunk.W!=null && x == ChunkObject.Max.X) PropogateLightR(chunk, x, y, z, chunk.W.GetBlock(0, y, z).R);
                            if (chunk.S!=null && z == 0) PropogateLightR(chunk, x, y, z, chunk.S.GetBlock(x, y, ChunkObject.Max.Z).R);
                            if (chunk.N!=null && z == ChunkObject.Max.Z) PropogateLightR(chunk, x, y, z, chunk.N.GetBlock(x, y, 0).R);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine("FillLightingR Exception");
            }
        }

        private void FillLightingG(ChunkObject chunk)
        {
            try
            {
                for (byte x = 0; x < ChunkObject.Size.X; x++)
                {
                    for (byte z = 0; z < ChunkObject.Size.Z; z++)
                    {
                        for (var y = chunk.LowestNoneBlock.Y; y < ChunkObject.Size.Y; y++)
                        {

                            BlockLogic.GetLightLevel(chunk.GetBlock(x, y, z).Id, out _r, out _g, out _b);

                            // Local light G
                            if (_g > 1)
                            {
                                var light = Attenuate(_g);
                                if (x > 0) PropogateLightG(chunk, (byte)(x - 1), y, z, light);
                                if (x < ChunkObject.Max.X) PropogateLightG(chunk, (byte)(x + 1), y, z, light);
                                if (y > 0) PropogateLightG(chunk, x, (byte)(y - 1), z, light);
                                if (y < ChunkObject.Max.Y) PropogateLightG(chunk, x, (byte)(y + 1), z, light);
                                if (z > 0) PropogateLightG(chunk, x, y, (byte)(z - 1), light);
                                if (z < ChunkObject.Max.Z) PropogateLightG(chunk, x, y, (byte)(z + 1), light);
                            }

                            // Pull in light from neighbours
                            if (chunk.E!=null && x == 0) PropogateLightG(chunk, x, y, z, chunk.E.GetBlock(ChunkObject.Max.X, y, z).G);
                            if (chunk.W!=null && x == ChunkObject.Max.X) PropogateLightG(chunk, x, y, z, chunk.W.GetBlock(0, y, z).G);
                            if (chunk.S!=null && z == 0) PropogateLightG(chunk, x, y, z, chunk.S.GetBlock(x, y, ChunkObject.Max.Z).G);
                            if (chunk.N!=null && z == ChunkObject.Max.Z) PropogateLightG(chunk, x, y, z, chunk.N.GetBlock(x, y, 0).G);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine("FillLightingG Exception");
            }
        }

        private void FillLightingB(ChunkObject chunk)
        {
            try
            {
                for (byte x = 0; x < ChunkObject.Size.X; x++)
                {
                    for (byte z = 0; z < ChunkObject.Size.Z; z++)
                    {
                        for (var y = chunk.LowestNoneBlock.Y; y < ChunkObject.Size.Y; y++)
                        {

                            BlockLogic.GetLightLevel(chunk.GetBlock(x, y, z).Id, out _r, out _g, out _b);

                            // Local light B
                            if (_b > 1)
                            {
                                var light = Attenuate(_b);
                                if (x > 0) PropogateLightB(chunk, (byte)(x - 1), y, z, light);
                                if (x < ChunkObject.Max.X) PropogateLightB(chunk, (byte)(x + 1), y, z, light);
                                if (y > 0) PropogateLightB(chunk, x, (byte)(y - 1), z, light);
                                if (y < ChunkObject.Max.Y) PropogateLightB(chunk, x, (byte)(y + 1), z, light);
                                if (z > 0) PropogateLightB(chunk, x, y, (byte)(z - 1), light);
                                if (z < ChunkObject.Max.Z) PropogateLightB(chunk, x, y, (byte)(z + 1), light);
                            }

                            // Pull in light from neighbours
                            if (chunk.E!=null && x == 0) PropogateLightB(chunk, x, y, z, chunk.E.GetBlock(ChunkObject.Max.X, y, z).B);
                            if (chunk.W!=null && x == ChunkObject.Max.X) PropogateLightB(chunk, x, y, z, chunk.W.GetBlock(0, y, z).B);
                            if (chunk.S!=null && z == 0) PropogateLightB(chunk, x, y, z, chunk.S.GetBlock(x, y, ChunkObject.Max.Z).B);
                            if (chunk.N!=null && z == ChunkObject.Max.Z) PropogateLightB(chunk, x, y, z, chunk.N.GetBlock(x, y, 0).B);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine("FillLightingB Exception");
            }
        }
        #endregion

    }
}
