#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
namespace Welt.Entities
{
    public class BlockEntity : Entity
    {

        public override EntityClass EntityClass => EntityClass.Block;

        public readonly ushort Id;
        public byte Count;

        public BlockEntity(ushort id)
        {
            Id = id;
            Count = 1;
        }
    }
}