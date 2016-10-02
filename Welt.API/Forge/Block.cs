namespace Welt.API.Forge
{
    public abstract class Block
    {
        public ushort Id;
        public byte Metadata;
        public string Name;

        public float Hardness;
        public float Width;
        public float Height;
        public float Depth;

        public bool IsOpaque;
        public bool IsFlammable;

        public bool HasCollision;
        public bool HasPhysics;
        public bool HasLifecycle;

        public (byte R, byte G, byte B) LightLevel;

        protected Block(
            ushort id, 
            byte metadata, 
            string name, 
            float hardness, 
            float width,
            float height,
            float depth,
            bool isOpaque,
            bool isFlammable,
            bool hasCollision,
            bool hasPhysics,
            bool hasLifecycle,
            byte rLight, byte gLight, byte bLight)
        {
            Id = id;
            Metadata = metadata;
            Name = name;
            Hardness = hardness;
            Width = width;
            Depth = depth;
            Height = height;
            IsOpaque = isOpaque;
            IsFlammable = isFlammable;
            HasCollision = hasCollision;
            HasPhysics = hasPhysics;
            LightLevel = (rLight, gLight, bLight);
        }

        public static bool operator ==(Block left, Block right)
        {
            return left.Id == right.Id && left.Metadata == right.Metadata;
        }

        public static bool operator !=(Block left, Block right)
        {
            return left.Id != right.Id || left.Metadata != right.Metadata;
        }
    }
}
