namespace TgCore.Api.Interfaces;

[JsonDerivedType(typeof(InlineKeyboard))]
[JsonDerivedType(typeof(ReplyKeyboard))]
public interface IKeyboardMarkup { }