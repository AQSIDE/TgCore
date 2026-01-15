using System.Text.Json.Serialization;
using TgCore.Api.Keyboards.Inline;
using TgCore.Api.Keyboards.Reply;

namespace TgCore.Api.Interfaces;

[JsonDerivedType(typeof(InlineKeyboard))]
[JsonDerivedType(typeof(ReplyKeyboard))]
public interface IKeyboardMarkup { }