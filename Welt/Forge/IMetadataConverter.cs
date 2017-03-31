namespace Welt.Forge
{
    public interface IMetadataConverter<TBlockProvider> where TBlockProvider : BlockProvider
    {
        byte ConvertTo(TBlockProvider block);
        TBlockProvider ConvertBack(byte data);
    }
}
