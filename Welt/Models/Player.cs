#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
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
        public string Username;
        public string AuthToken;
        
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
                }
            };
            player.Inventory[0] = new BlockStack(new Block(BlockType.DIRT));
            player.Inventory[1] = new BlockStack(new Block(BlockType.ROCK));
            player.Inventory[2] = new BlockStack(new Block(BlockType.LOG));
            player.Inventory[3] = new BlockStack(new Block(BlockType.LEAVES));
            player.Inventory[4] = new BlockStack(new Block(BlockType.RED_FLOWER));
            player.Inventory[5] = new BlockStack(new Block(BlockType.TORCH));
            player.Inventory[6] = new BlockStack(new Block(BlockType.TORCH, 1));
            player.Inventory[7] = new BlockStack(new Block(BlockType.TORCH, 2));
            player.Inventory[8] = new BlockStack(new Block(BlockType.TORCH, 3));
            player.Inventory[9] = new BlockStack(new Block(BlockType.TORCH, 4));
            Current = player;
            
        }
       
        public void AssignWorld(World world)
        {
            World = world;
            Entity = new PlayerEntity();
        }

        public bool LeftClick(GameTime time)
        {
            if (CurrentSelection == null) return false;
            ForgeEventHandlers.SetBlockHandler(World, CurrentSelection.Value.Position, new Block());
            //World.SetBlock(CurrentSelection.Value.Position, new Block(0, 0));
            return true;
        }

        public bool RightClick(GameTime time)
        {
            if (Inventory[HotbarIndex].Block.Id == 0) return false;
            if (CurrentSelectedAdjacent != null &&
                !BlockLogic.GetRightClick(World, CurrentSelectedAdjacent.Value.Position, this) && 
                Block.CanPlaceAt(
                    Inventory[HotbarIndex].Block.Id, 
                    World.GetBlock((Vector3) CurrentSelectedAdjacent.Value.Position + new Vector3(0, -1, 0)).Id, 
                    CurrentSelectedAdjacent.Value.Block.Id))
            {

                World.SetBlock(
                    BlockLogic.DetermineTarget(World, CurrentSelection.Value.Position,
                    CurrentSelectedAdjacent.Value.Position), 
                    Inventory[HotbarIndex].Block);
            }
            return true;
        }

        public Player()
        {
            Entity = new PlayerEntity();
        }
        
    }
}
