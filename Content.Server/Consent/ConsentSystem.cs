using System.Linq;
using Content.Shared.Consent;
using Content.Server.Mind;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Consent;

public sealed class ConsentSystem : SharedConsentSystem
{
    [Dependency] private readonly IServerConsentManager _consent = default!;
    [Dependency] private readonly MindSystem _serverMindSystem = default!;

    protected override FormattedMessage GetConsentText(NetUserId userId)
    {
        var text = _consent.GetPlayerConsentSettings(userId).Freetext;
        if (text == string.Empty)
        {
            text = Loc.GetString("consent-examine-not-set");
        }

        var message = new FormattedMessage();
        message.AddText(text);
        return message;
    }

    public bool HasConsent(
        Entity<MindContainerComponent?> ent,
        ProtoId<ConsentTogglePrototype> consentId)
    {
        if (!Resolve(ent, ref ent.Comp)
            || _serverMindSystem.GetMind(ent, ent) is not { } mind)
        {
            return true; // NPCs as well as player characters without a mind consent to everything
        }

        if (!TryComp<MindComponent>(mind, out var mindComponent)
            || mindComponent.UserId is not { } userId)
        {
            // Not sure if this is ever reached? MindComponent seems to always have UserId.
            Log.Warning("HasConsent No UserId or missing MindComponent");
            return false; // For entities that have a mind but with no user attached, consent to nothing.
        }

        var consentSettings = _consent.GetPlayerConsentSettings(userId);
        var toggled = consentSettings.Toggles.TryGetValue(consentId, out var val) && val == "on";

        return toggled;
    }

    public bool HasConsent(
        Entity<MindContainerComponent?> ent,
        Entity<MindContainerComponent?> attempter,
        ProtoId<ConsentTogglePrototype> consentId)
    {
        if (!Resolve(ent, ref ent.Comp)
            || _serverMindSystem.GetMind(ent, ent) is not { } mind)
        {
            return true; // NPCs as well as player characters without a mind consent to everything
        }

        if (!TryComp<MindComponent>(mind, out var mindComponent)
            || mindComponent.UserId is not { } userId)
        {
            // Not sure if this is ever reached? MindComponent seems to always have UserId.
            Log.Warning("HasConsent No UserId or missing MindComponent");
            return false; // For entities that have a mind but with no user attached, consent to nothing.
        }

        var consentSettings = _consent.GetPlayerConsentSettings(userId);
        var toggled = consentSettings.Toggles.TryGetValue(consentId, out var val) && val == "on";

        if (!TryComp<ActorComponent>(attempter, out var targetActor))
            return toggled;

        if (consentSettings.Permissions.SpecifiedConsents.TryGetValue(
            targetActor.PlayerSession.UserId,
            out var permissions)
            && permissions.Any(o => o.ConsentToggleId == consentId))
            return permissions.SingleOrDefault(o => o.ConsentToggleId == consentId).HasConsent;

        return toggled;
    }
}
