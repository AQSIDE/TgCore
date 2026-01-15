namespace TgCore.Api.Methods;

public class TelegramMethods
{
    // Отправка сообщений
    public const string SendMessage = "sendMessage";
    public const string SendPhoto = "sendPhoto";
    public const string SendAudio = "sendAudio";
    public const string SendDocument = "sendDocument";
    public const string SendVideo = "sendVideo";
    public const string SendPoll = "sendPoll";
    public const string SendInvoice = "sendInvoice";
    
    // Управление сообщениями
    public const string EditMessageText = "editMessageText";
    public const string DeleteMessage = "deleteMessage";
    public const string ForwardMessage = "forwardMessage";
    public const string CopyMessage = "copyMessage";
    public const string AnswerCallbackQuery = "answerCallbackQuery";
    
    // Управление чатами
    public const string GetChat = "getChat";
    public const string SetChatTitle = "setChatTitle";
    public const string SetChatPhoto = "setChatPhoto";
    public const string BanChatMember = "banChatMember";
    public const string CreateChatInviteLink = "createChatInviteLink";
    
    // Работа с пользователями/группами
    public const string GetChatAdministrators = "getChatAdministrators";
    public const string GetUserProfilePhotos = "getUserProfilePhotos";
    public const string GetChatMembersCount = "getChatMembersCount";
    
    // Управление ботом
    public const string GetMe = "getMe";
    public const string SetMe = "setMyCommands";
    public const string SetMyName = "setMyName";
    
    // Прочее
    public const string GetUpdates = "getUpdates";
}