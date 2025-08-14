using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._AS.Consent;

[Serializable, NetSerializable]
public sealed class ConsentCardRaisedEvent(NetUserId playerId, EntProtoId cardId) : EntityEventArgs
{
    public NetUserId PlayerId = playerId;
    public EntProtoId CardId = cardId;
}
