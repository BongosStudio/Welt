#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Welt.API.Forge.Generators;

namespace Welt.API.Forge
{
    /// <summary>
    ///     Interface for the world object. IWorld objects contains groups of
    ///     chunks and IWorlds are contained within IWorldSystems.
    /// </summary>
    public interface IWorld
    {
        string Name { get; }
        long Seed { get; }
        int Size { get; }
        IForgeGenerator Generator { get; }

        IChunk GetChunk(uint x, uint z);
        void SetChunk(uint x, uint z, IChunk value);
        void RemoveChunk(uint x, uint z);

        Block GetBlock(uint x, uint y, uint z);
        void SetBlock(uint x, uint y, uint z, Block value);
    }
}