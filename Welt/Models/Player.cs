#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
using Welt.Forge;
using Welt.Tools;
using Welt.Types;

#endregion

namespace Welt.Models
{
    public class Player
    {

        #region Fields
        public readonly World World;

        public Vector3 Position;
        public Vector3 Velocity;        
        public double HeadBob;

        public PositionedBlock? CurrentSelection;
        public PositionedBlock? CurrentSelectedAdjacent; // = where a block would be added with the add tool

        public Tool LeftTool;
        public Tool RightTool;

        public Tool AutoTool;
        public Vector3 TargetPoint;

        public bool IsInWater;

        //keep it stupid simple for now, left hand/mousebutton & right hand/mousebutton
        #endregion

        public Player(World world)
        {
            this.World = world;
            LeftTool = new BlockRemover(this);
            //LeftTool = new PowerDrill(this);
            RightTool = new BlockAdder(this);
            //RightTool = new BlockDebugger(this);
        }

    }
}
