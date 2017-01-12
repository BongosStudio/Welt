#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements



#endregion

namespace Welt.Models
{
    public enum ChunkState
    {
        AwaitingGenerate,
        Generating,
        AwaitingLighting,
        Lighting,
        AwaitingBuild,
        Building,
        Ready,
        AwaitingRelighting,
        AwaitingRebuild,
        ToDelete
    }
}
