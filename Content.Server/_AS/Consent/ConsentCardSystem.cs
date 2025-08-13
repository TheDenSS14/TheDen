using Content.Server.Administration.Managers;
using Content.Shared._AS.Consent;
using Content.Shared._AS.Consent.Components;
using Content.Shared.Administration.Logs;
using Content.Shared.Chat;
using Content.Shared.Coordinates;
using Content.Shared.Database;
using Content.Shared.Popups;
using Robust.Server.Player;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;

namespace Content.Server._AS.Consent;

public sealed class ConsentCardSystem : SharedConsentCardSystem
{
    [Dependency] private readonly ISharedAdminLogManager _adminLog = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly ISharedChatManager _chat = default!;
    [Dependency] private readonly IAdminManager _admin = default!;
    [Dependency] private readonly IPlayerManager _player = default!;

    public override void Initialize()
    {
        SubscribeNetworkEvent<ConsentCardRaisedEvent>(OnConsentCardRaised);
        base.Initialize();
    }

    private void OnConsentCardRaised(ConsentCardRaisedEvent ev)
    {
        var session = _player.GetSessionById(ev.PlayerId);
        if (session.AttachedEntity is not { } playerEnt)
        {
            Log.Error($"{ev.PlayerId} attempted to raise {ev.CardId} card but could not find an attached entity.");
            _adminLog.Add(LogType.Consent, LogImpact.Extreme,$"{ev.PlayerId} attempted to raise {ev.CardId} card but could not find an attached entity.");
            _chat.SendAdminAlert(Loc.GetString("unknown-consent-card-admin-message", ("player", session.Name), ("type", ev.CardId)));
            _audio.PlayGlobal(new SoundPathSpecifier("/Audio/Admin/ahelp_send.ogg"), Filter.Empty().AddPlayers(_admin.ActiveAdmins), false);
            return;
        }

        var card = SpawnAttachedTo(ev.CardId, playerEnt.ToCoordinates());
        _adminLog.Add(LogType.Consent, LogImpact.Extreme,
            $"{ev.PlayerId} raised a {ev.CardId} card.");

        if (!TryComp<ConsentCardComponent>(card, out var cardComp))
        {
            Log.Error($"{ev.PlayerId} attempted to raise {ev.CardId} card but was not recognized as a consent card.");
            _adminLog.Add(LogType.Consent, LogImpact.Extreme, $"{ev.PlayerId} attempted to raise {ev.CardId} card but was not recognized as a consent card.");
            _chat.SendAdminAlert(Loc.GetString("unknown-consent-card-admin-message", ("player", session.Name), ("type", ev.CardId)));
            _audio.PlayGlobal(new SoundPathSpecifier("/Audio/Admin/ahelp_send.ogg"), Filter.Empty().AddPlayers(_admin.ActiveAdmins), false);
            return;
        }

        string cardName = ev.CardId;
        if (TryComp(card, out MetaDataComponent? cardMeta))
            cardName = cardMeta.EntityName;

        _adminLog.Add(LogType.Consent, LogImpact.Extreme, $"{ev.PlayerId} raised {cardName} card.");
        if (cardComp.AdminMessage is {} adminMessage)
            _chat.SendAdminAlert(Loc.GetString(cardComp.AdminMessage, ("player", session.Name), ("type", cardName)));
        _audio.PlayGlobal(new SoundPathSpecifier("/Audio/Admin/ahelp_send.ogg"), Filter.Empty().AddPlayers(_admin.ActiveAdmins), false);
        _popup.PopupPredicted(string.Empty, Loc.GetString(cardComp.PopupMessage), playerEnt, playerEnt, PopupType.MediumCaution);
    }
}
