using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.Forge;
using Welt.Types;

namespace Welt.Events.Forge
{
    public class ForgeEventHandlers
    {
        public static void SetBlockHandler(World world, Vector3I position, Block block)
        {
            WeltGame.Instance.TaskManager.ExecuteInBackground(() => world.SetBlock(position, block));
        }
    }
}
