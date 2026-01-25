namespace TgCore.Api.Methods;

public static class TelegramMethods
{
    // Отправка сообщений
    public const string SEND_MESSAGE = "sendMessage";
    public const string SEND_PHOTO = "sendPhoto";
    public const string SEND_AUDIO = "sendAudio";
    public const string SEND_DOCUMENT = "sendDocument";
    public const string SEND_VIDEO = "sendVideo";
    public const string SEND_ANIMATION = "sendAnimation";
    public const string SEND_VOICE = "sendVoice";
    public const string SEND_VIDEO_NOTE = "sendVideoNote";
    public const string SEND_MEDIA_GROUP = "sendMediaGroup";
    public const string SEND_LOCATION = "sendLocation";
    public const string SEND_VENUE = "sendVenue";
    public const string SEND_CONTACT = "sendContact";
    public const string SEND_POLL = "sendPoll";
    public const string SEND_DICE = "sendDice";
    public const string SEND_CHAT_ACTION = "sendChatAction";
    public const string SEND_INVOICE = "sendInvoice";
    public const string SEND_GAME = "sendGame";
    public const string SET_GAME_SCORE = "setGameScore";
    public const string GET_GAME_HIGH_SCORES = "getGameHighScores";
    
    // Управление сообщениями
    public const string EDIT_MESSAGE_TEXT = "editMessageText";
    public const string EDIT_MESSAGE_CAPTION = "editMessageCaption";
    public const string EDIT_MESSAGE_MEDIA = "editMessageMedia";
    public const string EDIT_MESSAGE_REPLY_MARKUP = "editMessageReplyMarkup";
    public const string EDIT_MESSAGE_LIVE_LOCATION = "editMessageLiveLocation";
    public const string STOP_MESSAGE_LIVE_LOCATION = "stopMessageLiveLocation";
    public const string DELETE_MESSAGE = "deleteMessage";
    public const string FORWARD_MESSAGE = "forwardMessage";
    public const string COPY_MESSAGE = "copyMessage";
    public const string PIN_CHAT_MESSAGE = "pinChatMessage";
    public const string UNPIN_CHAT_MESSAGE = "unpinChatMessage";
    public const string UNPIN_ALL_CHAT_MESSAGES = "unpinAllChatMessages";
    public const string ANSWER_CALLBACK_QUERY = "answerCallbackQuery";
    public const string ANSWER_INLINE_QUERY = "answerInlineQuery";
    public const string ANSWER_WEB_APP_QUERY = "answerWebAppQuery";
    public const string ANSWER_PRE_CHECKOUT_QUERY = "answerPreCheckoutQuery";
    public const string ANSWER_SHIPPING_QUERY = "answerShippingQuery";
    
    // Управление чатами
    public const string GET_CHAT = "getChat";
    public const string GET_CHAT_MEMBER = "getChatMember";
    public const string GET_CHAT_MEMBERS_COUNT = "getChatMembersCount";
    public const string GET_CHAT_ADMINISTRATORS = "getChatAdministrators";
    public const string LEAVE_CHAT = "leaveChat";
    public const string SET_CHAT_TITLE = "setChatTitle";
    public const string SET_CHAT_DESCRIPTION = "setChatDescription";
    public const string SET_CHAT_PHOTO = "setChatPhoto";
    public const string DELETE_CHAT_PHOTO = "deleteChatPhoto";
    public const string SET_CHAT_STICKER_SET = "setChatStickerSet";
    public const string DELETE_CHAT_STICKER_SET = "deleteChatStickerSet";
    public const string GET_CHAT_MEMBER_COUNT = "getChatMemberCount";
    public const string GET_CHAT_MENU_BUTTON = "getChatMenuButton";
    public const string SET_CHAT_MENU_BUTTON = "setChatMenuButton";
    
    // Управление участниками чата
    public const string BAN_CHAT_MEMBER = "banChatMember";
    public const string UNBAN_CHAT_MEMBER = "unbanChatMember";
    public const string RESTRICT_CHAT_MEMBER = "restrictChatMember";
    public const string PROMOTE_CHAT_MEMBER = "promoteChatMember";
    public const string SET_CHAT_ADMINISTRATOR_CUSTOM_TITLE = "setChatAdministratorCustomTitle";
    public const string BAN_CHAT_SENDER_CHAT = "banChatSenderChat";
    public const string UNBAN_CHAT_SENDER_CHAT = "unbanChatSenderChat";
    
    // Приглашения и ссылки
    public const string CREATE_CHAT_INVITE_LINK = "createChatInviteLink";
    public const string EDIT_CHAT_INVITE_LINK = "editChatInviteLink";
    public const string REVOKE_CHAT_INVITE_LINK = "revokeChatInviteLink";
    public const string APPROVE_CHAT_JOIN_REQUEST = "approveChatJoinRequest";
    public const string DECLINE_CHAT_JOIN_REQUEST = "declineChatJoinRequest";
    public const string EXPORT_CHAT_INVITE_LINK = "exportChatInviteLink";
    
    // Работа с пользователями
    public const string GET_USER_PROFILE_PHOTOS = "getUserProfilePhotos";
    public const string GET_FILE = "getFile";
    public const string KICK_CHAT_MEMBER = "kickChatMember"; 
    
    // Управление ботом
    public const string GET_ME = "getMe";
    public const string GET_MY_COMMANDS = "getMyCommands";
    public const string SET_MY_COMMANDS = "setMyCommands";
    public const string DELETE_MY_COMMANDS = "deleteMyCommands";
    public const string GET_MY_DEFAULT_ADMINISTRATOR_RIGHTS = "getMyDefaultAdministratorRights";
    public const string SET_MY_DEFAULT_ADMINISTRATOR_RIGHTS = "setMyDefaultAdministratorRights";
    public const string GET_MY_NAME = "getMyName";
    public const string SET_MY_NAME = "setMyName";
    public const string GET_MY_DESCRIPTION = "getMyDescription";
    public const string SET_MY_DESCRIPTION = "setMyDescription";
    public const string GET_MY_SHORT_DESCRIPTION = "getMyShortDescription";
    public const string SET_MY_SHORT_DESCRIPTION = "setMyShortDescription";
    
    // Работа с файлами
    public const string DOWNLOAD_FILE = "getFile";
    
    // Стикеры
    public const string SEND_STICKER = "sendSticker";
    public const string GET_STICKER_SET = "getStickerSet";
    public const string UPLOAD_STICKER_FILE = "uploadStickerFile";
    public const string CREATE_NEW_STICKER_SET = "createNewStickerSet";
    public const string ADD_STICKER_TO_SET = "addStickerToSet";
    public const string SET_STICKER_POSITION_IN_SET = "setStickerPositionInSet";
    public const string DELETE_STICKER_FROM_SET = "deleteStickerFromSet";
    public const string SET_STICKER_SET_THUMB = "setStickerSetThumb";
    public const string SET_STICKER_EMOJI_LIST = "setStickerEmojiList";
    public const string SET_STICKER_KEYWORDS = "setStickerKeywords";
    public const string SET_STICKER_MASK_POSITION = "setStickerMaskPosition";
    public const string SET_CUSTOM_EMOJI_STICKER_SET_THUMBNAIL = "setCustomEmojiStickerSetThumbnail";
    
    // Опросы (Polls)
    public const string STOP_POLL = "stopPoll";
    
    // Форум
    public const string CREATE_FORUM_TOPIC = "createForumTopic";
    public const string EDIT_FORUM_TOPIC = "editForumTopic";
    public const string CLOSE_FORUM_TOPIC = "closeForumTopic";
    public const string REOPEN_FORUM_TOPIC = "reopenForumTopic";
    public const string DELETE_FORUM_TOPIC = "deleteForumTopic";
    public const string UNPIN_ALL_FORUM_TOPIC_MESSAGES = "unpinAllForumTopicMessages";
    public const string EDIT_GENERAL_FORUM_TOPIC = "editGeneralForumTopic";
    public const string CLOSE_GENERAL_FORUM_TOPIC = "closeGeneralForumTopic";
    public const string REOPEN_GENERAL_FORUM_TOPIC = "reopenGeneralForumTopic";
    public const string HIDE_GENERAL_FORUM_TOPIC = "hideGeneralForumTopic";
    public const string UNHIDE_GENERAL_FORUM_TOPIC = "unhideGeneralForumTopic";
    
    // Обновления
    public const string GET_UPDATES = "getUpdates";
    public const string SET_WEBHOOK = "setWebhook";
    public const string DELETE_WEBHOOK = "deleteWebhook";
    public const string GET_WEBHOOK_INFO = "getWebhookInfo";
    
    // Прочее
    public const string LOG_OUT = "logOut";
    public const string CLOSE = "close";
    public const string GET_CUSTOM_EMOJI_STICKERS = "getCustomEmojiStickers";
}