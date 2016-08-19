namespace Welt.API.Forge
{
    public interface ILightPalette
    {
        LightStruct GetLightAt(int x, int y, int z);
        void SetLightAt(int x, int y, int z, LightStruct value);

        void FillPalette();
        void Update();
    }
}