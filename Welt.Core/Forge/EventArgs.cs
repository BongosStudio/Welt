using System;

namespace Welt.Core.Forge
{
    public class BlockChangedEventArgs : EventArgs
    {
        public readonly uint X, Y, Z;

        public BlockChangedEventArgs(uint x, uint y, uint z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class ChunkChangedEventArgs : EventArgs
    {
        public readonly uint X, Z;
        public readonly ChunkChangedAction ChangedAction;

        public ChunkChangedEventArgs(uint x, uint z, ChunkChangedAction action)
        {
            X = x;
            Z = z;
            ChangedAction = action;
        }

        public enum ChunkChangedAction
        {
            Created,
            Built,
            Adjusted,
            Destroyed
        }
    }
}