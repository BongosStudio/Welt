#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
using Welt.API.Forge;
using Welt.Core.Forge;
using Welt.Entities;
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
        public InventoryContainer Inventory => Entity.Inventory;
        
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
            World.SetBlock(CurrentSelection.Value.Position.X, CurrentSelection.Value.Position.Y, CurrentSelection.Value.Position.Z, 0, 0);
            return true;
        }

        public bool RightClick(GameTime time)
        {
            if (Inventory[HotbarIndex].Block.Id == 0) return false;
            if (CurrentSelectedAdjacent != null &&
                !BlockLogic.GetRightClick(World, CurrentSelectedAdjacent.Value.Position, this))
            {

                World.SetBlock(
                    BlockLogic.DetermineTarget(World, CurrentSelection.Value.Position,
                        CurrentSelectedAdjacent.Value.Position), Inventory[HotbarIndex].Block);
            }
            return true;
        }

        public Player()
        {
            Entity = new PlayerEntity();
        }
        
    }
}
