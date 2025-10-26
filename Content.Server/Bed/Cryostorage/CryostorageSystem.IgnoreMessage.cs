using Content.Server._DEN.Bed.Cryostorage;
using Content.Server.Administration.Logs;
using Content.Server.Station.Components;
using Content.Server.Station.Systems;
using Content.Shared._DEN.Bed.Cryostorage.Components;
using Content.Shared.Bed.Cryostorage;
using Content.Shared.Chat;
using Content.Shared.Database;
using Robust.Shared.Utility;


namespace Content.Server.Bed.Cryostorage;


public sealed partial class CryostorageSystem : SharedCryostorageSystem
{
    [Dependency] private readonly StationSystem _stationSystem = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;

    partial void InitializeIgnoreMessage() => SubscribeNetworkEvent<IgnoreCryoMessage>(OnIgnoreRequest);

    private void OnIgnoreRequest(IgnoreCryoMessage msg, EntitySessionEventArgs args)
    {
        Logger.Info("Got ignore request!");

        var senderSession = args.SenderSession;

        // Whoever sent this actually has a character.
        if (senderSession.AttachedEntity is null)
            return;

        var entity = senderSession.AttachedEntity.Value;

        if (msg.Ignore)
            AddComp<IgnoringCryoMessagesComponent>(entity);
        else
            RemComp<IgnoringCryoMessagesComponent>(entity);
    }

    // This is essentially a copy of the _chatSystem's DispatchStationAnnouncement with the added benefit of being
    // able to filter out people who have the 'IgnoringCryoMessagesComponent'.
    partial void DispatchStationAnnouncement(EntityUid source, string message)
    {
        var sender = Loc.GetString("earlyleave-cryo-sender");

        var wrappedMessage = Loc.GetString(
            "chat-manager-sender-announcement-wrap-message",
            ("sender", sender),
            ("message", FormattedMessage.EscapeText(message)));

        var station = _stationSystem.GetOwningStation(source);
        // Can't send an announcement with no station.
        if (station == null)
            return;

        if (!EntityManager.TryGetComponent<StationDataComponent>(station, out var stationDataComp))
            return;

        var filter = _stationSystem.GetInStation(stationDataComp);
        filter.RemoveWhereAttachedEntity(ent => HasComp<IgnoringCryoMessagesComponent>(ent));

        _chatManager.ChatMessageToManyFiltered(filter, ChatChannel.Radio, message, wrappedMessage, source, false, true, null);
        _adminLogger.Add(LogType.Chat, LogImpact.Low, $"Station Announcement on {station} from {sender}: {message}");
    }
}
