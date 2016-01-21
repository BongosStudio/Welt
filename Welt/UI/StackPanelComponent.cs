#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.UI
{
    public class StackPanelComponent : UIComponent
    {
        // we will not allow stack panels to have a parent component just because it makes
        // no sense.
        public StackPanelComponent(string name, int width, int height, GraphicsDevice device)
            : base(name, width, height, device)
        {

        }

        public override void Draw(GameTime time)
        {
            
        }
    }
}