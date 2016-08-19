#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Welt.API.Forge
{
    // TODO: take these types out. THEY BELONG IN CORE.
    public class BlockType
    {
        public const ushort None = 0;
        public const ushort Dirt = 1;
        public const ushort Grass = 2;
        public const ushort Lava = 3;
        public const ushort Leaves = 4;
        public const ushort Rock = 5;
        public const ushort Sand = 6;
        public const ushort Tree = 7;
        public const ushort Snow = 8;
        public const ushort RedFlower = 9;
        public const ushort LongGrass = 10;
        public const ushort Torch = 90;
        public const ushort Water = 182;
        public const ushort Maximum = ushort.MaxValue;
    }

    #region Block structure

    public abstract class Block
    {
        public static List<Type> RegisteredTypes = new List<Type>();

        public ushort Id { get; }
        public byte Metadata { get; }

        public virtual float Hardness => 0.5f;
        public virtual float Width => 1;
        public virtual float Depth => 1;
        public virtual float Height => 1;

        public virtual bool Opaque => false;
        public virtual bool IsFlammable => false;
        public virtual bool IsReinforced => false;
        public virtual bool HasCollision => true;
        public virtual bool HasPhysics => false;
        public virtual bool HasLifecycle => false;


        protected Block(ushort blockId, byte metadata)
        {
            Id = blockId;
            Metadata = metadata;
            if (!RegisteredTypes.Contains(GetType())) RegisteredTypes.Add(GetType());
        }

        #region Overrides & Operators

        public static bool operator ==(Block left, Block right)
        {
            return left.Id == right.Id &&
                   left.Metadata == right.Metadata;
        }

        public static bool operator !=(Block left, Block right)
        {
            return !(left == right);
        }

        protected bool Equals(Block other)
        {
            return Id == other.Id && Metadata == other.Metadata;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Block)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ Metadata.GetHashCode();
            }
        }

        #endregion
    }

    #endregion
}