namespace CodeMagic.Core.Objects
{
    public interface IFuelObject : IMapObject
    {
        bool CanIgnite { get; }

        int FuelLeft { get; set; }

        int BurnTemperature { get; }

        int IgnitionTemperature { get; }
    }
}