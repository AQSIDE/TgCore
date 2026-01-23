namespace TgCore.Api.Data;

public class LifetimeOptions
{
    public ILifetimeModule Module { get; set; }
    public TimeSpan DefaultTime { get; set; }

    public LifetimeOptions(ILifetimeModule module, TimeSpan? defaultTime = null)
    {
        Module = module;
        DefaultTime = defaultTime ?? TimeSpan.FromSeconds(5);
    }
}