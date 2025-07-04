using Robust.Shared.GameStates;
using Robust.Shared.Serialization;


namespace Content.Shared._Floof.Examine;


[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class CustomExamineComponent : Component
{
    // This is simply so that the client can know its current custom examine messages
    // Other client will dynamically receive it over the network as needed to avoid lag
    public override bool SendOnlyToOwner => true;

    [DataField, AutoNetworkedField]
    public CustomExamineData PublicData = new()
    {
        Content = null,
        VisibilityRange = 20,
        ExpireTime = TimeSpan.Zero,
        RequiresConsent = false,
        LastUpdate = TimeSpan.Zero
    };

    [DataField, AutoNetworkedField]
    public CustomExamineData SubtleData = new()
    {
        Content = null,
        VisibilityRange = 2,
        ExpireTime = TimeSpan.Zero,
        RequiresConsent = false,
        LastUpdate = TimeSpan.Zero
    };
}

[DataDefinition, Serializable, NetSerializable]
public partial struct CustomExamineData
{
    [DataField]
    public string? Content;

    [DataField]
    public int VisibilityRange;

    /// <summary>
    ///     GameTime at which the message expires. Can be zero to never expire.
    /// </summary>
    [DataField]
    public TimeSpan ExpireTime;

    /// <summary>
    ///     Whether the text should only be shown if the examiner consents to seeing ERP descriptions.
    /// </summary>
    [DataField]
    public bool RequiresConsent;

    /// <summary>
    ///     Last time the message was updated, used in the UI to prevent accidental overwrites.
    /// </summary>
    [DataField]
    public TimeSpan LastUpdate;
}
