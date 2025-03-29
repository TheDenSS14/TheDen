using System.Linq;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Consent;

[Serializable, NetSerializable]
public sealed class PlayerConsentSettings
{
    public string Freetext;
    public Dictionary<ProtoId<ConsentTogglePrototype>, string> Toggles;
    public ConsentPermissions Permissions;

    public PlayerConsentSettings()
    {
        Freetext = string.Empty;
        Toggles = new();
        Permissions = new();
    }

    public PlayerConsentSettings(
        string freetext,
        Dictionary<ProtoId<ConsentTogglePrototype>, string> toggles,
        ConsentPermissions permissions)
    {
        Freetext = freetext;
        Toggles = toggles;
        Permissions = permissions;
    }

    public void EnsureValid(IConfigurationManager configManager, IPrototypeManager prototypeManager)
    {
        var maxLength = configManager.GetCVar(CCVars.ConsentFreetextMaxLength);
        Freetext = Freetext.Trim();
        if (Freetext.Length > maxLength)
            Freetext = Freetext.Substring(0, maxLength);

        Toggles = Toggles.Where(t =>
            prototypeManager.HasIndex<ConsentTogglePrototype>(t.Key)
            && t.Value == "on"
        ).ToDictionary();
    }
}

[Serializable, NetSerializable]
public struct ConsentPermissions
{
    public Guid UserId;
    public Dictionary<Guid, List<ConsentOption>> SpecifiedConsents;

    public ConsentPermissions()
    {
        UserId = Guid.Empty;
        SpecifiedConsents = new();
    }

    public ConsentPermissions(Guid userId)
    {
        UserId = userId;
        SpecifiedConsents = new();
    }

    public ConsentPermissions(Guid userId, Dictionary<Guid, List<ConsentOption>> specifiedConsents)
    {
        UserId = userId;
        SpecifiedConsents = specifiedConsents;
    }
}

[Serializable, NetSerializable]
public struct ConsentOption
{
    public string ConsentToggleId;
    public Consenting IsConsenting;

    public bool HasConsent => IsConsenting == Consenting.Allow;

    public void SetConsent(bool? value)
    {
        switch (value)
        {
            case true:
                IsConsenting = Consenting.Allow;
                break;
            case false:
                IsConsenting = Consenting.Reject;
                break;
            default:
                IsConsenting = Consenting.Default;
                break;
        }
    }
}

[Serializable, NetSerializable]
public enum Consenting
{
    Allow,
    Reject,
    Default
}
