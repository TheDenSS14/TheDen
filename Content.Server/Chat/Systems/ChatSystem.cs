// SPDX-FileCopyrightText: 2022 EmoGarbage404
// SPDX-FileCopyrightText: 2022 Flipp Syder
// SPDX-FileCopyrightText: 2022 Julian Giebel
// SPDX-FileCopyrightText: 2022 Justin Trotter
// SPDX-FileCopyrightText: 2022 Júlio César Ueti
// SPDX-FileCopyrightText: 2022 Morber
// SPDX-FileCopyrightText: 2022 Rane
// SPDX-FileCopyrightText: 2022 Taran
// SPDX-FileCopyrightText: 2022 Tom Richardson
// SPDX-FileCopyrightText: 2022 Tomás Alves
// SPDX-FileCopyrightText: 2022 Veritius
// SPDX-FileCopyrightText: 2022 keronshb
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 20kdc
// SPDX-FileCopyrightText: 2023 Alex Evgrashin
// SPDX-FileCopyrightText: 2023 Chief-Engineer
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Errant
// SPDX-FileCopyrightText: 2023 Hannah Giovanna Dawson
// SPDX-FileCopyrightText: 2023 HerCoyote23
// SPDX-FileCopyrightText: 2023 Interrobang01
// SPDX-FileCopyrightText: 2023 Jezithyr
// SPDX-FileCopyrightText: 2023 Kara
// SPDX-FileCopyrightText: 2023 Mr. 27
// SPDX-FileCopyrightText: 2023 PHCodes
// SPDX-FileCopyrightText: 2023 ShadowCommander
// SPDX-FileCopyrightText: 2023 Skye
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2023 Visne
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Debug
// SPDX-FileCopyrightText: 2024 Fansana
// SPDX-FileCopyrightText: 2024 FoxxoTrystan
// SPDX-FileCopyrightText: 2024 Leon Friedrich
// SPDX-FileCopyrightText: 2024 LordCarve
// SPDX-FileCopyrightText: 2024 Mnemotechnican
// SPDX-FileCopyrightText: 2024 Nemanja
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2024 ShatteredSwords
// SPDX-FileCopyrightText: 2024 Skubman
// SPDX-FileCopyrightText: 2024 SlamBamActionman
// SPDX-FileCopyrightText: 2024 Tayrtahn
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2024 beck-thompson
// SPDX-FileCopyrightText: 2024 fox
// SPDX-FileCopyrightText: 2024 ike709
// SPDX-FileCopyrightText: 2024 themias
// SPDX-FileCopyrightText: 2025 AirFryerBuyOneGetOneFree
// SPDX-FileCopyrightText: 2025 Falcon
// SPDX-FileCopyrightText: 2025 RedFoxIV
// SPDX-FileCopyrightText: 2025 Timfa
// SPDX-FileCopyrightText: 2025 ash lea
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Globalization;
using Content.Server.Administration.Logs;
using Content.Server.Administration.Managers;
using Content.Server.Chat.Managers;
using Content.Server.GameTicking;
using Content.Server.Ghost;
using Content.Server.Language;
using Content.Server.Speech.EntitySystems;
using Content.Server.Station.Systems;
using Content.Shared.ActionBlocker;
using Content.Shared.CCVar;
using Content.Shared.Chat;
using Content.Shared.Ghost;
using Content.Shared.Language;
using Content.Shared.Interaction;
using Content.Shared.Mobs.Systems;
using Content.Shared.Players.RateLimiting;
using Content.Shared.Whitelist;
using Robust.Server.Player;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Replays;
using Robust.Shared.Utility;
using Content.Shared.Physics;
using Content.Server.Hands.Systems;
using Content.Shared.Examine;
using Content.Shared.Popups;
using Content.Server._Wizden.Chat.Systems;
using Content.Server._Floof.Consent;
using Content.Shared._Floof.Consent;


namespace Content.Server.Chat.Systems;

// Dear contributor. When I was introducing changes to this system only god and I knew what I was doing.
// Now only god knows. Please don't touch this code ever again. If you do have to, increment this counter as a warning for others:
// TOTAL_HOURS_WASTED_HERE_EE = 19

// TODO refactor whatever active warzone this class and chatmanager have become
/// <summary>
///     ChatSystem is responsible for in-simulation chat handling, such as whispering, speaking, emoting, etc.
///     ChatSystem depends on ChatManager to actually send the messages.
/// </summary>
public sealed partial class ChatSystem : SharedChatSystem
{
    [Dependency] private readonly IReplayRecordingManager _replay = default!;
    [Dependency] private readonly IConfigurationManager _configurationManager = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly IChatSanitizationManager _sanitizer = default!;
    [Dependency] private readonly IAdminManager _adminManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly ActionBlockerSystem _actionBlocker = default!;
    [Dependency] private readonly StationSystem _stationSystem = default!;
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedInteractionSystem _interactionSystem = default!;
    [Dependency] private readonly ReplacementAccentSystem _wordreplacement = default!;
    [Dependency] private readonly LanguageSystem _language = default!;
    [Dependency] private readonly TelepathicChatSystem _telepath = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;
    [Dependency] private readonly ILogManager _logManager = default!;
    [Dependency] private readonly GhostSystem _ghost = default!;
    [Dependency] private readonly ExamineSystemShared _examine = default!;
    [Dependency] private readonly EmpathyChatSystem _empathy = default!;
    [Dependency] private readonly SharedPopupSystem _popups = default!; // Floof
    [Dependency] private readonly HandsSystem _hands = default!; // Floof
    [Dependency] private readonly LastMessageBeforeDeathSystem _lastMessageBeforeDeathSystem = default!; // Imp Edit LastMessageBeforeDeath Webhook
    [Dependency] private readonly ConsentSystem _consent = default!;

    private readonly ProtoId<ConsentTogglePrototype> LastMessageConsent = "LastMessage";

    public const int VoiceRange = 10; // how far voice goes in world units
    public const int WhisperClearRange = 2; // how far whisper goes while still being understandable, in world units
    public const float InSpaceRange = .3f; // how far speech travels in space
    public const string DefaultAnnouncementSound = "/Audio/Announcements/announce.ogg";
    public const float DefaultObfuscationFactor = 0.2f; // Percentage of symbols in a whispered message that can be seen even by "far" listeners
    public readonly Color DefaultSpeakColor = Color.White;
    private readonly CollisionGroup _subtleWhisperMask = CollisionGroup.InteractImpassable;

    private bool _loocEnabled = true;
    private bool _deadLoocEnabled;
    private bool _critLoocEnabled;
    private readonly bool _adminLoocEnabled = true;

    private ISawmill _sawmill = default!;

    public override void Initialize()
    {
        base.Initialize();
        CacheEmotes();
        Subs.CVar(_configurationManager, CCVars.LoocEnabled, OnLoocEnabledChanged, true);
        Subs.CVar(_configurationManager, CCVars.DeadLoocEnabled, OnDeadLoocEnabledChanged, true);
        Subs.CVar(_configurationManager, CCVars.CritLoocEnabled, OnCritLoocEnabledChanged, true);

        _sawmill = _logManager.GetSawmill("chat");

        SubscribeLocalEvent<GameRunLevelChangedEvent>(OnGameChange);
    }

    private void OnLoocEnabledChanged(bool val)
    {
        if (_loocEnabled == val) return;

        _loocEnabled = val;
        _chatManager.DispatchServerAnnouncement(
            Loc.GetString(val ? "chat-manager-looc-chat-enabled-message" : "chat-manager-looc-chat-disabled-message"));
    }

    private void OnDeadLoocEnabledChanged(bool val)
    {
        if (_deadLoocEnabled == val) return;

        _deadLoocEnabled = val;
        _chatManager.DispatchServerAnnouncement(
            Loc.GetString(val ? "chat-manager-dead-looc-chat-enabled-message" : "chat-manager-dead-looc-chat-disabled-message"));
    }

    private void OnCritLoocEnabledChanged(bool val)
    {
        if (_critLoocEnabled == val)
            return;

        _critLoocEnabled = val;
        _chatManager.DispatchServerAnnouncement(
            Loc.GetString(val ? "chat-manager-crit-looc-chat-enabled-message" : "chat-manager-crit-looc-chat-disabled-message"));
    }

    private void OnGameChange(GameRunLevelChangedEvent ev)
    {
        switch (ev.New)
        {
            case GameRunLevel.InRound:
                if (!_configurationManager.GetCVar(CCVars.OocEnableDuringRound))
                    _configurationManager.SetCVar(CCVars.OocEnabled, false);
                break;
            case GameRunLevel.PostRound:
                if (!_configurationManager.GetCVar(CCVars.OocEnableDuringRound))
                    _configurationManager.SetCVar(CCVars.OocEnabled, true);
                break;
        }
    }

    /// <summary>
    ///     Sends an in-character chat message to relevant clients.
    /// </summary>
    /// <param name="source">The entity that is speaking</param>
    /// <param name="message">The message being spoken or emoted</param>
    /// <param name="desiredType">The chat type</param>
    /// <param name="hideChat">Whether or not this message should appear in the chat window</param>
    /// <param name="hideLog">Whether or not this message should appear in the adminlog window</param>
    /// <param name="shell"></param>
    /// <param name="player">The player doing the speaking</param>
    /// <param name="nameOverride">The name to use for the speaking entity. Usually this should just be modified via <see cref="TransformSpeakerSpeechEvent"/>. If this is set, the event will not get raised.</param>
    public override void TrySendInGameICMessage(
        EntityUid source,
        string message,
        InGameICChatType desiredType,
        bool hideChat, bool hideLog = false,
        IConsoleShell? shell = null,
        ICommonSession? player = null, string? nameOverride = null,
        bool checkRadioPrefix = true,
        bool ignoreActionBlocker = false,
        LanguagePrototype? languageOverride = null)
    {
        TrySendInGameICMessage(source, message, desiredType, hideChat ? ChatTransmitRange.HideChat : ChatTransmitRange.Normal, hideLog, shell, player, nameOverride, checkRadioPrefix, ignoreActionBlocker, languageOverride: languageOverride);
    }

    /// <summary>
    ///     Sends an in-character chat message to relevant clients.
    /// </summary>
    /// <param name="source">The entity that is speaking</param>
    /// <param name="message">The message being spoken or emoted</param>
    /// <param name="desiredType">The chat type</param>
    /// <param name="range">Conceptual range of transmission, if it shows in the chat window, if it shows to far-away ghosts or ghosts at all...</param>
    /// <param name="shell"></param>
    /// <param name="player">The player doing the speaking</param>
    /// <param name="nameOverride">The name to use for the speaking entity. Usually this should just be modified via <see cref="TransformSpeakerSpeechEvent"/>. If this is set, the event will not get raised.</param>
    /// <param name="ignoreActionBlocker">If set to true, action blocker will not be considered for whether an entity can send this message.</param>
    public void TrySendInGameICMessage(
        EntityUid source,
        string message,
        InGameICChatType desiredType,
        ChatTransmitRange range,
        bool hideLog = false,
        IConsoleShell? shell = null,
        ICommonSession? player = null,
        string? nameOverride = null,
        bool checkRadioPrefix = true,
        bool ignoreActionBlocker = false,
        LanguagePrototype? languageOverride = null,
        string? color = null,
        bool separateNameAndMessage = false
        )
    {
        var isDetailed = message.StartsWith("!");

        if (HasComp<GhostComponent>(source))
        {
            // Ghosts can only send dead chat messages, so we'll forward it to InGame OOC.
            TrySendInGameOOCMessage(source, message, InGameOOCChatType.Dead, range == ChatTransmitRange.HideChat, shell, player);
            return;
        }

        if (player != null && _chatManager.HandleRateLimit(player) != RateLimitStatus.Allowed)
            return;

        // Sus
        if (player?.AttachedEntity is { Valid: true } entity && source != entity)
        {
            return;
        }

        if (!CanSendInGame(message, shell, player))
            return;
        ignoreActionBlocker = CheckIgnoreSpeechBlocker(source, ignoreActionBlocker);

        // this method is a disaster
        // every second i have to spend working with this code is fucking agony
        // scientists have to wonder how any of this was merged
        // coding any game admin feature that involves chat code is pure torture
        // changing even 10 lines of code feels like waterboarding myself
        // and i dont feel like vibe checking 50 code paths
        // so we set this here
        // todo free me from chat code
        if (player != null)
        {
            _chatManager.EnsurePlayer(player.UserId).AddEntity(GetNetEntity(source));
        }

        if (desiredType == InGameICChatType.Speak && message.StartsWith(LocalPrefix))
        {
            // prevent radios and remove prefix.
            checkRadioPrefix = false;
            message = message[1..];
        }

        if (desiredType == InGameICChatType.Subtle
            || desiredType == InGameICChatType.SubtleOOC)
            checkRadioPrefix = false;

        var language = languageOverride ?? _language.GetLanguage(source);

        var shouldCapitalize = (desiredType != InGameICChatType.Emote && desiredType != InGameICChatType.Subtle && desiredType != InGameICChatType.SubtleOOC);
        var shouldPunctuate = _configurationManager.GetCVar(CCVars.ChatPunctuation);
        // Capitalizing the word I only happens in English, so we check language here
        var shouldCapitalizeTheWordI = (!CultureInfo.CurrentCulture.IsNeutralCulture && CultureInfo.CurrentCulture.Parent.Name == "en")
            || (CultureInfo.CurrentCulture.IsNeutralCulture && CultureInfo.CurrentCulture.Name == "en");
        var keysWithinDialogue = _language.GetKeysWithinDialogue(message);
        message = SanitizeInGameICMessageDepending(
            source,
            message,
            out var emoteStr,
            shouldCapitalize,
            shouldPunctuate,
            shouldCapitalizeTheWordI,
            desiredType,
            keysWithinDialogue,
            isDetailed);

        // Was there an emote in the message? If so, send it.
        if (player != null
            && emoteStr != message
            && emoteStr != null
            && desiredType != InGameICChatType.Subtle
            && desiredType != InGameICChatType.SubtleOOC)
        {
            SendEntityEmote(source, emoteStr, range, nameOverride, language, ignoreActionBlocker);
        }

        // This can happen if the entire string is sanitized out.
        if (string.IsNullOrEmpty(message))
            return;

        // This is really terrible. I hate myself for doing this.
        if (language.SpeechOverride.ChatTypeOverride is { } chatTypeOverride)
            desiredType = chatTypeOverride;

        if (player != null && desiredType == InGameICChatType.Speak) // Imp Edit: Last Message Before Death System
            HandleLastMessageBeforeDeath(source, player, language, message);

        // This message may have a radio prefix, and should then be whispered to the resolved radio channel
        if (checkRadioPrefix)
        {
            if (TryProccessRadioMessage(source, message, out var modMessage, out var channel))
            {
                SendEntityWhisper(source, modMessage, range, channel, nameOverride, language, keysWithinDialogue, hideLog, ignoreActionBlocker);
                return;
            }
        }

        message = FormattedMessage.EscapeText(message);
        keysWithinDialogue = _language.GetKeysWithinDialogue(message);

        // Otherwise, send whatever type.
        switch (desiredType)
        {
            case InGameICChatType.Speak:
                SendEntitySpeak(source, message, range, nameOverride, language, keysWithinDialogue, hideLog, ignoreActionBlocker);
                break;
            case InGameICChatType.Whisper:
                SendEntityWhisper(source, message, range, null, nameOverride, language, keysWithinDialogue, hideLog, ignoreActionBlocker);
                break;
            case InGameICChatType.Emote:
                SendEntityEmote(source, message, range, nameOverride, language, hideLog: hideLog, ignoreActionBlocker: ignoreActionBlocker, separateNameAndMessage: separateNameAndMessage);
                break;
            case InGameICChatType.Subtle:
                SendEntitySubtle(source, message, range, nameOverride, hideLog: hideLog, ignoreActionBlocker: ignoreActionBlocker, color: color, separateNameAndMessage: separateNameAndMessage);
                break;
            case InGameICChatType.SubtleOOC:
                SendEntitySubtle(source, $"ooc: {message}", range, nameOverride, hideLog: hideLog, ignoreActionBlocker: ignoreActionBlocker, color: color);
                break;
            //Nyano - Summary: case adds the telepathic chat sending ability.
            case InGameICChatType.Telepathic:
                _telepath.SendTelepathicChat(source, message, range == ChatTransmitRange.HideChat);
                break;
        }
    }

    public void TrySendInGameOOCMessage(
        EntityUid source,
        string message,
        InGameOOCChatType type,
        bool hideChat,
        IConsoleShell? shell = null,
        ICommonSession? player = null
        )
    {
        if (!CanSendInGame(message, shell, player))
            return;

        if (player != null && _chatManager.HandleRateLimit(player) != RateLimitStatus.Allowed)
            return;

        // It doesn't make any sense for a non-player to send in-game OOC messages, whereas non-players may be sending
        // in-game IC messages.
        if (player?.AttachedEntity is not { Valid: true } entity || source != entity)
            return;

        message = SanitizeInGameOOCMessage(message);

        var sendType = type;
        // If dead player LOOC is disabled, unless you are an aghost, send dead messages to dead chat
        if (!_adminManager.IsAdmin(player) && !_deadLoocEnabled &&
            (HasComp<GhostComponent>(source) || _mobStateSystem.IsDead(source)))
            sendType = InGameOOCChatType.Dead;

        // If crit player LOOC is disabled, don't send the message at all.
        if (!_critLoocEnabled && _mobStateSystem.IsCritical(source))
            return;

        switch (sendType)
        {
            case InGameOOCChatType.Dead:
                SendDeadChat(source, player, message, hideChat);
                break;
            case InGameOOCChatType.Looc:
                SendLOOC(source, player, message, hideChat);
                break;
        }
    }
}
