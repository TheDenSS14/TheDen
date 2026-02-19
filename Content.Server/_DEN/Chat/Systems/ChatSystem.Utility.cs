// SPDX-FileCopyrightText: 2026 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using System.Text;
using Content.Server.Speech.Components;
using Content.Shared._DEN.Earmuffs;
using Content.Shared.Chat;
using Content.Shared.Ghost;
using Content.Shared.Language;
using Content.Shared.Language.Components;
using Content.Shared.Players;
using Content.Shared.Popups;
using Robust.Shared.Console;
using Robust.Shared.Network;
using Robust.Shared.Physics;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Utility;


namespace Content.Server.Chat.Systems;


public sealed partial class ChatSystem
{
    private enum MessageRangeCheckResult
    {
        Disallowed,
        HideChat,
        Full
    }

    /// <summary>
    ///     If hideChat should be set as far as replays are concerned.
    /// </summary>
    private bool MessageRangeHideChatForReplay(ChatTransmitRange range)
    {
        return range == ChatTransmitRange.HideChat;
    }

    /// <summary>
    ///     Checks if a target as returned from GetRecipients should receive the message.
    ///     Keep in mind data.Range is -1 for out of range observers.
    /// </summary>
    private MessageRangeCheckResult MessageRangeCheck(ICommonSession session, ICChatRecipientData data, ChatTransmitRange range)
    {
        var initialResult = MessageRangeCheckResult.Full;
        switch (range)
        {
            case ChatTransmitRange.Normal:
                initialResult = MessageRangeCheckResult.Full;
                break;
            case ChatTransmitRange.GhostRangeLimit:
                initialResult = (data.Observer && data.Range < 0 && !_adminManager.IsAdmin(session)) ? MessageRangeCheckResult.HideChat : MessageRangeCheckResult.Full;
                break;
            case ChatTransmitRange.HideChat:
                initialResult = MessageRangeCheckResult.HideChat;
                break;
            case ChatTransmitRange.NoGhosts:
                initialResult = (data.Observer && !_adminManager.IsAdmin(session)) ? MessageRangeCheckResult.Disallowed : MessageRangeCheckResult.Full;
                break;
        }
        var insistHideChat = data.HideChatOverride ?? false;
        var insistNoHideChat = !(data.HideChatOverride ?? true);
        if (insistHideChat && initialResult == MessageRangeCheckResult.Full)
            return MessageRangeCheckResult.HideChat;
        if (insistNoHideChat && initialResult == MessageRangeCheckResult.HideChat)
            return MessageRangeCheckResult.Full;
        return initialResult;
    }

    /// <summary>
    ///     Sends a chat message to the given players in range of the source entity.
    /// </summary>
    private void SendInVoiceRange(ChatChannel channel, string name, string message, string wrappedMessage, string obfuscated, string obfuscatedWrappedMessage, EntityUid source, ChatTransmitRange range, NetUserId? author = null, LanguagePrototype? languageOverride = null)
    {
        var language = languageOverride ?? _language.GetLanguage(source);
        var targetHasLanguage = TryComp<LanguageSpeakerComponent>(source, out var languageSpeakerComponent);
        var ignoreLanguage = channel.IsExemptFromLanguages();

        // Floof
        if (!ignoreLanguage && language.SpeechOverride.RequireHands
            // Sign language requires at least two complexly-interacting hands
            && !(_actionBlocker.CanComplexInteract(source) && _hands.EnumerateHands(source).Count(hand => hand.IsEmpty) >= 2))
        {
            _popups.PopupEntity(Loc.GetString("chat-manager-language-requires-hands"), source, PopupType.Medium);
            return;
        }

        foreach (var (session, data) in GetRecipients(source, VoiceRange))
        {
            if (session.AttachedEntity is not { Valid: true } playerEntity)
                continue;

            // DEN edit: VRChat earmuffs, but on Den!
            if (TryComp<EarmuffsComponent>(playerEntity, out var earmuffs)
                && earmuffs.Running && earmuffs.HearRange < data.Range
                && (channel == ChatChannel.Local || channel == ChatChannel.Emotes))
                continue;

            if (Transform(playerEntity).GridUid != Transform(source).GridUid
                && !CheckAttachedGrids(source, session.AttachedEntity.Value))
                continue;

            var entRange = MessageRangeCheck(session, data, range);
            if (entRange == MessageRangeCheckResult.Disallowed)
                continue;
            var entHideChat = entRange == MessageRangeCheckResult.HideChat;

            // If the channel does not support languages, or the entity can understand the message, send the original message, otherwise send the obfuscated version
            if (channel == ChatChannel.LOOC
                || channel == ChatChannel.Emotes
                || _language.CanUnderstand(playerEntity, language.ID, targetHasLanguage ? (source, languageSpeakerComponent) : null))
                _chatManager.ChatMessageToOne(channel, message, wrappedMessage, source, entHideChat, session.Channel, author: author);
            else
                _chatManager.ChatMessageToOne(channel, obfuscated, obfuscatedWrappedMessage, source, entHideChat, session.Channel, author: author);
        }

        _replay.RecordServerMessage(new ChatMessage(channel, message, wrappedMessage, GetNetEntity(source), null, MessageRangeHideChatForReplay(range)));
    }

    /// <summary>
    ///     Returns true if the given player is 'allowed' to send the given message, false otherwise.
    /// </summary>
    private bool CanSendInGame(string message, IConsoleShell? shell = null, ICommonSession? player = null)
    {
        // Non-players don't have to worry about these restrictions.
        if (player == null)
            return true;

        var mindContainerComponent = player.ContentData()?.Mind;

        if (mindContainerComponent == null)
        {
            shell?.WriteError("You don't have a mind!");
            return false;
        }

        if (player.AttachedEntity is not { Valid: true } _)
        {
            shell?.WriteError("You don't have an entity!");
            return false;
        }

        return !_chatManager.MessageCharacterLimit(player, message);
    }

    /// <summary>
    ///     Imp Edit: First modify message to respect entity accent, then send it to LastMessage system to record last message info for player
    /// </summary>
    public void HandleLastMessageBeforeDeath(EntityUid source, ICommonSession player, LanguagePrototype language, string message)
    {
        if (_consent.HasConsent(source, LastMessageConsent))
            return;

        var newMessage = TransformSpeech(source, message, language);
        _lastMessageBeforeDeathSystem.AddMessage(source, player, newMessage);
    }

    // ReSharper disable once InconsistentNaming
    private string SanitizeInGameICMessage(EntityUid source, string message, out string? emoteStr, bool capitalize = true, bool punctuate = false, bool capitalizeTheWordI = true, InGameICChatType channel = InGameICChatType.Speak)
    {
        if (channel == InGameICChatType.SubtleOOC)
        {
            emoteStr = null;
            return message;
        }

        var newMessage = SanitizeMessageReplaceWords(message.Trim());

        GetRadioKeycodePrefix(source, newMessage, out newMessage, out var prefix);
        _sanitizer.TrySanitizeOutSmilies(newMessage, source, out newMessage, out emoteStr);

        if (capitalize)
            newMessage = SanitizeMessageCapital(newMessage);
        if (capitalizeTheWordI)
            newMessage = SanitizeMessageCapitalizeTheWordI(newMessage, "i");
        if (punctuate)
            newMessage = SanitizeMessagePeriod(newMessage);

        return prefix + newMessage;
    }

    private string SanitizeInGameOOCMessage(string message)
    {
        var newMessage = message.Trim();
        newMessage = FormattedMessage.EscapeText(newMessage);

        return newMessage;
    }

    public string TransformSpeech(EntityUid sender, string message, LanguagePrototype language)
    {
        if (!language.SpeechOverride.RequireSpeech)
            return message; // Do not apply speech accents if there's no speech involved.

        var ev = new TransformSpeechEvent(sender, message);
        RaiseLocalEvent(ev);

        return ev.Message;
    }

    public bool CheckIgnoreSpeechBlocker(EntityUid sender, bool ignoreBlocker)
    {
        if (ignoreBlocker)
            return ignoreBlocker;

        var ev = new CheckIgnoreSpeechBlockerEvent(sender, ignoreBlocker);
        RaiseLocalEvent(sender, ev, true);

        return ev.IgnoreBlocker;
    }

    private IEnumerable<INetChannel> GetDeadChatClients()
    {
        return Filter.Empty()
            .AddWhereAttachedEntity(HasComp<GhostComponent>)
            .Recipients
            .Union(_adminManager.ActiveAdmins)
            .Select(p => p.Channel);
    }

    private string SanitizeMessagePeriod(string message)
    {
        if (string.IsNullOrEmpty(message))
            return message;
        // Adds a period if the last character is a letter.
        if (char.IsLetter(message[^1]))
            message += ".";
        return message;
    }

    [ValidatePrototypeId<ReplacementAccentPrototype>]
    public const string ChatSanitize_Accent = "chatsanitize";

    public string SanitizeMessageReplaceWords(string message)
    {
        if (string.IsNullOrEmpty(message)) return message;

        var msg = message;

        msg = _wordreplacement.ApplyReplacements(msg, ChatSanitize_Accent);

        return msg;
    }

    /// <summary>
    ///     Wraps a message sent by the specified entity into an "x says y" string.
    /// </summary>
    public string WrapPublicMessage(EntityUid source, string name, string message, LanguagePrototype? language = null)
    {
        var wrapId = GetSpeechVerb(source, message).Bold ? "chat-manager-entity-say-bold-wrap-message" : "chat-manager-entity-say-wrap-message";
        return WrapMessage(wrapId, InGameICChatType.Speak, source, name, message, language);
    }

    /// <summary>
    ///     Wraps a message whispered by the specified entity into an "x whispers y" string.
    /// </summary>
    public string WrapWhisperMessage(EntityUid source, LocId defaultWrap, string entityName, string message, LanguagePrototype? language = null)
    {
        return WrapMessage(defaultWrap, InGameICChatType.Whisper, source, entityName, message, language);
    }

    /// <summary>
    ///     Wraps a message sent by the specified entity into the specified wrap string.
    /// </summary>
    public string WrapMessage(LocId wrapId, InGameICChatType chatType, EntityUid source, string entityName, string message, LanguagePrototype? language)
    {
        language ??= _language.GetLanguage(source);
        if (language.SpeechOverride.MessageWrapOverrides.TryGetValue(chatType, out var wrapOverride))
            wrapId = wrapOverride;

        var speech = GetSpeechVerb(source, message);
        var verbId = language.SpeechOverride.SpeechVerbOverrides is { } verbsOverride
            ? _random.Pick(verbsOverride).ToString()
            : _random.Pick(speech.SpeechVerbStrings);
        var color = DefaultSpeakColor;
        if (language.SpeechOverride.Color is { } colorOverride)
            color = Color.InterpolateBetween(color, colorOverride, colorOverride.A);
        var languageDisplay = language.IsVisibleLanguage
            ? Loc.GetString("chat-manager-language-prefix", ("language", language.ChatName))
            : "";

        return Loc.GetString(wrapId,
            ("color", color),
            ("entityName", entityName),
            ("verb", Loc.GetString(verbId)),
            ("fontType", language.SpeechOverride.FontId ?? speech.FontId),
            ("fontSize", language.SpeechOverride.FontSize ?? speech.FontSize),
            ("message", message),
            ("language", languageDisplay));
    }

    /// <summary>
    ///     Returns list of players and ranges for all players withing some range. Also returns observers with a range of -1.
    /// </summary>
    private Dictionary<ICommonSession, ICChatRecipientData> GetRecipients(EntityUid source, float voiceGetRange)
    {
        // TODO proper speech occlusion

        var recipients = new Dictionary<ICommonSession, ICChatRecipientData>();
        var ghostHearing = GetEntityQuery<GhostHearingComponent>();
        var xforms = GetEntityQuery<TransformComponent>();

        var transformSource = xforms.GetComponent(source);
        var sourceMapId = transformSource.MapID;
        var sourceCoords = transformSource.Coordinates;

        foreach (var player in _playerManager.Sessions)
        {
            if (player.AttachedEntity is not { Valid: true } playerEntity)
                continue;

            var transformEntity = xforms.GetComponent(playerEntity);

            if (transformEntity.MapID != sourceMapId)
                continue;

            var observer = ghostHearing.HasComponent(playerEntity);

            // even if they are a ghost hearer, in some situations we still need the range
            if (sourceCoords.TryDistance(EntityManager, transformEntity.Coordinates, out var distance) && distance < voiceGetRange)
            {
                recipients.Add(player, new ICChatRecipientData(distance, observer));
                continue;
            }

            if (observer)
                recipients.Add(player, new ICChatRecipientData(-1, true));
        }

        RaiseLocalEvent(new ExpandICChatRecipientsEvent(source, voiceGetRange, recipients));
        return recipients;
    }

    public readonly record struct ICChatRecipientData(float Range, bool Observer, bool? HideChatOverride = null);

    public string ObfuscateMessageReadability(string message, float chance = DefaultObfuscationFactor)
    {
        var modifiedMessage = new StringBuilder(message);

        for (var i = 0; i < message.Length; i++)
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

        return modifiedMessage.ToString();
    }

    public string BuildGibberishString(IReadOnlyList<char> charOptions, int length)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < length; i++)
        {
            sb.Append(_random.Pick(charOptions));
        }
        return sb.ToString();
    }

    private bool CheckAttachedGrids(EntityUid source, EntityUid receiver)
    {
        if (!TryComp<JointComponent>(Transform(source).GridUid, out var sourceJoints)
            || !TryComp<JointComponent>(Transform(receiver).GridUid, out var receiverJoints))
            return false;

        foreach (var (id, _) in sourceJoints.GetJoints)
            if (receiverJoints.GetJoints.ContainsKey(id))
                return true;

        return false;
    }
}
