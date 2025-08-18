// SPDX-FileCopyrightText: 2024 Milon
// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2024 portfiend
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 MajorMoth
// SPDX-FileCopyrightText: 2025 little-meow-meow
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Abilities.Mime;
using Content.Server.Chat.Systems;
using Content.Shared.Chat;
using Content.Server.VoiceMask;
using Content.Server.Radio.Components; // starcup
using Content.Server.Speech.Components;
using Content.Shared.Chat;
using Content.Shared._DV.AACTablet;
using Content.Shared.IdentityManagement;
using Robust.Server.GameObjects; // starcup
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server._DV.AACTablet;

public sealed partial class AACTabletSystem : EntitySystem // starcup: made partial
{
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly MimePowersSystem _mimePowers = default!;
    [Dependency] private readonly UserInterfaceSystem _userInterface = default!; // starcup

    private readonly List<string> _localisedPhrases = [];

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AACTabletComponent, AACTabletSendPhraseMessage>(OnSendPhrase);

        // begin starcup
        Subs.BuiEvents<AACTabletComponent>(AACTabletKey.Key, subs =>
        {
            subs.Event<BoundUIOpenedEvent>(OnBoundUIOpened);
        });
        // end starcup
    }

    private void OnSendPhrase(Entity<AACTabletComponent> ent, ref AACTabletSendPhraseMessage message)
    {
        if (ent.Comp.NextPhrase > _timing.CurTime)
            return;

        var senderName = Identity.Entity(message.Actor, EntityManager);
        var speakerName = Loc.GetString("speech-name-relay",
            ("speaker", Name(ent)),
            ("originalName", senderName));

        _localisedPhrases.Clear();
        foreach (var phraseProto in message.PhraseIds)
        {
            if (_prototype.TryIndex(phraseProto, out var phrase))
            {
                // removed capitalization
                _localisedPhrases.Add(Loc.GetString(phrase.Text));
            }
        }

        if (_localisedPhrases.Count <= 0)
            return;

        if (HasComp<MimePowersComponent>(message.Actor))
            _mimePowers.BreakVow(message.Actor);

        EnsureComp<VoiceOverrideComponent>(ent).NameOverride = speakerName;

        // begin starcup: Radio support
        // Set the player's currently available channels before sending the message
        EnsureComp(ent, out IntrinsicRadioTransmitterComponent transmitter);
        transmitter.Channels = GetAvailableChannels(message.Actor);
        // end starcup

        _chat.TrySendInGameICMessage(ent,
            message.Prefix + _chat.SanitizeMessageCapital(string.Join(" ", _localisedPhrases)), // starcup: prefix
            InGameICChatType.Speak,
            hideChat: false,
            nameOverride: speakerName);

        var curTime = _timing.CurTime;
        ent.Comp.NextPhrase = curTime + ent.Comp.Cooldown;
    }
}
