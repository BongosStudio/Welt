#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace Welt.API.Forge
{
    #region Block structure

    /// <summary>
    ///     Represents an object of a space taken up in the world.
    /// </summary>
    public sealed class Block
    {
        /// <summary>
        ///     Gets a list of default blocks.
        /// </summary>
        public static List<Block> RegisteredTypes => new List<Block>
        {
            new Block("stone", 1, 0),
            new Block("grass", 2, 0),
            new Block("dirt", 3, 0),
            new Block("water", 11, 0, hardness: 0, height: 0.9f),
            new Block("lava", 12, 0, hardness: 0, height: 0.9f),
            new Block("rose", 20, 0, hardness: 0.1f, width: 0.2f, depth: 0.2f, height: 0.5f, flammable: true, hasPhysics: true),
            new Block("wood", 30, 0, flammable: true),
            new Block("leaves", 31, 0, hardness: 0.2f, flammable: true, hasLifecycle: true)
        };

        /// <summary>
        ///     Gets the name of the block.
        /// </summary>
        public string Name { get; }
        /// <summary>
        ///     Gets the ID of the block.
        /// </summary>
        public ushort Id { get; }
        /// <summary>
        ///     Gets the Metadata (subclass) of the block.
        /// </summary>
        public byte Metadata { get; }

        /// <summary>
        ///     Gets the hardness of the block. This does not include if a certain tool is being used. 0.5 is default, 0 means it 
        ///     cannot be struck/broken, 0.1 will break immediately, and 5.0 is the hardest possible.
        /// </summary>
        public float Hardness { get; }
        /// <summary>
        ///     Gets the width of the block. Note: this is for collision purposes only.
        /// </summary>
        public float Width { get; }
        /// <summary>
        ///     Gets the depth of the block. Note: this is for collision purposes only.
        /// </summary>
        public float Depth { get; }
        /// <summary>
        ///     Gets the height of the block. Note: this is for collision purposes only.
        /// </summary>
        public float Height { get; }

        /// <summary>
        ///     Gets whether or not the block allows light (both sun and ambient) to pass through.
        /// </summary>
        public bool Opaque { get; }
        /// <summary>
        ///     Gets whether or not the block will catch and burn when fire is near.
        /// </summary>
        public bool IsFlammable { get; }
        /// <summary>
        ///     Gets whether or not the block will withstand outer-atmosphere and deep-space travel.
        /// </summary>
        public bool IsReinforced { get; }
        /// <summary>
        ///     Gets whether or not the block has a collision frame.
        /// </summary>
        public bool HasCollision { get; }
        /// <summary>
        ///     Gets whether or not the block is affected by physics.
        /// </summary>
        public bool HasPhysics { get; }
        /// <summary>
        ///     Gets whether or not the block has a lifecycle.
        /// </summary>
        public bool HasLifecycle { get; }

        /// <summary>
        ///     Creates a new instance of a block with the specified parameters.
        /// </summary>
        /// <param name="name">Name of the block</param>
        /// <param name="blockId">ID of the block</param>
        /// <param name="metadata">Metadata of the block</param>
        /// <param name="hardness">Default hardness of the block</param>
        /// <param name="width">Collision width of the block</param>
        /// <param name="depth">Collision depth of the block</param>
        /// <param name="height">Collision height of the block</param>
        /// <param name="opaque">Whether or not the block is opaque</param>
        /// <param name="flammable">Whether or not the block is flammable</param>
        /// <param name="reinforced">Whether or not the block is reinforced</param>
        /// <param name="hasCollision">Whether or not the block has collision</param>
        /// <param name="hasPhysics">Whether or not the block is affected by the physics engine</param>
        /// <param name="hasLifecycle">Whether or not the block has a lifecycle</param>
        public Block(string name, ushort blockId, byte metadata, 
            float hardness = 0.5f, 
            float width = 1, 
            float depth = 1, 
            float height = 1,
            bool opaque = false,
            bool flammable = false,
            bool reinforced = false,
            bool hasCollision = false,
            bool hasPhysics = false,
            bool hasLifecycle = false)
        {
            Name = name;
            Id = blockId;
            Metadata = metadata;
            Hardness = hardness;
            Width = width;
            Depth = depth;
            Height = height;
            Opaque = opaque;
            IsFlammable = flammable;
            IsReinforced = reinforced;
            HasCollision = hasCollision;
            HasPhysics = hasPhysics;
            HasLifecycle = hasLifecycle;
        }

        public static void Initialize()
        {
            
        }

        /// <summary>
        ///     Creates a new block and adds it to the list of registered types, then returns the block object.
        /// </summary>
        /// <example>
        ///     public static Block TestBlock = Block.Create("test_block", 9283, 3, hardness: 0.8f, opaque: true);
        /// </example>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="md"></param>
        /// <param name="hardness"></param>
        /// <param name="width"></param>
        /// <param name="depth"></param>
        /// <param name="height"></param>
        /// <param name="opaque"></param>
        /// <param name="flammable"></param>
        /// <param name="reinforced"></param>
        /// <param name="hasCollision"></param>
        /// <param name="hasPhysics"></param>
        /// <param name="hasLifecycle"></param>
        /// <returns></returns>
        public static Block Create(string name, ushort id, byte md, 
            float hardness = 0.5f,
            float width = 1,
            float depth = 1,
            float height = 1,
            bool opaque = false,
            bool flammable = false,
            bool reinforced = false,
            bool hasCollision = false,
            bool hasPhysics = false,
            bool hasLifecycle = false)
        {
            var b = new Block(name, id, md, hardness, width, depth, height, opaque, flammable, reinforced,
                hasCollision, hasPhysics, hasLifecycle);
            RegisteredTypes.Add(b);
            return b;
        }

        public static Block Get(string name)
        {
            return RegisteredTypes.Find(b => b.Name == name);
        }

        public static Block Get(ushort id, byte md)
        {
            return RegisteredTypes.Find(b => b.Id == id && b.Metadata == md);
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
                return (Id.GetHashCode()*397) ^ Metadata.GetHashCode();
            }
        }

        #endregion
    }

    #endregion
}