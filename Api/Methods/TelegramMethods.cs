namespace TgCore.Api.Methods;

public class TelegramMethods
{
    // Отправка сообщений
    public const string SEND_MESSAGE = "sendMessage";
    public const string SEND_PHOTO = "sendPhoto";
    public const string SEND_AUDIO = "sendAudio";
    public const string SEND_DOCUMENT = "sendDocument";
    public const string SEND_VIDEO = "sendVideo";
    public const string SEND_ANIMATION = "sendAnimation";
    public const string SEND_POLL = "sendPoll";
    public const string SEND_INVOICE = "sendInvoice";
    
    // Управление сообщениями
    public const string EDIT_MESSAGE_TEXT = "editMessageText";
    public const string DELETE_MESSAGE = "deleteMessage";
    public const string FORWARD_MESSAGE = "forwardMessage";
    public const string COPY_MESSAGE = "copyMessage";
    public const string ANSWER_CALLBACK_QUERY = "answerCallbackQuery";
    
    // Управление чатами
    public const string GET_CHAT = "getChat";
    public const string SET_CHAT_TITLE = "setChatTitle";
    public const string SET_CHAT_PHOTO = "setChatPhoto";
    public const string BAN_CHAT_MEMBER = "banChatMember";
    public const string CREATE_CHAT_INVITE_LINK = "createChatInviteLink";
    
    // Работа с пользователями/группами
    public const string GET_CHAT_ADMINISTRATORS = "getChatAdministrators";
    public const string GET_USER_PROFILE_PHOTOS = "getUserProfilePhotos";
    public const string GET_CHAT_MEMBERS_COUNT = "getChatMembersCount";
    
    // Управление ботом
    public const string GET_ME = "getMe";
    public const string SET_MY_COMMANDS = "setMyCommands";
    public const string SET_MY_NAME = "setMyName";
    
    // Прочее
    public const string GET_UPDATES = "getUpdates";
}