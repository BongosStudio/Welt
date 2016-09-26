using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Welt.Core.Services
{
    public class BlockService : IWeltService
    {
        private static (
            ushort Id,
            byte Metadata,
            string Name,
            float Hardness,
            float Width,
            float Height,
            float Depth,
            bool IsOpaque,
            bool IsFlammable,
            bool IsReinforced,
            bool HasCollision,
            bool HasPhysics,
            bool HasLifecycle
            )[]_blocks =
        {
            (1, 0, "stone", 0.5f, 1, 1, 1, false, false, false, true, false, false),
            (1, 1, "limestone", 0.5f, 1, 1, 1, false, false, false, true, false, false),

        };

        public static void Initialize()
        {

        }

        public void Load(Assembly assembly)
        {

        }

        public void Unload()
        {

        }

        private static (
            ushort Id, byte Md, string Name, float Hrd, float W, float H, 
            float D, bool O, bool F, bool R, bool C, bool P, bool L) 
            GetBlock(ushort id, byte md)
        {
            return _blocks.Single(b => b.Id == id && b.Metadata == md);
        }

        public static (ushort Id, byte Metadata) GetBlockFromName(string name)
        {
            var b = _blocks.Single(block => block.Name == name);
            return (b.Id, b.Metadata);
        }

        public static string GetBlockName(ushort id, byte metadata) => GetBlock(id, metadata).Name;

        public static float GetHardness(ushort id, byte metadata) => GetBlock(id, metadata).Hrd;

        public static float GetWidth(ushort id, byte metadata) => GetBlock(id, metadata).W;

        public static float GetHeight(ushort id, byte metadata) => GetBlock(id, metadata).H;

        public static float GetDepth(ushort id, byte metadata) => GetBlock(id, metadata).D;
        public static bool GetOpaque(ushort id, byte metadata) => GetBlock(id, metadata).O;
        public static bool GetFlammable(ushort id, byte metadata) => GetBlock(id, metadata).F;
        public static bool GetReinforced(ushort id, byte metadata) => GetBlock(id, metadata).R;
        public static bool GetCollidable(ushort id, byte metadata) => GetBlock(id, metadata).C;
        public static bool GetPhysical(ushort id, byte metadata) => GetBlock(id, metadata).P; // yes I did this on purpose.
        public static bool GetLifecycled(ushort id, byte metadata) => GetBlock(id, metadata).L;
    }
}