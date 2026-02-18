// SPDX-FileCopyrightText: 2026 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Globalization;
using System.Linq;
using Content.Server.Language;
using Content.Shared._DEN.StringBounds;
using Content.Shared.CCVar;
using Content.Shared.Chat;
using Content.Shared.Database;
using Content.Shared.Ghost;
using Content.Shared.IdentityManagement;
using Content.Shared.Language;
using Content.Shared.Language.Components;
using Content.Shared.Popups;
using Content.Shared.Radio;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Utility;


namespace Content.Server.Chat.Systems;


public sealed partial class ChatSystem
{
    private void SendEntitySpeak(
        EntityUid source,
        string originalMessage,
        ChatTransmitRange range,
        string? nameOverride,
        LanguagePrototype language,
        List<StringBoundsResult> keysWithinDialogue,
        bool hideLog = false,
        bool ignoreActionBlocker = false
        )
    {
        var isDetailed = originalMessage.StartsWith("!");
        originalMessage = FormattedMessage.RemoveMarkupPermissive(originalMessage);

        if (isDetailed && originalMessage.Length > 1)
        {
            originalMessage = originalMessage.Substring(1);
            keysWithinDialogue = _language.GetKeysWithinDialogue(originalMessage);
        }

        // Floof: allow languages that don't require speech
        if (language.SpeechOverride.RequireSpeech && !_actionBlocker.CanSpeak(source) && !ignoreActionBlocker)
            return;

        // The original message
        var message = TransformSpeechDepending(source, originalMessage, language, keysWithinDialogue, isDetailed);

        if (message.Length == 0)
            return;

        keysWithinDialogue = _language.GetKeysWithinDialogue(message);

        var speech = GetSpeechVerb(source, message);

        // get the entity's apparent name (if no override provided).
        string name;
        if (nameOverride != null)
        {
            name = nameOverride;
        }
        else
        {
            var nameEv = new TransformSpeakerNameEvent(source, Name(source));
            RaiseLocalEvent(source, nameEv);
            name = nameEv.VoiceName ?? Name(source);
            // Check for a speech verb override
            if (nameEv.SpeechVerb != null && _prototypeManager.TryIndex(nameEv.SpeechVerb, out var proto))
                speech = proto;
        }

        name = FormattedMessage.EscapeText(name);
        var separation = false;

        if (isDetailed && originalMessage.StartsWith("\""))
        {
            separation = true;
            name = $"[color=#636161]([/color][BubbleHeader][Name]{name}[/Name][/BubbleHeader][color=#636161])[/color]";
        }
        else if (isDetailed)
        {
            name = $"[BubbleHeader][Name]{name}[/Name][/BubbleHeader]";
        }

        var space = " ";

        if (isDetailed && originalMessage.StartsWith("'") && !separation)
        {
            var shouldSpace = !(originalMessage.StartsWith("'") || originalMessage.StartsWith(',')); // DEN: remove space when starting an action with ' or ,

            space = shouldSpace ? " " : "";
        }

        // The chat message wrapped in a "x says y" string
        var wrappedMessage = WrapPublicMessageDepending(source, name, message, keysWithinDialogue, language, isDetailed, space);

        // The chat message obfuscated via language obfuscation
        // APRIL: Dude what the fuck.
        var obfuscatedText = ObfuscateSpeechDepending(message, language, keysWithinDialogue, isDetailed);
        var obfuscatedKeys = _language.GetKeysWithinDialogue(obfuscatedText);

        var obfuscated = SanitizeInGameICMessageDepending(
            source,
            obfuscatedText,
            out var emoteStr,
            true,
            _configurationManager.GetCVar(CCVars.ChatPunctuation),
            (!CultureInfo.CurrentCulture.IsNeutralCulture && CultureInfo.CurrentCulture.Parent.Name == "en")
                || (CultureInfo.CurrentCulture.IsNeutralCulture && CultureInfo.CurrentCulture.Name == "en"),
            desiredType: InGameICChatType.Speak,
            obfuscatedKeys,
            isDetailed);

        // The language-obfuscated message wrapped in a "x says y" string
        var wrappedObfuscated = WrapPublicMessageDepending(source, name, obfuscated, obfuscatedKeys, language, isDetailed, space);
        SendInVoiceRange(ChatChannel.Local, name, message, wrappedMessage, obfuscated, wrappedObfuscated, source, range, languageOverride: language);

        var ev = new EntitySpokeEvent(source, message, null, false, language);
        RaiseLocalEvent(source, ev, true);

        // To avoid logging any messages sent by entities that are not players, like vendors, cloning, etc.
        // Also doesn't log if hideLog is true.
        if (!HasComp<ActorComponent>(source) || hideLog)
            return;

        if (originalMessage == message)
        {
            if (name != Name(source))
                _adminLogger.Add(LogType.Chat, LogImpact.Low, $"Say from {ToPrettyString(source):user} as {name}: {originalMessage}.");
            else
                _adminLogger.Add(LogType.Chat, LogImpact.Low, $"Say from {ToPrettyString(source):user}: {originalMessage}.");
        }
        else
        {
            if (name != Name(source))
                _adminLogger.Add(LogType.Chat, LogImpact.Low,
                    $"Say from {ToPrettyString(source):user} as {name}, original: {originalMessage}, transformed: {message}.");
            else
                _adminLogger.Add(LogType.Chat, LogImpact.Low,
                    $"Say from {ToPrettyString(source):user}, original: {originalMessage}, transformed: {message}.");
        }
    }

    private void SendEntityWhisper(
        EntityUid source,
        string originalMessage,
        ChatTransmitRange range,
        RadioChannelPrototype? channel,
        string? nameOverride,
        LanguagePrototype language,
        List<StringBoundsResult> keysWithinDialogue,
        bool hideLog = false,
        bool ignoreActionBlocker = false
    )
    {
        var isDetailed = originalMessage.StartsWith("!");
        originalMessage = FormattedMessage.RemoveMarkupPermissive(originalMessage);

        if (isDetailed && originalMessage.Length > 1)
        {
            originalMessage = originalMessage.Substring(1);
            keysWithinDialogue = _language.GetKeysWithinDialogue(originalMessage);
        }

        // Floof: allow languages that don't require speech
        if (language.SpeechOverride.RequireSpeech && !_actionBlocker.CanSpeak(source) && !ignoreActionBlocker)
            return;

        var targetHasLanguage = TryComp<LanguageSpeakerComponent>(source, out var languageSpeakerComponent);
        var message = TransformSpeechDepending(
            source,
            originalMessage,
            language,
            keysWithinDialogue,
            isDetailed);

        // Floof
        if (language.SpeechOverride.RequireHands
            // Sign language requires at least two complexly-interacting hands
            && !(_actionBlocker.CanComplexInteract(source) && _hands.EnumerateHands(source).Count(hand => hand.IsEmpty) >= 2))
        {
            _popups.PopupEntity(Loc.GetString("chat-manager-language-requires-hands"), source, source, PopupType.Medium);
            return;
        }

        if (message.Length == 0)
            return;

        // get the entity's name by visual identity (if no override provided).
        var nameIdentity = FormattedMessage.EscapeText(nameOverride ?? Identity.Name(source, EntityManager));
        // get the entity's name by voice (if no override provided).
        string name;
        if (nameOverride != null)
        {
            name = nameOverride;
        }
        else
        {
            var nameEv = new TransformSpeakerNameEvent(source, Name(source));
            RaiseLocalEvent(source, nameEv);
            name = nameEv.VoiceName;
        }

        name = FormattedMessage.EscapeText(name);

        var separation = false;

        if (isDetailed && originalMessage.StartsWith("\""))
        {
            separation = true;
            name = $"[color=#636161]([/color][BubbleHeader][Name]{name}[/Name][/BubbleHeader][color=#636161])[/color]";
        }
        else if (isDetailed)
        {
            name = $"[BubbleHeader][Name]{name}[/Name][/BubbleHeader]";
        }

        var space = " ";

        if (isDetailed && originalMessage.StartsWith("'") && !separation)
        {
            var shouldSpace = !(originalMessage.StartsWith("'") || originalMessage.StartsWith(',')); // DEN: remove space when starting an action with ' or ,

            space = shouldSpace ? " " : "";
        }

        var obfuscatedText = ObfuscateSpeechDepending(message, language, keysWithinDialogue, isDetailed);
        var obfuscatedKeys = _language.GetKeysWithinDialogue(obfuscatedText);
        var languageObfuscatedMessage = SanitizeInGameICMessageDepending(
            source,
            obfuscatedText,
            out var emoteStr,
            true,
            _configurationManager.GetCVar(CCVars.ChatPunctuation),
            (!CultureInfo.CurrentCulture.IsNeutralCulture && CultureInfo.CurrentCulture.Parent.Name == "en") ||
            (CultureInfo.CurrentCulture.IsNeutralCulture && CultureInfo.CurrentCulture.Name == "en"),
            InGameICChatType.Whisper,
            obfuscatedKeys,
            isDetailed);

        foreach (var (session, data) in GetRecipients(source, WhisperMuffledRange))
        {
            if (session.AttachedEntity is not { Valid: true } listener
                || session.AttachedEntity.HasValue && HasComp<GhostComponent>(session.AttachedEntity.Value)
                || !_interactionSystem.InRangeUnobstructed(source, listener, WhisperClearRange, _subtleWhisperMask))
                continue;

            if (MessageRangeCheck(session, data, range) != MessageRangeCheckResult.Full)
                continue; // Won't get logged to chat, and ghosts are too far away to see the pop-up, so we just won't send it to them.

            var canUnderstandLanguage = _language.CanUnderstand(
                listener,
                language.ID,
                targetHasLanguage ? (source, languageSpeakerComponent) : null);
            // How the entity perceives the message depends on whether it can understand its language
            var perceivedMessage = canUnderstandLanguage ? message : languageObfuscatedMessage;
            var perceivedKeys = _language.GetKeysWithinDialogue(perceivedMessage);

            // Result is the intermediate message derived from the perceived one via obfuscation
            // Wrapped message is the result wrapped in an "x says y" string
            string result, wrappedMessage;
            // Floof: handle languages that require LOS
            if (!language.SpeechOverride.RequireLOS && data.Range <= WhisperClearRange
                || _interactionSystem.InRangeUnobstructed(source, listener, WhisperClearRange, _subtleWhisperMask))
            {
                // Scenario 1: the listener can clearly understand the message
                result = perceivedMessage;
                wrappedMessage = WrapWhisperMessageDepending(source, false, name, result, perceivedKeys, language, isDetailed, space);
            }
            else if (_interactionSystem.InRangeUnobstructed(source, listener, WhisperMuffledRange, _subtleWhisperMask))
            {
                // Scenario 2: if the listener is too far, they only hear fragments of the message
                result = ObfuscateMessageReadabilityDepending(perceivedMessage, perceivedKeys, isDetailed: isDetailed);
                perceivedKeys = _language.GetKeysWithinDialogue(result);
                wrappedMessage = WrapWhisperMessageDepending(source, false, nameIdentity, result, perceivedKeys, language, isDetailed, space);
            }
            else
            {
                // Floof: If there is no LOS, the listener doesn't see at all
                if (language.SpeechOverride.RequireLOS)
                    return;

                // Scenario 3: If listener is too far and has no line of sight, they can't identify the whisperer's identity
                result = ObfuscateMessageReadabilityDepending(perceivedMessage, perceivedKeys, isDetailed: isDetailed);
                perceivedKeys = _language.GetKeysWithinDialogue(result);
                wrappedMessage = WrapWhisperMessageDepending(source, true, string.Empty, result, perceivedKeys, language, isDetailed, space);
            }

            _chatManager.ChatMessageToOne(ChatChannel.Whisper, result, wrappedMessage, source, false, session.Channel);
        }

        var replayWrap = WrapWhisperMessage(source, "chat-manager-entity-whisper-wrap-message", name, message, language);
        _replay.RecordServerMessage(new ChatMessage(ChatChannel.Whisper, message, replayWrap, GetNetEntity(source), null, MessageRangeHideChatForReplay(range)));

        var ev = new EntitySpokeEvent(source, message, channel, true, language);
        RaiseLocalEvent(source, ev, true);
        if (!hideLog)
            if (originalMessage == message)
            {
                if (name != Name(source))
                    _adminLogger.Add(LogType.Chat, LogImpact.Low, $"Whisper from {ToPrettyString(source):user} as {name}: {originalMessage}.");
                else
                    _adminLogger.Add(LogType.Chat, LogImpact.Low, $"Whisper from {ToPrettyString(source):user}: {originalMessage}.");
            }
            else
            {
                if (name != Name(source))
                    _adminLogger.Add(LogType.Chat, LogImpact.Low,
                    $"Whisper from {ToPrettyString(source):user} as {name}, original: {originalMessage}, transformed: {message}.");
                else
                    _adminLogger.Add(LogType.Chat, LogImpact.Low,
                    $"Whisper from {ToPrettyString(source):user}, original: {originalMessage}, transformed: {message}.");
            }
    }

    private void SendEntityEmote(
        EntityUid source,
        string action,
        ChatTransmitRange range,
        string? nameOverride,
        LanguagePrototype language,
        bool hideLog = false,
        bool checkEmote = true,
        bool ignoreActionBlocker = false,
        bool separateNameAndMessage = false,
        NetUserId? author = null
        )
    {
        if (!_actionBlocker.CanEmote(source) && !ignoreActionBlocker)
            return;

        // get the entity's apparent name (if no override provided).
        var ent = Identity.Entity(source, EntityManager);
        var name = FormattedMessage.EscapeText(nameOverride ?? Name(ent));
        action = FormattedMessage.RemoveMarkupPermissive(action);

        // DEN: use the format of 'detailed' when starting with '!'
        if (!separateNameAndMessage && action.StartsWith('!'))
        {
            action = action.Substring(1);
            separateNameAndMessage = true;
        }

        var useSpace = !(action.StartsWith("'") || action.StartsWith(",")); // DEN: remove space when starting an action with ' or ,
        var space = useSpace || separateNameAndMessage ? " " : ""; // DEN: remove space when starting an action with ' or ,
        var locString = "chat-manager-entity-me-wrap-message";

        if (separateNameAndMessage)
            locString = "chat-manager-entity-me-no-separate-wrap-message";

        // DEN: conditional was missing, added so PAis inherit names from the plushies they're put in
        if (nameOverride != null)
        {
            name = nameOverride;
        }
        else
        {
            var nameEv = new TransformSpeakerNameEvent(source, Name(source));
            RaiseLocalEvent(source, nameEv);
            name = nameEv.VoiceName;
        }
        name = FormattedMessage.EscapeText(name);
        // End DEN changes

        // Emotes use Identity.Name, since it doesn't actually involve your voice at all.
        var wrappedMessage = Loc.GetString(locString,
            ("entityName", name),
            ("entity", ent),
            ("space", space),
            ("color", "#d3d3d3"),
            ("message", action));

        if (checkEmote)
            TryEmoteChatInput(source, action);

        SendInVoiceRange(ChatChannel.Emotes, name, action, wrappedMessage, obfuscated: "", obfuscatedWrappedMessage: "", source, range, author);

        if (!hideLog)
            if (name != Name(source))
                _adminLogger.Add(LogType.Chat, LogImpact.Low, $"Emote from {ToPrettyString(source):user} as {name}: {action}");
            else
                _adminLogger.Add(LogType.Chat, LogImpact.Low, $"Emote from {ToPrettyString(source):user}: {action}");
    }

    private void SendEntitySubtle(
        EntityUid source,
        string action,
        ChatTransmitRange range,
        string? nameOverride,
        bool hideLog = false,
        bool ignoreActionBlocker = false,
        bool separateNameAndMessage = false,
        NetUserId? author = null,
        string? color = null
        )
    {
        if (!_actionBlocker.CanEmote(source) && !ignoreActionBlocker)
            return;

        // get the entity's apparent name (if no override provided).
        var ent = Identity.Entity(source, EntityManager);
        var name = FormattedMessage.EscapeText(nameOverride ?? Name(ent));
        action = FormattedMessage.RemoveMarkupPermissive(action).Trim();

        // DEN: use the format of 'detailed' when starting with '!'
        if (!separateNameAndMessage && action.StartsWith('!'))
        {
            action = action.Substring(1);
            separateNameAndMessage = true;
        }

        var useSpace = !(action.StartsWith("'") || action.StartsWith(",")); // DEN: remove space when starting an action with ' or ,
        var space = useSpace || separateNameAndMessage ? " " : ""; // DEN: remove space when starting an action with ' or ,
        var locString = "chat-manager-entity-subtle-wrap-message";

        if (separateNameAndMessage)
            locString = "chat-manager-entity-subtle-no-separate-wrap-message";

        // DEN: conditional was missing, added so PAis inherit names from the plushies they're put in
        if (nameOverride != null)
        {
            name = nameOverride;
        }
        else
        {
            var nameEv = new TransformSpeakerNameEvent(source, Name(source));
            RaiseLocalEvent(source, nameEv);
            name = nameEv.VoiceName;
        }
        name = FormattedMessage.EscapeText(name);
        // End DEN changes

        // Emotes use Identity.Name, since it doesn't actually involve your voice at all.
        var wrappedMessage = Loc.GetString(locString,
            ("entityName", name),
            ("entity", ent),
            ("color", color ?? DefaultSpeakColor.ToHex()),
            ("space", space), // DEN: remove space when starting an action with ' or ,
            ("message", action));

        foreach (var (session, data) in GetRecipients(source, WhisperClearRange))
        {
            if (session.AttachedEntity is not { Valid: true } listener
                || session.AttachedEntity.HasValue && HasComp<GhostComponent>(session.AttachedEntity.Value)
                || !_interactionSystem.InRangeUnobstructed(source, listener, WhisperClearRange, _subtleWhisperMask))
                continue;

            if (MessageRangeCheck(session, data, range) == MessageRangeCheckResult.Disallowed)
                continue;

            _chatManager.ChatMessageToOne(ChatChannel.Subtle, action, wrappedMessage, source, false, session.Channel);
        }

        if (!hideLog)
            if (name != Name(source))
                _adminLogger.Add(LogType.Chat, LogImpact.Low, $"Subtle from {ToPrettyString(source):user} as {name}: {action}");
            else
                _adminLogger.Add(LogType.Chat, LogImpact.Low, $"Subtle from {ToPrettyString(source):user}: {action}");
    }

    // ReSharper disable once InconsistentNaming
    private void SendLOOC(EntityUid source, ICommonSession player, string message, bool hideChat)
    {
        var name = FormattedMessage.EscapeText(Identity.Name(source, EntityManager));

        if (_adminManager.IsAdmin(player))
        {
            if (!_adminLoocEnabled) return;
        }
        else if (!_loocEnabled) return;

        // If crit player LOOC is disabled, don't send the message at all.
        if (!_critLoocEnabled && _mobStateSystem.IsCritical(source))
            return;

        var wrappedMessage = Loc.GetString("chat-manager-entity-looc-wrap-message",
            ("entityName", name),
            ("message", FormattedMessage.EscapeText(message)));

        SendInVoiceRange(ChatChannel.LOOC, name, message, wrappedMessage,
            obfuscated: string.Empty,
            obfuscatedWrappedMessage: string.Empty, // will be skipped anyway
            source,
            hideChat ? ChatTransmitRange.HideChat : ChatTransmitRange.Normal,
            player.UserId,
            languageOverride: LanguageSystem.Universal);
        _adminLogger.Add(LogType.Chat, LogImpact.Low, $"LOOC from {player:Player}: {message}");
    }

    private void SendDeadChat(EntityUid source, ICommonSession player, string message, bool hideChat)
    {
        var clients = GetDeadChatClients();
        var playerName = Name(source);
        string wrappedMessage;
        if (_adminManager.IsAdmin(player))
        {
            wrappedMessage = Loc.GetString("chat-manager-send-admin-dead-chat-wrap-message",
                ("adminChannelName", Loc.GetString("chat-manager-admin-channel-name")),
                ("userName", player.Channel.UserName),
                ("message", FormattedMessage.EscapeText(message)));
            _adminLogger.Add(LogType.Chat, LogImpact.Low, $"Admin dead chat from {player:Player}: {message}");
        }
        else
        {
            wrappedMessage = Loc.GetString("chat-manager-send-dead-chat-wrap-message",
                ("deadChannelName", Loc.GetString("chat-manager-dead-channel-name")),
                ("playerName", (playerName)),
                ("message", FormattedMessage.EscapeText(message)));
            _adminLogger.Add(LogType.Chat, LogImpact.Low, $"Dead chat from {player:Player}: {message}");
        }

        _chatManager.ChatMessageToMany(ChatChannel.Dead, message, wrappedMessage, source, hideChat, true, clients.ToList(), author: player.UserId);
    }
}
