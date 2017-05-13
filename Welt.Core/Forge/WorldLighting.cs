using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    public class WorldLighting
    {
        private const int MAX_SUN_VALUE = 15;
        private readonly Random _mR = new Random();
        private Vector3B m_L;
        private IBlockRepository m_Repo;

        public WorldLighting(IBlockRepository repo)
        {
            m_Repo = repo;
        }

        public void ProcessChunk(Chunk chunk)
        {
            if (chunk == null) return;
            Task.Run(() =>
            {
                ClearLighting(chunk);
                FillLighting(chunk);
            });
        }

        #region ClearLighting

        private void ClearLighting(Chunk chunk)
        {
            try
            {
                
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

        private void PropogateLightSun(Chunk chunk, uint x, uint y, uint z, byte light)
        {
            
        }

        private void PropogateLightR(Chunk chunk, uint x, uint y, uint z, byte lightR)
        {
            
        }

        private void PropogateLightG(Chunk chunk, uint x, uint y, uint z, byte lightG)
        {
            
        }

        private void PropogateLightB(Chunk chunk, uint x, uint y, uint z, byte lightB)
        {
            
        }
        #endregion

        #region FillLighting
        private void FillLighting(Chunk chunk)
        {
            FillLightingSun(chunk);
            FillLightingR(chunk);
            FillLightingG(chunk);
            FillLightingB(chunk);
            chunk.IsModified = true;
        }

        private void FillLightingSun(Chunk chunk)
        {

            
        }

        private void FillLightingR(Chunk chunk)
        {
           
        }

        private void FillLightingG(Chunk chunk)
        {
            
        }

        private void FillLightingB(Chunk chunk)
        {
            
        }
        #endregion
    }
}
