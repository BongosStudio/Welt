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

            // Basic Natural Blocks

            (1, 0, "stone", 0.5f, 1, 1, 1, false, false, false, true, false, false),
            (1, 1, "limestone", 0.5f, 1, 1, 1, false, false, false, true, false, false),
            (2, 0, "grass_block", 0.3f, 1, 1, 1, false, false, false, true, false, false),
            (3, 0, "dirt", 0.3f, 1, 1, 1, false, false, false, true, false, true),
            (3, 1, "volcanic_soil", 0.3f, 1, 1, 1, false, false, false, true, false, false),
            (4, 0, "clay", 0.2f, 1, 1, 1, false, false, false, true, false, false),
            (5, 0, "water", 0, 1, 1, 1, true, false, false, false, true, false), // collision is processed elsewhere
            (6, 0, "lava", 0, 1, 1, 1, false, false, false, false, true, false),
            (7, 0, "wood", 0.3f, 1, 1, 1, false, true, false, true, false, false),
            (8, 0, "leaves", 0.1f, 1, 1, 1, true, true, false, true, false, true),

            (20, 0, "snow", 0.01f, 1, 0.1f, 1, false, false, false, false, true, false),

            (30, 0, "rose", 0.01f, 0.2f, 0.4f, 0.2f, false, true, false, false, true, false),
            (31, 0, "grass", 0.01f, 1, 0.4f, 1, false, true, false, false, true, false),
            
            // Minerals

            (100, 0, "iron_ore", 0.65f, 1, 1, 1, false, false, false, true, false, false),
            (101, 0, "coal_ore", 0.6f, 1, 1, 1, false, false, false, true, false, false),
            (102, 0, "sulphur", 0.5f, 1, 1, 1, false, false, false, true, false, false),
            (103, 0, "niter", 0.5f, 1, 1, 1, false, false, false, true, false, false),
            (104, 0, "copper_ore", 0.6f, 1, 1, 1, false, false, false, true, false, false),
            (105, 0, "zinc_ore", 0.6f, 1, 1, 1, false, false, false, true, false, false),
            (106, 0, "titanium_ore", 0.8f, 1, 1, 1, false, false, false, true, false, false),
            (107, 0, "uranium_ore", 0.65f, 1, 1, 1, false, false, false, true, false, false),
            (108, 0, "aluminum", 0.5f, 1, 1, 1, false, false, false, true, false, false),

            // Decoration Blocks

            (200, 0, "glass", 0.01f, 1, 1, 1, true, false, false, true, false, false),
            (201, 0, "plexiglass", 0.5f, 1, 1, 1, true, false, false, true, false, false),

            // Construction Blocks

            (300, 0, "flagstone", 0.5f, 1, 1, 1, false, false, false, true, false, false),

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

        public static (
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