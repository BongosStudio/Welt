#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Welt.API.Forge;

namespace Welt.Core.Forge.Biome
{

    #region enum BiomeType

    public enum BiomeType : byte
    {
        None = 0,

        //  Tundra 
        TundraAlpine = 1,
        TundraArtic = 2,

        //  Grassland
        GrasslandSavanna = 3,
        GrasslandTemperate = 4,

        //  Forest
        ForestTropical = 5,
        ForestTemperate = 6,
        ForestTaiga = 7,

        //  Desert
        DesertSubtropical = 8,
        DesertSemiarid = 9,
        DesertCoastal = 10,
        DesertCold = 11,

        //  Marine
        MarineOcean = 12,
        MarineCoralReef = 13,
        MarineEstuary = 14,

        //  Freshwater
        FreshwaterLake = 15,
        FreshwaterRiver = 16,
        FreshwaterWetland = 17,

        Custom = 18,
        Maximum = 19
    }

    #endregion

    public class Biome
    {
        public byte TemperatureLowest { get; set; }
        public byte TemperatureHighest { get; set; }
        public byte RainfallLowest { get; set; }
        public byte RainfallHighest { get; set; }
        public BlockType Treetype { get; set; }
        public BlockType Topgroundblocktype { get; set; }
        public BlockType Watertype { get; set; }
    }
}