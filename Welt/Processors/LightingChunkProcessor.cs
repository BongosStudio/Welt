#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Diagnostics;
using Welt.Forge;
using Welt.Models;

#endregion

namespace Welt.Processors
{
    public class LightingChunkProcessor : IChunkProcessor
    {
        private const int MAX_SUN_VALUE = 16;
        private Random r = new Random();

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

                for (byte x = 0; x < Chunk.SIZE.X; x++)
                {
                    for (byte z = 0; z < Chunk.SIZE.Z; z++)
                    {
                        var offset = x * Chunk.FlattenOffset + z * Chunk.SIZE.Y; // we don't want this x-z value to be calculated each in in y-loop!
                        var inShade = false;
                        //for (byte y = Chunk.MAX.Y; y > 0; y--)
                        for (var y = Chunk.MAX.Y; y > chunk.lowestNoneBlock.Y; y--)
                        {
                            if (chunk.Blocks[offset + y].Type != BlockType.None) inShade = true;
                            if (!inShade)
                            {
                                chunk.Blocks[offset + y].Sun = sunValue;
                            }
                            else
                            {
                                chunk.Blocks[offset + y].Sun = 0;
                            }

                            if (chunk.Blocks[offset + y].Type == BlockType.RedFlower)
                            {
                                chunk.Blocks[offset + y].R = (byte)r.Next(17);
                                chunk.Blocks[offset + y].G = (byte)r.Next(17);
                                chunk.Blocks[offset + y].B = (byte)r.Next(17);
                            }
                            else
                            {
                                chunk.Blocks[offset + y].R = 0;
                                chunk.Blocks[offset + y].G = 0;
                                chunk.Blocks[offset + y].B = 0;
                            }
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
                var offset = x * Chunk.FlattenOffset + z * Chunk.SIZE.Y + y;
                if (chunk.Blocks[offset].Type != BlockType.None && chunk.Blocks[offset].Type != BlockType.Water) return;
                if (chunk.Blocks[offset].Sun >= light) return;
                chunk.Blocks[offset].Sun = light;

                if (light > 1)
                {
                    light = Attenuate(light);

                    // Propogate light within this chunk
                    if (x > 0) PropogateLightSun(chunk, (byte)(x - 1), y, z, light);
                    if (x < Chunk.MAX.X) PropogateLightSun(chunk, (byte)(x + 1), y, z, light);
                    if (y > 0) PropogateLightSun(chunk, x, (byte)(y - 1), z, light);
                    if (y < Chunk.MAX.Y) PropogateLightSun(chunk, x, (byte)(y + 1), z, light);
                    if (z > 0) PropogateLightSun(chunk, x, y, (byte)(z - 1), light);
                    if (z < Chunk.MAX.Z) PropogateLightSun(chunk, x, y, (byte)(z + 1), light);

                    //if (chunk.E == null || chunk.W == null || chunk.S == null || chunk.N == null)
                    //{
                    //    throw new Exception("LIGHTING ISSUE");
                    //}

                    if (chunk.E != null && x == 0) PropogateLightSun(chunk.E, (byte)(Chunk.MAX.X), y, z, light);
                    if (chunk.W != null && (x == Chunk.MAX.X)) PropogateLightSun(chunk.W, 0, y, z, light);
                    if (chunk.S != null && z == 0) PropogateLightSun(chunk.S, x, y, (byte)(Chunk.MAX.Z), light);
                    if (chunk.N != null && (z == Chunk.MAX.Z)) PropogateLightSun(chunk.N, x, y, 0, light);
                }
        }

        private void PropogateLightR(Chunk chunk, byte x, byte y, byte z, byte lightR)
        {
            try
            {
                var offset = x * Chunk.FlattenOffset + z * Chunk.SIZE.Y + y;
                if (chunk.Blocks[offset].Type != BlockType.None && chunk.Blocks[offset].Type != BlockType.Water) return;
                if (chunk.Blocks[offset].R >= lightR) return;
                chunk.Blocks[offset].R = lightR;
                if (chunk.State > ChunkState.Lighting) chunk.State = ChunkState.AwaitingBuild;
                if (lightR > 1)
                {
                    lightR = Attenuate(lightR);

                    if (x > 0) PropogateLightR(chunk, (byte)(x - 1), y, z, lightR);
                    if (x < Chunk.MAX.X) PropogateLightR(chunk, (byte)(x + 1), y, z, lightR);
                    if (y > 0) PropogateLightR(chunk, x, (byte)(y - 1), z, lightR);
                    if (y < Chunk.MAX.Y) PropogateLightR(chunk, x, (byte)(y + 1), z, lightR);
                    if (z > 0) PropogateLightR(chunk, x, y, (byte)(z - 1), lightR);
                    if (z < Chunk.MAX.Z) PropogateLightR(chunk, x, y, (byte)(z + 1), lightR);

                    if (chunk.E != null && x == 0) PropogateLightR(chunk.E, (byte)(Chunk.MAX.X), y, z, lightR);
                    if (chunk.W != null && (x == Chunk.MAX.X)) PropogateLightR(chunk.W, 0, y, z, lightR);
                    if (chunk.S != null && z == 0) PropogateLightR(chunk.S, x, y, (byte)(Chunk.MAX.Z), lightR);
                    if (chunk.N != null && (z == Chunk.MAX.Z)) PropogateLightR(chunk.N, x, y, 0, lightR);
                }
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
                var offset = x * Chunk.FlattenOffset + z * Chunk.SIZE.Y + y;
                if (chunk.Blocks[offset].Type != BlockType.None && chunk.Blocks[offset].Type != BlockType.Water) return;
                if (chunk.Blocks[offset].G >= lightG) return;
                chunk.Blocks[offset].G = lightG;

                if (lightG > 1)
                {
                    lightG = Attenuate(lightG);
                    if (x > 0) PropogateLightG(chunk, (byte)(x - 1), y, z, lightG);
                    if (x < Chunk.MAX.X) PropogateLightG(chunk, (byte)(x + 1), y, z, lightG);
                    if (y > 0) PropogateLightG(chunk, x, (byte)(y - 1), z, lightG);
                    if (y < Chunk.MAX.Y) PropogateLightG(chunk, x, (byte)(y + 1), z, lightG);
                    if (z > 0) PropogateLightG(chunk, x, y, (byte)(z - 1), lightG);
                    if (z < Chunk.MAX.Z) PropogateLightG(chunk, x, y, (byte)(z + 1), lightG);

                    if (chunk.E != null && x == 0) PropogateLightG(chunk.E, (byte)(Chunk.MAX.X), y, z, lightG);
                    if (chunk.W != null && (x == Chunk.MAX.X)) PropogateLightG(chunk.W, 0, y, z, lightG);
                    if (chunk.S != null && z == 0) PropogateLightG(chunk.S, x, y, (byte)(Chunk.MAX.Z), lightG);
                    if (chunk.N != null && (z == Chunk.MAX.Z)) PropogateLightG(chunk.N, x, y, 0, lightG);
                }
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
                var offset = x * Chunk.FlattenOffset + z * Chunk.SIZE.Y + y;
                if (chunk.Blocks[offset].Type != BlockType.None && chunk.Blocks[offset].Type != BlockType.Water) return;
                if (chunk.Blocks[offset].B >= lightB) return;
                chunk.Blocks[offset].B = lightB;

                if (lightB > 1)
                {
                    lightB = Attenuate(lightB);

                    if (x > 0) PropogateLightB(chunk, (byte)(x - 1), y, z, lightB);
                    if (x < Chunk.MAX.X) PropogateLightB(chunk, (byte)(x + 1), y, z, lightB);
                    if (y > 0) PropogateLightB(chunk, x, (byte)(y - 1), z, lightB);
                    if (y < Chunk.MAX.Y) PropogateLightB(chunk, x, (byte)(y + 1), z, lightB);
                    if (z > 0) PropogateLightB(chunk, x, y, (byte)(z - 1), lightB);
                    if (z < Chunk.MAX.Z) PropogateLightB(chunk, x, y, (byte)(z + 1), lightB);

                    if (chunk.E != null && x == 0) PropogateLightB(chunk.E, (byte)(Chunk.MAX.X), y, z, lightB);
                    if (chunk.W != null && (x == Chunk.MAX.X)) PropogateLightB(chunk.W, 0, y, z, lightB);
                    if (chunk.S != null && z == 0) PropogateLightB(chunk.S, x, y, (byte)(Chunk.MAX.Z), lightB);
                    if (chunk.N != null && (z == Chunk.MAX.Z)) PropogateLightB(chunk.N, x, y, 0, lightB);
                }
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

            for (byte x = 0; x < Chunk.SIZE.X; x++)
            {
                for (byte z = 0; z < Chunk.SIZE.Z; z++)
                {
                    var offset = x * Chunk.FlattenOffset + z * Chunk.SIZE.Y; // we don't want this x-z value to be calculated each in in y-loop!
                    //for (byte y = 0; y < Chunk.SIZE.Y; y++)
                    for (var y = chunk.lowestNoneBlock.Y; y < Chunk.SIZE.Y; y++)
                    {
                        if (chunk.Blocks[offset + y].Type == BlockType.None)
                        {
                            // Sunlight
                            if (chunk.Blocks[offset + y].Sun > 1)
                            {
                                var light = Attenuate(chunk.Blocks[offset + y].Sun);

                                if (x > 0) PropogateLightSun(chunk, (byte)(x - 1), y, z, light);
                                if (x < Chunk.MAX.X) PropogateLightSun(chunk, (byte)(x + 1), y, z, light);
                                if (y > 0) PropogateLightSun(chunk, x, (byte)(y - 1), z, light);
                                if (y < Chunk.MAX.Y) PropogateLightSun(chunk, x, (byte)(y + 1), z, light);
                                if (z > 0) PropogateLightSun(chunk, x, y, (byte)(z - 1), light);
                                if (z < Chunk.MAX.Z) PropogateLightSun(chunk, x, y, (byte)(z + 1), light);
                            }

                            // Pull in light from neighbours
                            if (chunk.E!=null && x == 0) PropogateLightSun(chunk, x, y, z, chunk.E.BlockAt(Chunk.MAX.X, y, z).Sun);
                            if (chunk.W!=null && x == Chunk.MAX.X) PropogateLightSun(chunk, x, y, z, chunk.W.BlockAt(0, y, z).Sun);
                            if (chunk.S!=null && z == 0) PropogateLightSun(chunk, x, y, z, chunk.S.BlockAt(x, y, Chunk.MAX.Z).Sun);
                            if (chunk.N!=null && z == Chunk.MAX.Z) PropogateLightSun(chunk, x, y, z, chunk.N.BlockAt(x, y, 0).Sun);
                        }
                    }
                }
            }

        }

        private void FillLightingR(Chunk chunk)
        {
            try
            {
                for (byte x = 0; x < Chunk.SIZE.X; x++)
                {
                    for (byte z = 0; z < Chunk.SIZE.Z; z++)
                    {
                        var offset = x * Chunk.FlattenOffset + z * Chunk.SIZE.Y; // we don't want this x-z value to be calculated each in in y-loop!
                        //for (byte y = 0; y < Chunk.SIZE.Y; y++)
                        for (var y = chunk.lowestNoneBlock.Y; y < Chunk.SIZE.Y; y++)
                        {
                            if (chunk.Blocks[offset + y].Type == BlockType.None || chunk.Blocks[offset + y].Type == BlockType.Tree || chunk.Blocks[offset + y].Type == BlockType.RedFlower)
                            {
                                // Local light R
                                if (chunk.Blocks[offset + y].R > 1)
                                {
                                    var light = Attenuate(chunk.Blocks[offset + y].R);

                                    if (x > 0) PropogateLightR(chunk, (byte)(x - 1), y, z, light);
                                    if (x < Chunk.MAX.X) PropogateLightR(chunk, (byte)(x + 1), y, z, light);
                                    if (y > 0) PropogateLightR(chunk, x, (byte)(y - 1), z, light);
                                    if (y < Chunk.MAX.Y) PropogateLightR(chunk, x, (byte)(y + 1), z, light);
                                    if (z > 0) PropogateLightR(chunk, x, y, (byte)(z - 1), light);
                                    if (z < Chunk.MAX.Z) PropogateLightR(chunk, x, y, (byte)(z + 1), light);
                                }

                                // Pull in light from neighbours
                                if (chunk.E!=null && x == 0) PropogateLightR(chunk, x, y, z, chunk.E.BlockAt(Chunk.MAX.X, y, z).R);
                                if (chunk.W!=null && x == Chunk.MAX.X) PropogateLightR(chunk, x, y, z, chunk.W.BlockAt(0, y, z).R);
                                if (chunk.S!=null && z == 0) PropogateLightR(chunk, x, y, z, chunk.S.BlockAt(x, y, Chunk.MAX.Z).R);
                                if (chunk.N!=null && z == Chunk.MAX.Z) PropogateLightR(chunk, x, y, z, chunk.N.BlockAt(x, y, 0).R);
                            }
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
                for (byte x = 0; x < Chunk.SIZE.X; x++)
                {
                    for (byte z = 0; z < Chunk.SIZE.Z; z++)
                    {
                        var offset = x * Chunk.FlattenOffset + z * Chunk.SIZE.Y; // we don't want this x-z value to be calculated each in in y-loop!
                        //for (byte y = 0; y < Chunk.SIZE.Y; y++)
                        for (var y = chunk.lowestNoneBlock.Y; y < Chunk.SIZE.Y; y++)
                        {
                            if (chunk.Blocks[offset + y].Type == BlockType.None || chunk.Blocks[offset + y].Type == BlockType.Tree || chunk.Blocks[offset + y].Type == BlockType.RedFlower)
                            {
                                // Local light G
                                if (chunk.Blocks[offset + y].G > 1)
                                {
                                    var light = Attenuate(chunk.Blocks[offset + y].G);
                                    if (x > 0) PropogateLightG(chunk, (byte)(x - 1), y, z, light);
                                    if (x < Chunk.MAX.X) PropogateLightG(chunk, (byte)(x + 1), y, z, light);
                                    if (y > 0) PropogateLightG(chunk, x, (byte)(y - 1), z, light);
                                    if (y < Chunk.MAX.Y) PropogateLightG(chunk, x, (byte)(y + 1), z, light);
                                    if (z > 0) PropogateLightG(chunk, x, y, (byte)(z - 1), light);
                                    if (z < Chunk.MAX.Z) PropogateLightG(chunk, x, y, (byte)(z + 1), light);
                                }

                                // Pull in light from neighbours
                                if (chunk.E!=null && x == 0) PropogateLightG(chunk, x, y, z, chunk.E.BlockAt(Chunk.MAX.X, y, z).G);
                                if (chunk.W!=null && x == Chunk.MAX.X) PropogateLightG(chunk, x, y, z, chunk.W.BlockAt(0, y, z).G);
                                if (chunk.S!=null && z == 0) PropogateLightG(chunk, x, y, z, chunk.S.BlockAt(x, y, Chunk.MAX.Z).G);
                                if (chunk.N!=null && z == Chunk.MAX.Z) PropogateLightG(chunk, x, y, z, chunk.N.BlockAt(x, y, 0).G);
                            }
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
                for (byte x = 0; x < Chunk.SIZE.X; x++)
                {
                    for (byte z = 0; z < Chunk.SIZE.Z; z++)
                    {
                        var offset = x * Chunk.FlattenOffset + z * Chunk.SIZE.Y; // we don't want this x-z value to be calculated each in in y-loop!
                        //for (byte y = 0; y < Chunk.SIZE.Y; y++)
                        for (var y = chunk.lowestNoneBlock.Y; y < Chunk.SIZE.Y; y++)
                        {
                            if (chunk.Blocks[offset + y].Type == BlockType.None || chunk.Blocks[offset + y].Type == BlockType.Tree || chunk.Blocks[offset + y].Type == BlockType.RedFlower)
                            {
                                // Local light B
                                if (chunk.Blocks[offset + y].B > 1)
                                {
                                    var light = Attenuate(chunk.Blocks[offset + y].B);
                                    if (x > 0) PropogateLightB(chunk, (byte)(x - 1), y, z, light);
                                    if (x < Chunk.MAX.X) PropogateLightB(chunk, (byte)(x + 1), y, z, light);
                                    if (y > 0) PropogateLightB(chunk, x, (byte)(y - 1), z, light);
                                    if (y < Chunk.MAX.Y) PropogateLightB(chunk, x, (byte)(y + 1), z, light);
                                    if (z > 0) PropogateLightB(chunk, x, y, (byte)(z - 1), light);
                                    if (z < Chunk.MAX.Z) PropogateLightB(chunk, x, y, (byte)(z + 1), light);
                                }

                                // Pull in light from neighbours
                                if (chunk.E!=null && x == 0) PropogateLightB(chunk, x, y, z, chunk.E.BlockAt(Chunk.MAX.X, y, z).B);
                                if (chunk.W!=null && x == Chunk.MAX.X) PropogateLightB(chunk, x, y, z, chunk.W.BlockAt(0, y, z).B);
                                if (chunk.S!=null && z == 0) PropogateLightB(chunk, x, y, z, chunk.S.BlockAt(x, y, Chunk.MAX.Z).B);
                                if (chunk.N!=null && z == Chunk.MAX.Z) PropogateLightB(chunk, x, y, z, chunk.N.BlockAt(x, y, 0).B);
                            }
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
