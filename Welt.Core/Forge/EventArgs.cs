using System;

namespace Welt.Core.Forge
{
    public class BlockChangedEventArgs : EventArgs
    {
        public readonly int X, Y, Z;

        public BlockChangedEventArgs(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public BlockChangedEventArgs(uint x, uint y, uint z)
        {
            X = (int) x;
            Y = (int) y;
            Z = (int) z;
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