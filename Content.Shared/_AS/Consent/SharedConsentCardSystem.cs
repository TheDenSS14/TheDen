using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Shared._AS.Consent;

[Virtual]
public class SharedConsentCardSystem : EntitySystem
{
    public void RaiseConsentCard(NetUserId playerId, EntProtoId cardId)
    {
        // We need this handled by the server, so receive message sent by client and raise network event.
        var msg = new ConsentCardRaisedEvent(playerId, cardId);
        RaiseNetworkEvent(msg);
    }
}
