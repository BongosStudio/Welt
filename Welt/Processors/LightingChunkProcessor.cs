#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Welt.Forge;
using Welt.Models;

#endregion

namespace Welt.Processors
{
    public class LightingChunkProcessor : IChunkProcessor
    {
        private const int MAX_SUN_VALUE = 16;
        private readonly Random _mR = new Random();

        public void ProcessChunk(Chunk chunk)
        {
            ClearLighting(chunk);
            FillLighting(chunk);
        }

        #region ClearLighting

        private void ClearLighting(Chunk chunk)
        {
            try
            {
                byte sunValue = MAX_SUN_VALUE;

                for (byte x = 0; x < Chunk.Size.X; x++)
                {
                    for (byte z = 0; z < Chunk.Size.Z; z++)
                    {
                        var offset = x*Chunk.FlattenOffset + z*Chunk.Size.Y;
                            // we don't want this x-z value to be calculated each in in y-loop!
                        var inShade = false;
                        //for (byte y = Chunk.MAX.Y; y > 0; y--)
                        for (var y = Chunk.Max.Y; y > chunk.LowestNoneBlock.Y; y--)
                        {
                            if (chunk.Blocks[offset + y].Id != BlockType.None) inShade = true;
                            chunk.Blocks[offset + y].Sun = (byte) (!inShade ? sunValue : 0);
                            
                            Block.GetLightLevel(chunk.Blocks[offset + y].Id, 
                                out chunk.Blocks[offset + y].R,
                                out chunk.Blocks[offset + y].G, 
                                out chunk.Blocks[offset + y].B);

                            //if (chunk.Blocks[offset + y].Id == BlockType.RedFlower)
                            //{
                            //    chunk.Blocks[offset + y].R = (byte) m_r.Next(17);
                            //    chunk.Blocks[offset + y].G = (byte) m_r.Next(17);
                            //    chunk.Blocks[offset + y].B = (byte) m_r.Next(17);
                            //}
                            //else
                            //{
                            //    chunk.Blocks[offset + y].R = 0;
                            //    chunk.Blocks[offset + y].G = 0;
                            //    chunk.Blocks[offset + y].B = 0;
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

        private void PropogateLightSun(Chunk chunk, byte x, byte y, byte z, byte light)
        {
            var offset = x*Chunk.FlattenOffset + z*Chunk.Size.Y + y;
            if (chunk.Blocks[offset].Id != BlockType.None && chunk.Blocks[offset].Id != BlockType.Water) return;
            if (chunk.Blocks[offset].Sun >= light) return;
            chunk.Blocks[offset].Sun = light;

            if (light <= 1) return;
            light = Attenuate(light);

            // Propogate light within this chunk
            if (x > 0) PropogateLightSun(chunk, (byte) (x - 1), y, z, light);
            if (x < Chunk.Max.X) PropogateLightSun(chunk, (byte) (x + 1), y, z, light);
            if (y > 0) PropogateLightSun(chunk, x, (byte) (y - 1), z, light);
            if (y < Chunk.Max.Y) PropogateLightSun(chunk, x, (byte) (y + 1), z, light);
            if (z > 0) PropogateLightSun(chunk, x, y, (byte) (z - 1), light);
            if (z < Chunk.Max.Z) PropogateLightSun(chunk, x, y, (byte) (z + 1), light);

            //if (chunk.E == null || chunk.W == null || chunk.S == null || chunk.N == null)
            //{
            //    throw new Exception("LIGHTING ISSUE");
            //}

            if (chunk.E != null && x == 0) PropogateLightSun(chunk.E, (Chunk.Max.X), y, z, light);
            if (chunk.W != null && (x == Chunk.Max.X)) PropogateLightSun(chunk.W, 0, y, z, light);
            if (chunk.S != null && z == 0) PropogateLightSun(chunk.S, x, y, (Chunk.Max.Z), light);
            if (chunk.N != null && (z == Chunk.Max.Z)) PropogateLightSun(chunk.N, x, y, 0, light);
        }

        private void PropogateLightR(Chunk chunk, byte x, byte y, byte z, byte lightR)
        {
            try
            {
                var offset = x * Chunk.FlattenOffset + z * Chunk.Size.Y + y;
                if (chunk.Blocks[offset].Id != BlockType.None && chunk.Blocks[offset].Id != BlockType.Water) return;
                if (chunk.Blocks[offset].R >= lightR) return;
                chunk.Blocks[offset].R = lightR;
                if (chunk.State > ChunkState.Lighting) chunk.State = ChunkState.AwaitingBuild;
                if (lightR <= 1) return;
                lightR = Attenuate(lightR);

                if (x > 0) PropogateLightR(chunk, (byte)(x - 1), y, z, lightR);
                if (x < Chunk.Max.X) PropogateLightR(chunk, (byte)(x + 1), y, z, lightR);
                if (y > 0) PropogateLightR(chunk, x, (byte)(y - 1), z, lightR);
                if (y < Chunk.Max.Y) PropogateLightR(chunk, x, (byte)(y + 1), z, lightR);
                if (z > 0) PropogateLightR(chunk, x, y, (byte)(z - 1), lightR);
                if (z < Chunk.Max.Z) PropogateLightR(chunk, x, y, (byte)(z + 1), lightR);

                if (chunk.E != null && x == 0) PropogateLightR(chunk.E, (Chunk.Max.X), y, z, lightR);
                if (chunk.W != null && (x == Chunk.Max.X)) PropogateLightR(chunk.W, 0, y, z, lightR);
                if (chunk.S != null && z == 0) PropogateLightR(chunk.S, x, y, (Chunk.Max.Z), lightR);
                if (chunk.N != null && (z == Chunk.Max.Z)) PropogateLightR(chunk.N, x, y, 0, lightR);
            }
            catch (Exception)
            {
                Debug.WriteLine("PropogateLightR Exception");
            }
        }

        private void PropogateLightG(Chunk chunk, byte x, byte y, byte z, byte lightG)
        {
            try
            {
                var offset = x * Chunk.FlattenOffset + z * Chunk.Size.Y + y;
                if (chunk.Blocks[offset].Id != BlockType.None && chunk.Blocks[offset].Id != BlockType.Water) return;
                if (chunk.Blocks[offset].G >= lightG) return;
                chunk.Blocks[offset].G = lightG;

                if (lightG <= 1) return;
                lightG = Attenuate(lightG);
                if (x > 0) PropogateLightG(chunk, (byte)(x - 1), y, z, lightG);
                if (x < Chunk.Max.X) PropogateLightG(chunk, (byte)(x + 1), y, z, lightG);
                if (y > 0) PropogateLightG(chunk, x, (byte)(y - 1), z, lightG);
                if (y < Chunk.Max.Y) PropogateLightG(chunk, x, (byte)(y + 1), z, lightG);
                if (z > 0) PropogateLightG(chunk, x, y, (byte)(z - 1), lightG);
                if (z < Chunk.Max.Z) PropogateLightG(chunk, x, y, (byte)(z + 1), lightG);

                if (chunk.E != null && x == 0) PropogateLightG(chunk.E, (Chunk.Max.X), y, z, lightG);
                if (chunk.W != null && (x == Chunk.Max.X)) PropogateLightG(chunk.W, 0, y, z, lightG);
                if (chunk.S != null && z == 0) PropogateLightG(chunk.S, x, y, (Chunk.Max.Z), lightG);
                if (chunk.N != null && (z == Chunk.Max.Z)) PropogateLightG(chunk.N, x, y, 0, lightG);
            }
            catch (Exception)
            {
                Debug.WriteLine("PropogateLightG Exception");
            }
        }

        private void PropogateLightB(Chunk chunk, byte x, byte y, byte z, byte lightB)
        {
            try
            {
                var offset = x * Chunk.FlattenOffset + z * Chunk.Size.Y + y;
                if (chunk.Blocks[offset].Id != BlockType.None && chunk.Blocks[offset].Id != BlockType.Water) return;
                if (chunk.Blocks[offset].B >= lightB) return;
                chunk.Blocks[offset].B = lightB;

                if (lightB <= 1) return;
                lightB = Attenuate(lightB);

                if (x > 0) PropogateLightB(chunk, (byte)(x - 1), y, z, lightB);
                if (x < Chunk.Max.X) PropogateLightB(chunk, (byte)(x + 1), y, z, lightB);
                if (y > 0) PropogateLightB(chunk, x, (byte)(y - 1), z, lightB);
                if (y < Chunk.Max.Y) PropogateLightB(chunk, x, (byte)(y + 1), z, lightB);
                if (z > 0) PropogateLightB(chunk, x, y, (byte)(z - 1), lightB);
                if (z < Chunk.Max.Z) PropogateLightB(chunk, x, y, (byte)(z + 1), lightB);

                if (chunk.E != null && x == 0) PropogateLightB(chunk.E, (Chunk.Max.X), y, z, lightB);
                if (chunk.W != null && (x == Chunk.Max.X)) PropogateLightB(chunk.W, 0, y, z, lightB);
                if (chunk.S != null && z == 0) PropogateLightB(chunk.S, x, y, (Chunk.Max.Z), lightB);
                if (chunk.N != null && (z == Chunk.Max.Z)) PropogateLightB(chunk.N, x, y, 0, lightB);
            }
            catch (Exception)
            {
                Debug.WriteLine("PropogateLightB Exception");
            }
        }
        #endregion

        #region FillLighting
        private void FillLighting(Chunk chunk)
        {
            FillLightingSun(chunk);
            FillLightingR(chunk);
            FillLightingG(chunk);
            FillLightingB(chunk);
        }

        private void FillLightingSun(Chunk chunk)
        {

            for (byte x = 0; x < Chunk.Size.X; x++)
            {
                for (byte z = 0; z < Chunk.Size.Z; z++)
                {
                    var offset = x * Chunk.FlattenOffset + z * Chunk.Size.Y; // we don't want this x-z value to be calculated each in in y-loop!
                    //for (byte y = 0; y < Chunk.SIZE.Y; y++)
                    for (var y = chunk.LowestNoneBlock.Y; y < Chunk.Size.Y; y++)
                    {
                        if (chunk.Blocks[offset + y].Id != BlockType.None) continue;
                        // Sunlight
                        if (chunk.Blocks[offset + y].Sun > 1)
                        {
                            var light = Attenuate(chunk.Blocks[offset + y].Sun);

                            if (x > 0) PropogateLightSun(chunk, (byte)(x - 1), y, z, light);
                            if (x < Chunk.Max.X) PropogateLightSun(chunk, (byte)(x + 1), y, z, light);
                            if (y > 0) PropogateLightSun(chunk, x, (byte)(y - 1), z, light);
                            if (y < Chunk.Max.Y) PropogateLightSun(chunk, x, (byte)(y + 1), z, light);
                            if (z > 0) PropogateLightSun(chunk, x, y, (byte)(z - 1), light);
                            if (z < Chunk.Max.Z) PropogateLightSun(chunk, x, y, (byte)(z + 1), light);
                        }

                        // Pull in light from neighbours
                        if (chunk.E!=null && x == 0) PropogateLightSun(chunk, x, y, z, chunk.E.GetBlock(Chunk.Max.X, y, z).Sun);
                        if (chunk.W!=null && x == Chunk.Max.X) PropogateLightSun(chunk, x, y, z, chunk.W.GetBlock(0, y, z).Sun);
                        if (chunk.S!=null && z == 0) PropogateLightSun(chunk, x, y, z, chunk.S.GetBlock(x, y, Chunk.Max.Z).Sun);
                        if (chunk.N!=null && z == Chunk.Max.Z) PropogateLightSun(chunk, x, y, z, chunk.N.GetBlock(x, y, 0).Sun);
                    }
                }
            }

        }

        private void FillLightingR(Chunk chunk)
        {
            try
            {
                for (byte x = 0; x < Chunk.Size.X; x++)
                {
                    for (byte z = 0; z < Chunk.Size.Z; z++)
                    {
                        var offset = x * Chunk.FlattenOffset + z * Chunk.Size.Y; // we don't want this x-z value to be calculated each in in y-loop!
                        //for (byte y = 0; y < Chunk.SIZE.Y; y++)
                        for (var y = chunk.LowestNoneBlock.Y; y < Chunk.Size.Y; y++)
                        {
                            if (chunk.Blocks[offset + y].Id != BlockType.None &&
                                chunk.Blocks[offset + y].Id != BlockType.Tree &&
                                chunk.Blocks[offset + y].Id != BlockType.RedFlower) continue;
                            // Local light R
                            if (chunk.Blocks[offset + y].R > 1)
                            {
                                var light = Attenuate(chunk.Blocks[offset + y].R);

                                if (x > 0) PropogateLightR(chunk, (byte)(x - 1), y, z, light);
                                if (x < Chunk.Max.X) PropogateLightR(chunk, (byte)(x + 1), y, z, light);
                                if (y > 0) PropogateLightR(chunk, x, (byte)(y - 1), z, light);
                                if (y < Chunk.Max.Y) PropogateLightR(chunk, x, (byte)(y + 1), z, light);
                                if (z > 0) PropogateLightR(chunk, x, y, (byte)(z - 1), light);
                                if (z < Chunk.Max.Z) PropogateLightR(chunk, x, y, (byte)(z + 1), light);
                            }

                            // Pull in light from neighbours
                            if (chunk.E!=null && x == 0) PropogateLightR(chunk, x, y, z, chunk.E.GetBlock(Chunk.Max.X, y, z).R);
                            if (chunk.W!=null && x == Chunk.Max.X) PropogateLightR(chunk, x, y, z, chunk.W.GetBlock(0, y, z).R);
                            if (chunk.S!=null && z == 0) PropogateLightR(chunk, x, y, z, chunk.S.GetBlock(x, y, Chunk.Max.Z).R);
                            if (chunk.N!=null && z == Chunk.Max.Z) PropogateLightR(chunk, x, y, z, chunk.N.GetBlock(x, y, 0).R);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine("FillLightingR Exception");
            }
        }

        private void FillLightingG(Chunk chunk)
        {
            try
            {
                for (byte x = 0; x < Chunk.Size.X; x++)
                {
                    for (byte z = 0; z < Chunk.Size.Z; z++)
                    {
                        var offset = x * Chunk.FlattenOffset + z * Chunk.Size.Y; // we don't want this x-z value to be calculated each in in y-loop!
                        //for (byte y = 0; y < Chunk.SIZE.Y; y++)
                        for (var y = chunk.LowestNoneBlock.Y; y < Chunk.Size.Y; y++)
                        {
                            if (chunk.Blocks[offset + y].Id != BlockType.None &&
                                chunk.Blocks[offset + y].Id != BlockType.Tree &&
                                chunk.Blocks[offset + y].Id != BlockType.RedFlower) continue;
                            // Local light G
                            if (chunk.Blocks[offset + y].G > 1)
                            {
                                var light = Attenuate(chunk.Blocks[offset + y].G);
                                if (x > 0) PropogateLightG(chunk, (byte)(x - 1), y, z, light);
                                if (x < Chunk.Max.X) PropogateLightG(chunk, (byte)(x + 1), y, z, light);
                                if (y > 0) PropogateLightG(chunk, x, (byte)(y - 1), z, light);
                                if (y < Chunk.Max.Y) PropogateLightG(chunk, x, (byte)(y + 1), z, light);
                                if (z > 0) PropogateLightG(chunk, x, y, (byte)(z - 1), light);
                                if (z < Chunk.Max.Z) PropogateLightG(chunk, x, y, (byte)(z + 1), light);
                            }

                            // Pull in light from neighbours
                            if (chunk.E!=null && x == 0) PropogateLightG(chunk, x, y, z, chunk.E.GetBlock(Chunk.Max.X, y, z).G);
                            if (chunk.W!=null && x == Chunk.Max.X) PropogateLightG(chunk, x, y, z, chunk.W.GetBlock(0, y, z).G);
                            if (chunk.S!=null && z == 0) PropogateLightG(chunk, x, y, z, chunk.S.GetBlock(x, y, Chunk.Max.Z).G);
                            if (chunk.N!=null && z == Chunk.Max.Z) PropogateLightG(chunk, x, y, z, chunk.N.GetBlock(x, y, 0).G);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine("FillLightingG Exception");
            }
        }

        private void FillLightingB(Chunk chunk)
        {
            try
            {
                for (byte x = 0; x < Chunk.Size.X; x++)
                {
                    for (byte z = 0; z < Chunk.Size.Z; z++)
                    {
                        var offset = x * Chunk.FlattenOffset + z * Chunk.Size.Y; // we don't want this x-z value to be calculated each in in y-loop!
                        //for (byte y = 0; y < Chunk.SIZE.Y; y++)
                        for (var y = chunk.LowestNoneBlock.Y; y < Chunk.Size.Y; y++)
                        {
                            if (chunk.Blocks[offset + y].Id != BlockType.None &&
                                chunk.Blocks[offset + y].Id != BlockType.Tree &&
                                chunk.Blocks[offset + y].Id != BlockType.RedFlower) continue;
                            // Local light B
                            if (chunk.Blocks[offset + y].B > 1)
                            {
                                var light = Attenuate(chunk.Blocks[offset + y].B);
                                if (x > 0) PropogateLightB(chunk, (byte)(x - 1), y, z, light);
                                if (x < Chunk.Max.X) PropogateLightB(chunk, (byte)(x + 1), y, z, light);
                                if (y > 0) PropogateLightB(chunk, x, (byte)(y - 1), z, light);
                                if (y < Chunk.Max.Y) PropogateLightB(chunk, x, (byte)(y + 1), z, light);
                                if (z > 0) PropogateLightB(chunk, x, y, (byte)(z - 1), light);
                                if (z < Chunk.Max.Z) PropogateLightB(chunk, x, y, (byte)(z + 1), light);
                            }

                            // Pull in light from neighbours
                            if (chunk.E!=null && x == 0) PropogateLightB(chunk, x, y, z, chunk.E.GetBlock(Chunk.Max.X, y, z).B);
                            if (chunk.W!=null && x == Chunk.Max.X) PropogateLightB(chunk, x, y, z, chunk.W.GetBlock(0, y, z).B);
                            if (chunk.S!=null && z == 0) PropogateLightB(chunk, x, y, z, chunk.S.GetBlock(x, y, Chunk.Max.Z).B);
                            if (chunk.N!=null && z == Chunk.Max.Z) PropogateLightB(chunk, x, y, z, chunk.N.GetBlock(x, y, 0).B);
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
