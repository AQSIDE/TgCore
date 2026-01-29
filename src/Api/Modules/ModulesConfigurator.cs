namespace TgCore.Api.Modules;

public class ModulesConfigurator
{
    private readonly TelegramBot _bot;

    private IRateLimitModule? _rateLimitModule;
    private ILifetimeModule? _lifetimeModule;
    private ITextFormatterModule? _textFormatterModule;
    private ITemporaryMessageLimiterModule? _temporaryMessageLimiterModule;

    private bool _useRateLimit;
    private bool _useLifetime;
    private bool _useTextFormatter;
    private bool _useTemporaryMessageLimiter;

    public ModulesConfigurator(TelegramBot bot)
    {
        _bot = bot;
    }

    public ModulesConfigurator UseRateLimit(IRateLimitModule? module = null)
    {
        _useRateLimit = true;
        _rateLimitModule = module;
        return this;
    }

    public ModulesConfigurator UseLifetime(ILifetimeModule? module = null)
    {
        _useLifetime = true;
        _lifetimeModule = module;
        return this;
    }
    
    public ModulesConfigurator UseTextFormatter(ITextFormatterModule? module = null)
    {
        _useTextFormatter = true;
        _textFormatterModule = module;
        return this;
    }

    public ModulesConfigurator UseTemporaryMessageLimiter(ITemporaryMessageLimiterModule? module = null)
    {
        _useTemporaryMessageLimiter = true;
        _temporaryMessageLimiterModule = module;
        return this;
    }

    public void Apply()
    {
        if (_useTextFormatter)
            _bot.Options.TextFormatter = _textFormatterModule ?? new TextFormatterModule();
        
        if (_useLifetime)
            _bot.Options.Lifetime = _lifetimeModule ?? new LifetimeModule(_bot, _bot.MainLoop);

        if (_useRateLimit)
            _bot.Options.RateLimit = _rateLimitModule ?? new RateLimitModule();

        if (_useTemporaryMessageLimiter)
        {
            _bot.Options.TemporaryMessageLimiter 
                = _temporaryMessageLimiterModule ?? new TemporaryMessageLimiterModule(lifetimeModule: _bot.Options.Lifetime);
        }
    }
}