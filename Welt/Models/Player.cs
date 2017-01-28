#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
using System;
using Welt.Entities;
using Welt.Events.Forge;
using Welt.Forge;
using Welt.Logic.Forge;
using Welt.Types;

#endregion

namespace Welt.Models
{
    public class Player
    {
        public static Player Current;
        #region Fields
        public World World;
        public Chunk Chunk
        {
            get
            {
                // we only want this to update every time the chunk is grabbed
                var chunk = World.ChunkAt(Position);
                if (chunk.Index != m_PreviousChunkIndex)
                {
                    m_PreviousChunkIndex = chunk.Index;
                    OnEnteredNewChunk(this, null);
                }
                return chunk;
            }
        }
        public PlayerEntity Entity;

        public Vector3 Position;
        public Vector3 Velocity;        
        public double HeadBob;

        public PositionedBlock? CurrentSelection;
        public PositionedBlock? CurrentSelectedAdjacent; // = where a block would be added with the add tool
        
        // NOTE: should I have a cap set on the hotbar index? It might be kinda cool to see what people
        // could do if I don't set one... Maybe a macro mod/plugin? :o
        public byte HotbarIndex;
        public InventoryContainer Inventory = new InventoryContainer();
        public BlockStack HeldItem => Inventory[HotbarIndex];
        
        public Vector3 TargetPoint;

        public bool IsPaused;
        public bool IsMouseLocked;
        public string Username;
        public string AuthToken;

        private Vector3I m_PreviousChunkIndex;
        
        #endregion

        public static void CreatePlayer(string username, string token)
        {
            var player = new Player
            {
                Username = username,
                AuthToken = token,
                Entity =
                {
                    Stamina = 1,
                    Health = 1
                },
                IsMouseLocked = true
            };
            player.Inventory[0] = new BlockStack(new Block(BlockType.DIRT));
            player.Inventory[1] = new BlockStack(new Block(BlockType.STONE));
            player.Inventory[2] = new BlockStack(new Block(BlockType.LOG));
            player.Inventory[3] = new BlockStack(new Block(BlockType.LEAVES));
            player.Inventory[4] = new BlockStack(new Block(BlockType.FLOWER_ROSE));
            player.Inventory[5] = new BlockStack(new Block(BlockType.TORCH));
            player.Inventory[6] = new BlockStack(new Block(BlockType.LADDER, 0));
            Current = player;
            
        }
       
        public void AssignWorld(World world)
        {
            World = world;
            Entity = new PlayerEntity();
        }

        public bool LeftClick(GameTime time)
        {
            if (!IsMouseLocked) return false;
            if (CurrentSelection == null) return false;
            ForgeEventHandlers.SetBlockHandler(World, CurrentSelection.Value.Position, new Block());
            //World.SetBlock(CurrentSelection.Value.Position, new Block(0, 0));
            return true;
        }

        public bool RightClick(GameTime time)
        {
            if (!IsMouseLocked) return false;
            if (Inventory[HotbarIndex].Block.Id == 0) return false;
            var provider = BlockProvider.GetProvider(Inventory[HotbarIndex].Block.Id);
            if (!CurrentSelection.HasValue || !CurrentSelectedAdjacent.HasValue) return false;
            provider.PlaceBlock(World, CurrentSelection.Value.Position, CurrentSelectedAdjacent.Value.Position, Inventory[HotbarIndex].Block);
            return true;
        }

        public Player()
        {
            Entity = new PlayerEntity();
        }

        public event EventHandler EnteredNewChunk;

        public void OnEnteredNewChunk(object sender, EventArgs args)
        {
            EnteredNewChunk?.Invoke(sender, args);
        }
    }
}
