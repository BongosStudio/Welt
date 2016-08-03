using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Welt.API.Forge;

namespace Welt.Forge
{
    public class BlockTextureModel
    {
        public ushort Id { get; protected set; }
        public byte Metadata { get; protected set; }
        public int DefaultTexture { get; protected set; }
        public int XIncreasingTexture { get; protected set; }
        public int XDecreasingTexture { get; protected set; }
        public int ZIncreasingTexture { get; protected set; }
        public int ZDecreasingTexture { get; protected set; }
        public int YIncreasingTexture { get; protected set; }
        public int YDecreasingTexture { get; protected set; }

        public static List<BlockTextureModel> Created = new List<BlockTextureModel>(); 
        
        // TODO: consider moving this to Welt.Core? People may need this for mods/plugins.

        protected BlockTextureModel(ushort id, byte md, int deft, int xi, int xd, int zi, int zd, int yi,
            int yd)
        {
            Id = id;
            Metadata = md;
            DefaultTexture = deft;
            XIncreasingTexture = xi;
            XDecreasingTexture = xd;
            ZIncreasingTexture = zi;
            ZDecreasingTexture = zd;
            YIncreasingTexture = yi;
            YDecreasingTexture = yd;
        }

        public static void Create(ushort id, byte md, int text)
        {
            Create(id, md, text, text, text);
        }

        public static void Create(
            ushort id,
            byte md,
            int deft,
            int sides,
            int y
            )
        {
            Create(id, md, deft, sides, y, y);
        }

        public static void Create(
            ushort id, 
            byte md,
            int deft,
            int sides,
            int top,
            int bottom
            )
        {
            Create(id, md, deft, sides, sides, sides, sides, top, bottom);
        }

        public static void Create(
            ushort id, 
            byte md, 
            int deft, 
            int xi, 
            int xd, 
            int zi,
            int zd, 
            int yi, 
            int yd
            )
        {
            Created.Add(new BlockTextureModel(
                id, 
                md, 
                deft, 
                xi, 
                xd,
                zi, 
                zd, 
                yi, 
                yd
                ));
        }

        public static BlockTextureModel Find(ushort id, byte md) => Created.Find(b => b.Id == id && b.Metadata == md);
    }
}