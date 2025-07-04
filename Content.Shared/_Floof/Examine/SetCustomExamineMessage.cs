using Robust.Shared.Serialization;


namespace Content.Shared._Floof.Examine;


/// <summary>
///     Raised client->server to update its entity's custom examine message.
/// </summary>
[Serializable, NetSerializable]
public sealed class SetCustomExamineMessage : EntityEventArgs
{
    public NetEntity Target;

    public CustomExamineData PublicData, SubtleData;
}
