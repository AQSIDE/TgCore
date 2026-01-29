using TgCore.Api.Requests.Parameters;
using TgCore.Api.Types.Poll;

namespace TgCore.Api.Requests;

public partial class TelegramRequests
{
    public async Task<Message?> SendPoll(long chatId, 
        string question, 
        InputPollOption[] options, 
        int openPeriod = 30, 
        PollType type = PollType.Regular,
        int? correctOptionId = null, 
        bool isAnonymous = false, 
        bool allowsMultipleAnswers = false,
        string? explanation = null,
        IKeyboardMarkup? keyboard = null,
        ParseMode? parseMode = null,
        DefaultParameters? defaultParameters = null)
    {
        try
        {
            await ApplyRateLimit();
            
            if (type == PollType.Quiz)
            {
                if (!correctOptionId.HasValue || correctOptionId < 0 || correctOptionId >= options.Length)
                    throw new ArgumentException("correctOptionId must be set and within options array bounds for Quiz type.");
            }
            
            var pm = parseMode ?? _bot.Options.DefaultParseMode;
            var parameters = new TelegramParametersBuilder()
                .Add("chat_id", chatId)
                .Add("question", TextFormatter?.Process(question, pm) ?? question)
                .Add("options", options)
                .Add("type", BotHelper.GetPollType(type))
                .Add("open_period", openPeriod)
                .Add("is_anonymous", isAnonymous)
                .Add("allows_multiple_answers", allowsMultipleAnswers)
                .Add("correct_option_id", correctOptionId)
                .Add("explanation", string.IsNullOrEmpty(explanation) ? null : TextFormatter?.Process(explanation, pm) ?? explanation)
                .Add("explanation_parse_mode", BotHelper.GetParseModeName(pm))
                .Add("question_parse_mode", BotHelper.GetParseModeName(pm))
                .Add("reply_markup", keyboard)
                .AddDictionary(defaultParameters?.ToDictionary())
                .Build();

            var message = await _bot.Client.CallAsync<Message?>(TelegramMethods.SEND_POLL, parameters);

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }
    
    public async Task<Message?> StopPoll(
        long chatId, 
        long messageId,
        IKeyboardMarkup? keyboard = null,
        ShortParameters? shortParameters = null)
    {
        try
        {
            await ApplyRateLimit();
            
            var parameters = new TelegramParametersBuilder()
                .Add("chat_id", chatId)
                .Add("message_id", messageId)
                .Add("reply_markup", keyboard)
                .AddDictionary(shortParameters?.ToDictionary())
                .Build();

            var message = await _bot.Client.CallAsync<Message?>(TelegramMethods.STOP_POLL, parameters);

            return message;
        }
        catch (Exception ex)
        {
            await _bot.AddException(ex);
            return null;
        }
    }
}