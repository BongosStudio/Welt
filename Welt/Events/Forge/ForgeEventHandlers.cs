
using Welt.API;
using Welt.API.Forge;
using Welt.Core.Forge;

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
