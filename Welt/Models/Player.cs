#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
using Welt.Entities;
using Welt.Forge;
<<<<<<< HEAD
=======
using Welt.Tools;
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
using Welt.Types;

#endregion

namespace Welt.Models
{
    public class Player
    {
<<<<<<< HEAD
        public static Player Current;
        #region Fields
        public World World;
        public PlayerEntity Entity;
=======

        #region Fields
        public readonly World World;
        public readonly PlayerEntity Entity;
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5

        public Vector3 Position;
        public Vector3 Velocity;        
        public double HeadBob;

        public PositionedBlock? CurrentSelection;
        public PositionedBlock? CurrentSelectedAdjacent; // = where a block would be added with the add tool

<<<<<<< HEAD
        // NOTE: should I have a cap set on the hotbar index? It might be kinda cool to see what people
        // could do if I don't set one... Maybe a macro mod/plugin? :o
        public byte HotbarIndex;
        public InventoryContainer Inventory;
        
=======
        public Tool LeftTool;
        public Tool RightTool;

        public Tool AutoTool;
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        public Vector3 TargetPoint;

        public bool IsPaused;
        public string Username;
<<<<<<< HEAD
        public string AuthToken;
        
        #endregion

        public static void CreatePlayer(string username, string token)
        {
            var player = new Player
            {
                Username = username,
                AuthToken = token
            };
            Current = player;
        }
       
        public void AssignWorld(World world)
        {
            World = world;
            Entity = new PlayerEntity();
            Inventory = new InventoryContainer();
        }

        public bool LeftClick(GameTime time)
        {
            if (CurrentSelection == null) return false;
            World.SetBlock(CurrentSelection.Value.Position, new Block());
            return true;
        }

        public bool RightClick(GameTime time)
        {
            if (Inventory[HotbarIndex].Block.Id == 0) return false; // TODO: use block
            if (CurrentSelectedAdjacent != null)
                World.SetBlock(CurrentSelectedAdjacent.Value.Position, Inventory[HotbarIndex].Block);
            return true;
        }
=======

        //keep it stupid simple for now, left hand/mousebutton & right hand/mousebutton
        #endregion

        public Player(World world)
        {
            World = world;
            LeftTool = new BlockRemover(this);
            //LeftTool = new PowerDrill(this);
            RightTool = new TorchTool(this);
            Entity = new PlayerEntity();
        }

>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
    }
}
