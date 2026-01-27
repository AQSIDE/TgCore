namespace TgCore.Api.Requests.Parameters;

public class ShortParameters : RequestParameters
{
    public override Dictionary<string, object> ToDictionary()
    {
        return new TelegramParametersBuilder()
            .AddDictionary(GetBase())
            .Build();
    }
}