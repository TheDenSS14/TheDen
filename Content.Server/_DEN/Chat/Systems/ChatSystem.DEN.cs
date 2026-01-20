using System.Text;
using Content.Shared._DEN.StringBounds;
using Content.Shared.Chat;
using Content.Shared.Language;
using Robust.Shared.Random;


namespace Content.Server.Chat.Systems;


public sealed partial class ChatSystem
{
    public string ObfuscateSpeechDepending(
        string text,
        LanguagePrototype language,
        List<StringBoundsResult> keysWithinDialogue
    )
    {
        if (text.IndexOf("\"", StringComparison.Ordinal) == -1)
            return _language.ObfuscateSpeech(text, language);

        return _language.ObfuscateOnlyText(text, language, keysWithinDialogue);
    }

    public string TransformSpeechDepending(
        EntityUid sender,
        string text,
        LanguagePrototype language,
        List<StringBoundsResult> keysWithinDialogue
    )
    {
        if (text.IndexOf("\"", StringComparison.Ordinal) == -1)
            return TransformSpeech(sender, text, language);

        return TransformOnlyDialogue(sender, text, language, keysWithinDialogue);
    }

    public string TransformOnlyDialogue(
        EntityUid sender,
        string text,
        LanguagePrototype language,
        List<StringBoundsResult> keysWithinDialogue
    )
    {
        var lastBuiltText = text;

        if (!language.SpeechOverride.RequireSpeech)
            return text;

        foreach (var key in keysWithinDialogue)
        {
            var ev = new TransformSpeechEvent(sender, key.Result);
            RaiseLocalEvent(ev);

            lastBuiltText = _language.ReplaceRange(lastBuiltText, key.StartIndex + 1, key.EndIndex - 1, ev.Message);
        }

        return lastBuiltText;
    }

    public string ObfuscateMessageReadabilityDepending(
        string message,
        List<StringBoundsResult> keysWithinDialogue,
        float chance = DefaultObfuscationFactor
    )
    {
        if (message.IndexOf("\"", StringComparison.Ordinal) == -1)
            return ObfuscateMessageReadability(message, chance);

        return ObfuscateMessageReadability(message, keysWithinDialogue, chance);
    }

    public string ObfuscateMessageReadability(
        string message,
        List<StringBoundsResult> keysWithinDialogue,
        float chance = DefaultObfuscationFactor
    )
    {
        var lastMessage = message;

        foreach (var key in keysWithinDialogue)
        {
            var modifiedMessage = new StringBuilder(key.Result);

            for (var i = 0; i < key.Result.Length; i++)
            {
                if (char.IsWhiteSpace((modifiedMessage[i])))
                {
                    continue;
                }

                if (_random.Prob(1 - chance))
                {
                    modifiedMessage[i] = '~';
                }
            }

            lastMessage = _language.ReplaceRange(
                lastMessage,
                key.StartIndex - 1,
                key.EndIndex + 1,
                modifiedMessage.ToString());
        }

        return lastMessage;
    }

    public string SanitizeInGameICMessageDialogue(
        string message,
        bool shouldCapitalize,
        bool shouldPunctuate,
        bool shouldCapitalizeTheWordI,
        List<StringBoundsResult> keysWithinDialogue
    )
    {
        var newMessage = message;

        foreach (var key in keysWithinDialogue)
        {
            var newPartialMessage = SanitizeMessageReplaceWords(key.Result);

            if (shouldCapitalize)
                newPartialMessage = SanitizeMessageCapital(newPartialMessage);
            if (shouldCapitalizeTheWordI)
                newPartialMessage = SanitizeMessageCapitalizeTheWordI(newPartialMessage);
            if (shouldPunctuate)
                newPartialMessage = SanitizeMessagePeriod(newPartialMessage);

            newMessage = _language.ReplaceRange(newMessage, key.StartIndex + 1, key.EndIndex - 1, newPartialMessage);
        }

        return newMessage;
    }

    public string SanitizeInGameICMessageDepending(
        EntityUid source,
        string message,
        out string? emoteStr,
        bool shouldCapitalize,
        bool shouldPunctuate,
        bool shouldCapitalizeTheWordI,
        InGameICChatType desiredType,
        List<StringBoundsResult> keysWithinDialogue
    )
    {
        if (desiredType == InGameICChatType.SubtleOOC)
        {
            emoteStr = null;
            return message;
        }

        if (message.IndexOf("\"", StringComparison.Ordinal) == -1)
        {
            return SanitizeInGameICMessage(
                source,
                message,
                out emoteStr,
                shouldCapitalize,
                shouldPunctuate,
                shouldCapitalizeTheWordI,
                desiredType);
        }

        emoteStr = null;
        var msg = SanitizeInGameICMessageDialogue(
            message,
            shouldCapitalize,
            shouldPunctuate,
            shouldCapitalizeTheWordI,
            keysWithinDialogue);
        return msg;
    }

    public string WrapPublicMessageDialogueOnly(
        EntityUid source,
        string name,
        string message,
        List<StringBoundsResult> keysWithinDialogue,
        LanguagePrototype? languageOverride = null
    )
    {
        var color = DefaultSpeakColor;
        var localeId = "chat-manager-entity-say-wrap-free-message";
        var language = languageOverride ??= _language.GetLanguage(source);
        var languageDisplay = language.IsVisibleLanguage
            ? Loc.GetString("chat-manager-language-prefix", ("language", language.ChatName))
            : "";

        foreach (var key in keysWithinDialogue)
        {
            message = _language.ReplaceRange(
                message,
                key.StartIndex + 1,
                key.EndIndex - 1,
                $"[color={color.ToHexNoAlpha()}]{key.Result}[/color]");
        }

        return Loc.GetString(
            localeId,
            ("color", color),
            ("entityName", name),
            ("message", message),
            ("language", languageDisplay));
    }

    public string WrapPublicMessageDepending(
        EntityUid source,
        string name,
        string message,
        List<StringBoundsResult> keysWithinDialogue,
        LanguagePrototype? language = null
    )
    {
        if (message.IndexOf("\"", StringComparison.Ordinal) == -1)
            return WrapPublicMessage(source, name, message, language);

        return WrapPublicMessageDialogueOnly(source, name, message, keysWithinDialogue, language);
    }

    public string WrapWhisperMessageDialogueOnly(
        EntityUid source,
        bool isUnknown,
        string name,
        string message,
        List<StringBoundsResult> keysWithinDialogue,
        LanguagePrototype? languageOverride = null
    )
    {
        var color = DefaultSpeakColor;
        var localeId = "chat-manager-entity-whisper-free-wrap-message";

        if (isUnknown)
            localeId = "chat-manager-entity-whisper-unknown-free-wrap-message";

        var language = languageOverride ??= _language.GetLanguage(source);
        var languageDisplay = language.IsVisibleLanguage
            ? Loc.GetString("chat-manager-language-prefix", ("language", language.ChatName))
            : "";

        foreach (var key in keysWithinDialogue)
        {
            message = _language.ReplaceRange(
                message,
                key.StartIndex + 1,
                key.EndIndex - 1,
                $"[color={color.ToHexNoAlpha()}]{key.Result}[/color]");
        }

        return Loc.GetString(
            localeId,
            ("color", color),
            ("entityName", name),
            ("message", message),
            ("language", languageDisplay));
    }

    public string WrapWhisperMessageDepending(
        EntityUid source,
        bool isUnknown,
        string entityName,
        string message,
        List<StringBoundsResult> keysWithinDialogue,
        LanguagePrototype? language = null
    )
    {
        var wrapId = "chat-manager-entity-whisper-wrap-message";

        if (isUnknown)
            wrapId = "chat-manager-entity-whisper-unknown-wrap-message";

        if (message.IndexOf("\"", StringComparison.Ordinal) == -1)
            return WrapWhisperMessage(source, wrapId, entityName, message, language);

        return WrapWhisperMessageDialogueOnly(source, isUnknown, entityName, message, keysWithinDialogue, language);
    }
}
