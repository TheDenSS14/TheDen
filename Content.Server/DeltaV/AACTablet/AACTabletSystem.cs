using Content.Server.Abilities.Mime;
using Content.Server.Chat.Systems;
using Content.Shared.Chat;
using Content.Server.VoiceMask;
using Content.Server.Speech.Components;
using Content.Shared.Chat;
using Content.Shared.DeltaV.AACTablet;
using Content.Shared.IdentityManagement;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server.DeltaV.AACTablet;

public sealed class AACTabletSystem : EntitySystem
{
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly MimePowersSystem _mimePowers = default!;

    private readonly List<string> _localisedPhrases = [];

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AACTabletComponent, AACTabletSendPhraseMessage>(OnSendPhrase);
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

        _chat.TrySendInGameICMessage(ent,
            _chat.SanitizeMessageCapital(string.Join(" ", _localisedPhrases)),
            InGameICChatType.Speak,
            hideChat: false,
            nameOverride: speakerName);

        var curTime = _timing.CurTime;
        ent.Comp.NextPhrase = curTime + ent.Comp.Cooldown;
    }
}
