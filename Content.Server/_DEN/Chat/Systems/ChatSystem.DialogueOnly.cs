// SPDX-FileCopyrightText: 2026 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Text;
using Content.Shared._DEN.StringBounds;
using Content.Shared.Chat;
using Content.Shared.Language;
using Content.Shared.Language.Systems;
using Robust.Shared.Random;


namespace Content.Server.Chat.Systems;


public sealed partial class ChatSystem
{
    public string ReplaceRangeMultiple(
        string text,
        List<(int StartIndex, int EndIndex, string Replacement)> replacements
    )
    {
        if (replacements.Count == 0)
            return text;

        var builder = new StringBuilder();
        var currentIdx = 0;

        foreach (var replacement in replacements)
        {
            var before = text.Substring(currentIdx, replacement.StartIndex - currentIdx);

            builder.Append(before);
            builder.Append(replacement.Replacement);
            currentIdx = replacement.EndIndex;
        }

        var after = text.Substring(currentIdx);
        builder.Append(after);

        return builder.ToString();
    }

    public string ObfuscateSpeechDepending(
        string text,
        LanguagePrototype language,
        List<StringBoundsResult> keysWithinDialogue,
        bool isDetailed = false
    )
    {
        if (text.IndexOf("\"", StringComparison.Ordinal) == -1 || !isDetailed)
            return _language.ObfuscateSpeech(text, language);

        return ObfuscateOnlyText(text, language, keysWithinDialogue);
    }

    public string TransformSpeechDepending(
        EntityUid sender,
        string text,
        LanguagePrototype language,
        List<StringBoundsResult> keysWithinDialogue,
        bool isDetailed = false
    )
    {
        if (text.IndexOf("\"", StringComparison.Ordinal) == -1 || !isDetailed)
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
        if (!language.SpeechOverride.RequireSpeech)
            return text;

        var replacements = new List<(int StartIndex, int EndIndex, string Replacement)>();

        foreach (var key in keysWithinDialogue)
        {
            var ev = new TransformSpeechEvent(sender, key.Result);
            RaiseLocalEvent(ev);

            replacements.Add((key.StartIndex, key.EndIndex, ev.Message));
        }

        return ReplaceRangeMultiple(text, replacements);
    }

    public string ObfuscateMessageReadabilityDepending(
        string message,
        List<StringBoundsResult> keysWithinDialogue,
        float chance = DefaultObfuscationFactor,
        bool isDetailed = false
    )
    {
        if (message.IndexOf("\"", StringComparison.Ordinal) == -1 || !isDetailed)
            return ObfuscateMessageReadability(message, chance);

        return ObfuscateMessageReadability(message, keysWithinDialogue, chance);
    }

    public string ObfuscateMessageReadability(
        string message,
        List<StringBoundsResult> keysWithinDialogue,
        float chance = DefaultObfuscationFactor
    )
    {
        var replacements = new List<(int StartIndex, int EndIndex, string Replacement)>();

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

            replacements.Add((key.StartIndex, key.EndIndex, modifiedMessage.ToString()));
        }

        return ReplaceRangeMultiple(message, replacements);
    }

    public string SanitizeInGameICMessageDialogue(
        string message,
        bool shouldCapitalize,
        bool shouldPunctuate,
        bool shouldCapitalizeTheWordI,
        List<StringBoundsResult> keysWithinDialogue
    )
    {
        var replacements = new List<(int StartIndex, int EndIndex, string Replacement)>();

        foreach (var key in keysWithinDialogue)
        {
            var newPartialMessage = SanitizeMessageReplaceWords(key.Result);

            if (shouldCapitalize)
                newPartialMessage = SanitizeMessageCapital(newPartialMessage);
            if (shouldCapitalizeTheWordI)
                newPartialMessage = SanitizeMessageCapitalizeTheWordI(newPartialMessage);
            if (shouldPunctuate)
                newPartialMessage = SanitizeMessagePeriod(newPartialMessage);

            replacements.Add((key.StartIndex, key.EndIndex, newPartialMessage));
        }

        return ReplaceRangeMultiple(message, replacements);
    }

    public string SanitizeInGameICMessageDepending(
        EntityUid source,
        string message,
        out string? emoteStr,
        bool shouldCapitalize,
        bool shouldPunctuate,
        bool shouldCapitalizeTheWordI,
        InGameICChatType desiredType,
        List<StringBoundsResult> keysWithinDialogue,
        bool isDetailed = false
    )
    {
        if (desiredType == InGameICChatType.SubtleOOC)
        {
            emoteStr = null;
            return message;
        }

        if (message.IndexOf("\"", StringComparison.Ordinal) == -1 || !isDetailed)
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
        LanguagePrototype? languageOverride = null,
        string space = " "
    )
    {
        var color = DefaultSpeakColor;
        var localeId = "chat-manager-entity-say-wrap-free-message";
        var language = languageOverride ??= _language.GetLanguage(source);

        if (language.SpeechOverride.Color is { } colorOverride)
            color = Color.InterpolateBetween(color, colorOverride, colorOverride.A);

        var languageDisplay = language.ID !=  SharedLanguageSystem.FallbackLanguagePrototype
            ? Loc.GetString("chat-manager-language-prefix", ("language", language.ChatName))
            : string.Empty;
        var replacements = new List<(int StartIndex, int EndIndex, string Replacement)>();

        foreach (var key in keysWithinDialogue)
        {
            replacements.Add((key.StartIndex, key.EndIndex, $"[color={color.ToHexNoAlpha()}]{key.Result}[/color]"));
        }

        message = ReplaceRangeMultiple(message, replacements);

        return Loc.GetString(
            localeId,
            ("color", color),
            ("entityName", name),
            ("message", message),
            ("language", languageDisplay),
            ("space", space));
    }

    public string WrapPublicMessageDepending(
        EntityUid source,
        string name,
        string message,
        List<StringBoundsResult> keysWithinDialogue,
        LanguagePrototype? language = null,
        bool isDetailed = false,
        string space = " "
    )
    {
        if (message.IndexOf("\"", StringComparison.Ordinal) == -1 || !isDetailed)
            return WrapPublicMessage(source, name, message, language);

        return WrapPublicMessageDialogueOnly(source, name, message, keysWithinDialogue, language, space);
    }

    public string WrapWhisperMessageDialogueOnly(
        EntityUid source,
        bool isUnknown,
        string name,
        string message,
        List<StringBoundsResult> keysWithinDialogue,
        LanguagePrototype? languageOverride = null,
        string space = " "
    )
    {
        var color = DefaultSpeakColor;
        var localeId = "chat-manager-entity-whisper-free-wrap-message";

        if (isUnknown)
            localeId = "chat-manager-entity-whisper-unknown-free-wrap-message";

        var language = languageOverride ??= _language.GetLanguage(source);

        if (language.SpeechOverride.Color is { } colorOverride)
            color = Color.InterpolateBetween(color, colorOverride, colorOverride.A);

        var languageDisplay = language.ID !=  SharedLanguageSystem.FallbackLanguagePrototype
            ? Loc.GetString("chat-manager-language-prefix", ("language", language.ChatName))
            : string.Empty;
        var replacements = new List<(int StartIndex, int EndIndex, string Replacement)>();

        foreach (var key in keysWithinDialogue)
        {
            replacements.Add((key.StartIndex, key.EndIndex, $"[color={color.ToHexNoAlpha()}]{key.Result}[/color]"));
        }

        message = ReplaceRangeMultiple(message, replacements);

        return Loc.GetString(
            localeId,
            ("color", color),
            ("entityName", name),
            ("message", message),
            ("language", languageDisplay),
            ("space", space));
    }

    public string WrapWhisperMessageDepending(
        EntityUid source,
        bool isUnknown,
        string entityName,
        string message,
        List<StringBoundsResult> keysWithinDialogue,
        LanguagePrototype? language = null,
        bool isDetailed = false,
        string space = " "
    )
    {
        var wrapId = "chat-manager-entity-whisper-wrap-message";

        if (isUnknown)
            wrapId = "chat-manager-entity-whisper-unknown-wrap-message";

        if (message.IndexOf("\"", StringComparison.Ordinal) == -1 || !isDetailed)
            return WrapWhisperMessage(source, wrapId, entityName, message, language);

        return WrapWhisperMessageDialogueOnly(source, isUnknown, entityName, message, keysWithinDialogue, language, space);
    }

    /// <summary>
    ///     Returns the obfuscated version of text only within the dialogue constraints.
    /// </summary>
    public string ObfuscateOnlyText(string text, LanguagePrototype language, List<StringBoundsResult> keysWithinDialogue)
    {
        var replacements =  new List<(int StartIndex, int EndIndex, string Replacement)>();

        foreach (var key in keysWithinDialogue)
        {
            var obfuscated = language.Obfuscation.Obfuscate(key.Result);
            replacements.Add((key.StartIndex, key.EndIndex, obfuscated));
        }

        return ReplaceRangeMultiple(text, replacements);
    }
}
