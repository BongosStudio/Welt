using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.Types;

namespace Welt.Forge
{
    /// <summary>
    ///     Contains a map for all light vectors in a chunk.
    /// </summary>
    public class LightMap
    {
        // Main logic for this map is that when traversing horizontally, light calculations
        // are adjusted on a diag plane which only change values on the block. This means
        // that if the light source appears at the top left of a block, bottom right will be
        // TL - 1, top right will be TL - 0.5, bottom left will be TL - 0.5. When traversing 
        // vertically, we MUST calculate the Y first of the block. All Y will be adjusted by
        // light value - 1 then will move across the horizontal plane once it comes into 
        // contact with a solid.

        /// <summary>
        ///     Corners for on which face the light value is at.
        /// </summary>
        public enum LightCorner
        {
            TopLeft, TopRight, BottomLeft, BottomRight
        }

        public int Size;
        private BlockPalette m_BlockPalette;
        private (Vector3B Light, bool IsSource)[] m_Data;

        public LightMap(BlockPalette blocks, int size)
        {
            m_BlockPalette = blocks;
            Size = size;
            m_Data = new(Vector3B Light, bool IsSource)[size*4];
        }

        public Vector3B GetLightAt(int index, LightCorner corner)
        {
            return m_Data[index * 4 + (int)corner].Light;
        }

        public void PlaceLightAt(int index, Vector3B value)
        {
            m_Data[index * 4].IsSource = true;
            var tr = m_Data[index * 4].Light;
            var tl = m_Data[index * 4 + 1].Light;
            var br = m_Data[index * 4 + 2].Light;
            var bl = m_Data[index * 4 + 3].Light;
            if (tr < value) m_Data[index * 4].Light = value;
            if (tl < value) m_Data[index * 4 + 1].Light = value;
            if (br < value) m_Data[index * 4 + 2].Light = value;
            if (bl < value) m_Data[index * 4 + 3].Light = value;
            // should we do propogation and attenuation here?
        }
    }
}
