namespace AntiLag
{
    using Exiled.API.Interfaces;

    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int MaxAmmoStackSize { get; set; } = 160;
    }
}