using Welt.Forge.BlockProviders;

namespace Welt.Forge.MetadataConverters
{
    public class TorchMetadataConverter : IMetadataConverter<TorchBlockProvider>
    {
        public TorchBlockProvider ConvertBack(byte data)
        {
            return null;
        }

        public byte ConvertTo(TorchBlockProvider block)
        {
            return 0;
        }
    }
}
